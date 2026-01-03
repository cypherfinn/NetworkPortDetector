using System;
using System.Drawing;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using SharpPcap;
using PacketDotNet;
using PacketDotNet.Lldp;

namespace NetworkPortDetector
{
    public class MainForm : Form
    {
        private ListView listView;
        private Button refreshButton;
        private Button startCaptureButton;
        private Label statusLabel;
        private TextBox detailsTextBox;
        private Dictionary<string, SwitchPortInfo> switchInfoCache = new Dictionary<string, SwitchPortInfo>();
        private List<ICaptureDevice> activeCaptures = new List<ICaptureDevice>();
        private bool isCapturing = false;

        public MainForm()
        {
            InitializeUI();
            RefreshNetworkInfo();
        }

        private void InitializeUI()
        {
            this.Text = "Network Port Detector - Switch Port Finder";
            this.Size = new Size(1000, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Button Panel (add first because Dock.Bottom)
            var buttonPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 50,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(5)
            };

            startCaptureButton = new Button
            {
                Text = "LLDP Erfassung starten",
                Width = 180,
                Height = 40,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                BackColor = Color.LightGreen
            };
            startCaptureButton.Click += (s, e) => ToggleCapture();
            buttonPanel.Controls.Add(startCaptureButton);

            refreshButton = new Button
            {
                Text = "Aktualisieren",
                Width = 120,
                Height = 40,
                Font = new Font("Segoe UI", 9)
            };
            refreshButton.Click += (s, e) => RefreshNetworkInfo();
            buttonPanel.Controls.Add(refreshButton);

            var infoButton = new Button
            {
                Text = "Hilfe",
                Width = 100,
                Height = 40
            };
            infoButton.Click += (s, e) => ShowHelp();
            buttonPanel.Controls.Add(infoButton);

            this.Controls.Add(buttonPanel);

            // Status Label (add after bottom controls)
            statusLabel = new Label
            {
                Text = "Bereit - Klicken Sie auf 'LLDP Erfassung starten'",
                Dock = DockStyle.Top,
                Height = 30,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(10, 0, 0, 0),
                BackColor = Color.LightBlue,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            this.Controls.Add(statusLabel);

            // ListView
            listView = new ListView
            {
                Dock = DockStyle.Top,
                Height = 250,
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                Font = new Font("Consolas", 9)
            };

            listView.Columns.Add("Netzwerkadapter", 200);
            listView.Columns.Add("Switch Name", 150);
            listView.Columns.Add("Switch Port", 150);
            listView.Columns.Add("IP Adresse", 120);
            listView.Columns.Add("MAC Adresse", 140);
            listView.Columns.Add("Status", 100);

            listView.SelectedIndexChanged += ListView_SelectedIndexChanged;
            this.Controls.Add(listView);

            // Details Label
            var detailsLabel = new Label
            {
                Text = "Details & LLDP Informationen:",
                Dock = DockStyle.Top,
                Height = 25,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(10, 5, 0, 0),
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            this.Controls.Add(detailsLabel);

            // Details TextBox (Fill remaining space)
            detailsTextBox = new TextBox
            {
                Dock = DockStyle.Fill,
                Multiline = true,
                ScrollBars = ScrollBars.Both,
                ReadOnly = true,
                Font = new Font("Consolas", 9),
                BackColor = Color.White,
                Text = "WICHTIG: Diese App benötigt Administrator-Rechte und Npcap/WinPcap!\r\n\r\n" +
                       "1. Starten Sie die App als Administrator (Rechtsklick -> Als Administrator ausführen)\r\n" +
                       "2. Installieren Sie Npcap von https://npcap.com wenn noch nicht vorhanden\r\n" +
                       "3. Klicken Sie auf 'LLDP Erfassung starten'\r\n\r\n" +
                       "Die App wird dann LLDP-Pakete vom Switch empfangen und Switch-Name + Port anzeigen."
            };
            this.Controls.Add(detailsTextBox);
        }

        private void ToggleCapture()
        {
            if (isCapturing)
            {
                StopCapture();
            }
            else
            {
                StartCapture();
            }
        }

        private void StartCapture()
        {
            try
            {
                var devices = CaptureDeviceList.Instance;

                if (devices.Count == 0)
                {
                    MessageBox.Show(
                        "Keine Netzwerkgeräte gefunden!\n\n" +
                        "Mögliche Ursachen:\n" +
                        "1. Npcap/WinPcap ist nicht installiert\n" +
                        "2. Die App läuft nicht als Administrator\n\n" +
                        "Lösung:\n" +
                        "- Installieren Sie Npcap von https://npcap.com\n" +
                        "- Starten Sie die App als Administrator",
                        "Fehler",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    return;
                }

                // Show adapter selection dialog
                var selectedDevices = ShowAdapterSelectionDialog(devices);
                if (selectedDevices == null || selectedDevices.Count == 0)
                {
                    return; // User cancelled
                }

                int devicesStarted = 0;
                var errors = new List<string>();

                foreach (var device in selectedDevices)
                {
                    try
                    {
                        device.Open(DeviceModes.Promiscuous, 1000);
                        device.Filter = "ether proto 0x88cc or (ether dst 01:00:0c:cc:cc:cc and ether[20:2] == 0x2000)";
                        device.OnPacketArrival += Device_OnPacketArrival;
                        device.StartCapture();
                        activeCaptures.Add(device);
                        devicesStarted++;
                    }
                    catch (Exception ex)
                    {
                        errors.Add($"{device.Description}: {ex.Message}");
                    }
                }

                if (devicesStarted > 0)
                {
                    isCapturing = true;
                    startCaptureButton.Text = "LLDP Erfassung stoppen";
                    startCaptureButton.BackColor = Color.LightCoral;
                    statusLabel.Text = $"LLDP Erfassung läuft auf {devicesStarted} Adapter(n) - Warte auf Pakete...";
                    statusLabel.BackColor = Color.LightGreen;

                    detailsTextBox.Text = $"LLDP-Erfassung gestartet auf {devicesStarted} Netzwerkadapter(n).\r\n\r\n";
                    detailsTextBox.AppendText("Warte auf LLDP-Pakete vom Switch...\r\n");
                    detailsTextBox.AppendText("(LLDP-Pakete werden normalerweise alle 30 Sekunden gesendet)\r\n\r\n");

                    if (errors.Count > 0)
                    {
                        detailsTextBox.AppendText("Hinweise:\r\n");
                        foreach (var error in errors)
                        {
                            detailsTextBox.AppendText($"- {error}\r\n");
                        }
                    }
                }
                else
                {
                    MessageBox.Show(
                        "Konnte keine Netzwerkgeräte öffnen!\n\n" +
                        "Die App muss als Administrator gestartet werden.\n\n" +
                        "Fehler:\n" + string.Join("\n", errors.Take(5)),
                        "Fehler",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Fehler beim Starten der Erfassung:\n\n{ex.Message}\n\n" +
                    "Stellen Sie sicher, dass:\n" +
                    "1. Npcap installiert ist\n" +
                    "2. Die App als Administrator läuft",
                    "Fehler",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private List<ICaptureDevice>? ShowAdapterSelectionDialog(CaptureDeviceList devices)
        {
            var selectionForm = new Form
            {
                Text = "Netzwerkadapter auswählen",
                Size = new Size(700, 500),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            var label = new Label
            {
                Text = "Wählen Sie die Netzwerkadapter aus, auf denen LLDP erfasst werden soll:",
                Dock = DockStyle.Top,
                Height = 40,
                Padding = new Padding(10, 10, 10, 0),
                Font = new Font("Segoe UI", 10)
            };
            selectionForm.Controls.Add(label);

            var checkedListBox = new CheckedListBox
            {
                Dock = DockStyle.Fill,
                CheckOnClick = true,
                Font = new Font("Consolas", 9)
            };

            // Match network interfaces with capture devices
            var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces()
                .Where(ni => ni.OperationalStatus == OperationalStatus.Up &&
                             ni.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                .ToDictionary(ni => ni.Description, ni => ni);

            foreach (var device in devices)
            {
                var desc = device.Description ?? device.Name;
                var displayName = desc;

                // Try to find matching network interface for IP address
                if (networkInterfaces.TryGetValue(desc, out var ni))
                {
                    var ipAddress = ni.GetIPProperties().UnicastAddresses
                        .Where(addr => addr.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        .Select(addr => addr.Address.ToString())
                        .FirstOrDefault();

                    if (!string.IsNullOrEmpty(ipAddress))
                    {
                        displayName = $"{desc} - {ipAddress}";
                    }
                }

                checkedListBox.Items.Add(new DeviceItem { Device = device, DisplayName = displayName });
            }

            // Pre-select active adapters
            for (int i = 0; i < checkedListBox.Items.Count; i++)
            {
                var item = (DeviceItem)checkedListBox.Items[i];
                if (networkInterfaces.ContainsKey(item.Device.Description ?? item.Device.Name))
                {
                    checkedListBox.SetItemChecked(i, true);
                }
            }

            selectionForm.Controls.Add(checkedListBox);

            var buttonPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 50,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(10)
            };

            var cancelButton = new Button
            {
                Text = "Abbrechen",
                Width = 100,
                Height = 35,
                DialogResult = DialogResult.Cancel
            };
            buttonPanel.Controls.Add(cancelButton);

            var okButton = new Button
            {
                Text = "OK",
                Width = 100,
                Height = 35,
                DialogResult = DialogResult.OK
            };
            buttonPanel.Controls.Add(okButton);

            var selectAllButton = new Button
            {
                Text = "Alle auswählen",
                Width = 120,
                Height = 35
            };
            selectAllButton.Click += (s, e) =>
            {
                for (int i = 0; i < checkedListBox.Items.Count; i++)
                    checkedListBox.SetItemChecked(i, true);
            };
            buttonPanel.Controls.Add(selectAllButton);

            var selectNoneButton = new Button
            {
                Text = "Keine auswählen",
                Width = 120,
                Height = 35
            };
            selectNoneButton.Click += (s, e) =>
            {
                for (int i = 0; i < checkedListBox.Items.Count; i++)
                    checkedListBox.SetItemChecked(i, false);
            };
            buttonPanel.Controls.Add(selectNoneButton);

            selectionForm.Controls.Add(buttonPanel);

            selectionForm.AcceptButton = okButton;
            selectionForm.CancelButton = cancelButton;

            if (selectionForm.ShowDialog() == DialogResult.OK)
            {
                var selected = new List<ICaptureDevice>();
                foreach (DeviceItem item in checkedListBox.CheckedItems)
                {
                    selected.Add(item.Device);
                }
                return selected;
            }

            return null;
        }

        private void StopCapture()
        {
            foreach (var device in activeCaptures)
            {
                try
                {
                    device.StopCapture();
                    device.Close();
                }
                catch { }
            }

            activeCaptures.Clear();
            isCapturing = false;
            startCaptureButton.Text = "LLDP Erfassung starten";
            startCaptureButton.BackColor = Color.LightGreen;
            statusLabel.Text = "LLDP Erfassung gestoppt";
            statusLabel.BackColor = Color.LightYellow;
        }

        private void Device_OnPacketArrival(object sender, PacketCapture e)
        {
            try
            {
                var rawPacket = e.GetPacket();
                var packet = Packet.ParsePacket(rawPacket.LinkLayerType, rawPacket.Data);
                var ethernetPacket = packet.Extract<EthernetPacket>();

                if (ethernetPacket == null) return;

                // LLDP (0x88cc)
                if (ethernetPacket.Type == EthernetType.Lldp)
                {
                    var lldpPacket = packet.Extract<LldpPacket>();
                    if (lldpPacket != null)
                    {
                        ProcessLLDPPacket(sender as ICaptureDevice, lldpPacket);
                    }
                }
                // CDP detection removed for now (complex to parse)
            }
            catch
            {
                // Ignore packet parsing errors
            }
        }

        private void ProcessLLDPPacket(ICaptureDevice? device, LldpPacket lldpPacket)
        {
            if (device == null) return;

            var info = new SwitchPortInfo
            {
                InterfaceName = device.Description ?? device.Name,
                LastUpdate = DateTime.Now,
                Protocol = "LLDP"
            };

            foreach (var tlv in lldpPacket.TlvCollection)
            {
                try
                {
                    var tlvType = (int)tlv.Type;

                    // TLV Types: 1=Chassis ID, 2=Port ID, 3=TTL, 4=Port Desc, 5=System Name, 6=System Desc
                    if (tlvType == 5) // System Name
                    {
                        if (tlv.Bytes.Length > 2)
                            info.SwitchName = System.Text.Encoding.UTF8.GetString(tlv.Bytes, 2, tlv.Bytes.Length - 2);
                    }
                    else if (tlvType == 2) // Port ID
                    {
                        if (tlv.Bytes.Length > 2)
                        {
                            var portId = System.Text.Encoding.UTF8.GetString(tlv.Bytes, 2, tlv.Bytes.Length - 2);
                            info.SwitchPort = portId;
                        }
                    }
                    else if (tlvType == 4) // Port Description
                    {
                        if (tlv.Bytes.Length > 2)
                        {
                            var portDesc = System.Text.Encoding.UTF8.GetString(tlv.Bytes, 2, tlv.Bytes.Length - 2);
                            if (string.IsNullOrEmpty(info.SwitchPort))
                                info.SwitchPort = portDesc;
                            else
                                info.PortDescription = portDesc;
                        }
                    }
                    else if (tlvType == 6) // System Description
                    {
                        if (tlv.Bytes.Length > 2)
                            info.SystemDescription = System.Text.Encoding.UTF8.GetString(tlv.Bytes, 2, tlv.Bytes.Length - 2);
                    }
                }
                catch { }
            }

            if (!string.IsNullOrEmpty(info.SwitchName) || !string.IsNullOrEmpty(info.SwitchPort))
            {
                lock (switchInfoCache)
                {
                    switchInfoCache[device.Name] = info;
                }

                this.Invoke((MethodInvoker)delegate
                {
                    RefreshNetworkInfo();
                    statusLabel.Text = $"LLDP Paket empfangen von {info.SwitchName} - Port: {info.SwitchPort}";
                    statusLabel.BackColor = Color.LightGreen;
                });
            }
        }


        private void ListView_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (listView.SelectedItems.Count > 0)
            {
                var selectedItem = listView.SelectedItems[0];
                var adapterName = selectedItem.Text;
                ShowAdapterDetails(adapterName);
            }
        }

        private void ShowAdapterDetails(string adapterName)
        {
            var details = new System.Text.StringBuilder();
            details.AppendLine($"=== Details für: {adapterName} ===");
            details.AppendLine();

            try
            {
                var adapter = NetworkInterface.GetAllNetworkInterfaces()
                    .FirstOrDefault(ni => ni.Name == adapterName);

                if (adapter != null)
                {
                    details.AppendLine($"Beschreibung: {adapter.Description}");
                    details.AppendLine($"Typ: {adapter.NetworkInterfaceType}");
                    details.AppendLine($"Status: {adapter.OperationalStatus}");
                    details.AppendLine($"Geschwindigkeit: {adapter.Speed / 1_000_000} Mbps");
                    details.AppendLine($"MAC-Adresse: {adapter.GetPhysicalAddress()}");
                    details.AppendLine();

                    var ipProps = adapter.GetIPProperties();

                    details.AppendLine("IP-Adressen:");
                    foreach (var ip in ipProps.UnicastAddresses)
                    {
                        details.AppendLine($"  - {ip.Address} (/{ip.PrefixLength})");
                    }
                    details.AppendLine();

                    details.AppendLine("DNS-Server:");
                    foreach (var dns in ipProps.DnsAddresses)
                    {
                        details.AppendLine($"  - {dns}");
                    }
                    details.AppendLine();

                    details.AppendLine("Gateway:");
                    foreach (var gateway in ipProps.GatewayAddresses)
                    {
                        details.AppendLine($"  - {gateway.Address}");
                    }
                    details.AppendLine();

                    // LLDP Info
                    details.AppendLine("=== LLDP/CDP Switch Informationen ===");
                    details.AppendLine();

                    var matchingInfo = switchInfoCache.Values.FirstOrDefault(info =>
                        info.InterfaceName.Contains(adapter.Description) ||
                        adapter.Description.Contains(info.InterfaceName));

                    if (matchingInfo != null)
                    {
                        details.AppendLine($"Protokoll: {matchingInfo.Protocol}");
                        details.AppendLine($"Switch Name: {matchingInfo.SwitchName ?? "N/A"}");
                        details.AppendLine($"Switch Port: {matchingInfo.SwitchPort ?? "N/A"}");
                        if (!string.IsNullOrEmpty(matchingInfo.PortDescription))
                            details.AppendLine($"Port Beschreibung: {matchingInfo.PortDescription}");
                        if (!string.IsNullOrEmpty(matchingInfo.SystemDescription))
                            details.AppendLine($"System: {matchingInfo.SystemDescription}");
                        details.AppendLine($"Letzte Aktualisierung: {matchingInfo.LastUpdate:HH:mm:ss}");

                        var age = (DateTime.Now - matchingInfo.LastUpdate).TotalSeconds;
                        details.AppendLine($"Alter: {age:F0} Sekunden");
                    }
                    else
                    {
                        details.AppendLine("Keine LLDP/CDP-Informationen empfangen.");
                        details.AppendLine();
                        details.AppendLine("Stellen Sie sicher, dass:");
                        details.AppendLine("- LLDP Erfassung läuft (Button oben)");
                        details.AppendLine("- LLDP auf dem Switch aktiviert ist");
                        details.AppendLine("- Die App als Administrator läuft");
                        details.AppendLine("- Npcap installiert ist");
                    }
                }
            }
            catch (Exception ex)
            {
                details.AppendLine($"Fehler beim Laden der Details: {ex.Message}");
            }

            detailsTextBox.Text = details.ToString();
        }

        private void RefreshNetworkInfo()
        {
            listView.Items.Clear();

            var interfaces = NetworkInterface.GetAllNetworkInterfaces()
                .Where(ni => ni.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                .OrderByDescending(ni => ni.OperationalStatus)
                .ThenBy(ni => ni.Name);

            int activeCount = 0;
            int lldpCount = 0;

            foreach (var ni in interfaces)
            {
                var item = new ListViewItem(ni.Name);

                // Get IP Address
                var ipProps = ni.GetIPProperties();
                var ipAddress = ipProps.UnicastAddresses
                    .Where(addr => addr.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    .Select(addr => addr.Address.ToString())
                    .FirstOrDefault() ?? "-";

                // Get MAC Address
                var macAddress = string.Join(":", ni.GetPhysicalAddress()
                    .GetAddressBytes()
                    .Select(b => b.ToString("X2")));

                if (string.IsNullOrEmpty(macAddress))
                    macAddress = "-";

                // Check for LLDP info
                string switchName = "-";
                string switchPort = "-";
                string status = ni.OperationalStatus.ToString();

                var matchingInfo = switchInfoCache.Values.FirstOrDefault(info =>
                    info.InterfaceName.Contains(ni.Description) ||
                    ni.Description.Contains(info.InterfaceName));

                if (matchingInfo != null)
                {
                    switchName = matchingInfo.SwitchName ?? "-";
                    switchPort = matchingInfo.SwitchPort ?? "-";
                    var age = (DateTime.Now - matchingInfo.LastUpdate).TotalSeconds;
                    status = age < 60 ? "LLDP Aktiv" : $"LLDP ({age:F0}s alt)";
                    lldpCount++;
                }

                item.SubItems.Add(switchName);
                item.SubItems.Add(switchPort);
                item.SubItems.Add(ipAddress);
                item.SubItems.Add(macAddress);
                item.SubItems.Add(status);

                // Color coding
                if (matchingInfo != null && (DateTime.Now - matchingInfo.LastUpdate).TotalSeconds < 60)
                {
                    item.BackColor = Color.LightGreen;
                    item.Font = new Font(item.Font, FontStyle.Bold);
                }
                else if (ni.OperationalStatus == OperationalStatus.Up)
                {
                    item.BackColor = Color.LightYellow;
                    activeCount++;
                }
                else
                {
                    item.BackColor = Color.LightGray;
                    item.ForeColor = Color.DarkGray;
                }

                listView.Items.Add(item);
            }

            if (!isCapturing)
            {
                statusLabel.Text = $"{activeCount} aktive Verbindung(en) - Klicken Sie auf 'LLDP Erfassung starten'";
            }
            else if (lldpCount > 0)
            {
                statusLabel.Text = $"LLDP Erfassung läuft - {lldpCount} Switch-Verbindung(en) erkannt";
                statusLabel.BackColor = Color.LightGreen;
            }
        }

        private void ShowHelp()
        {
            var helpText = @"Network Port Detector - Hilfe

VORAUSSETZUNGEN:
1. Npcap installiert (https://npcap.com)
2. App als Administrator ausführen
3. LLDP auf Switch aktiviert

VERWENDUNG:
1. App als Administrator starten (Rechtsklick -> Als Admin)
2. Auf 'LLDP Erfassung starten' klicken
3. Warten auf LLDP-Pakete (alle 30s vom Switch)
4. Switch-Name und Port werden automatisch angezeigt

FARBCODIERUNG:
- Grün + Fett = LLDP empfangen (Switch-Info verfügbar)
- Gelb = Aktiv, kein LLDP empfangen
- Grau = Inaktiv

TASTEN:
F5 - Aktualisieren

HINWEISE:
- LLDP-Pakete werden ca. alle 30 Sekunden gesendet
- Bei CDP (Cisco) kann es länger dauern
- Nicht alle Switches senden LLDP standardmäßig
";

            MessageBox.Show(helpText, "Hilfe", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F5)
            {
                RefreshNetworkInfo();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            StopCapture();
            base.OnFormClosing(e);
        }
    }

    public class SwitchPortInfo
    {
        public string InterfaceName { get; set; } = string.Empty;
        public string? SwitchName { get; set; }
        public string? SwitchPort { get; set; }
        public string? PortDescription { get; set; }
        public string? SystemDescription { get; set; }
        public string Protocol { get; set; } = "LLDP";
        public DateTime LastUpdate { get; set; }
    }

    public class DeviceItem
    {
        public ICaptureDevice Device { get; set; } = null!;
        public string DisplayName { get; set; } = string.Empty;

        public override string ToString()
        {
            return DisplayName;
        }
    }
}
