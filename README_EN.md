# Network Port Detector - LLDP Switch Port Finder

> 🇩🇪 [Deutsche Version](README_DE.md) | 🇬🇧 **English Version**

A portable Windows application for **automatic** detection of switch port connections via LLDP (Link Layer Discovery Protocol).

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![.NET](https://img.shields.io/badge/.NET-8.0-blue)](https://dotnet.microsoft.com/download/dotnet/8.0)
[![Platform](https://img.shields.io/badge/platform-Windows-lightgrey)](https://www.microsoft.com/windows)
[![Release](https://img.shields.io/github/v/release/cypherfinn/NetworkPortDetector)](https://github.com/cypherfinn/NetworkPortDetector/releases/latest)

## 📥 Download

**[⬇️ Download NetworkPortDetector.exe (v1.0.0)](https://github.com/cypherfinn/NetworkPortDetector/releases/latest/download/NetworkPortDetector.exe)**

Or visit the [Releases page](https://github.com/cypherfinn/NetworkPortDetector/releases) for all versions.

## 🎯 Features

✅ **Automatic LLDP Capture** - Shows switch name and port number
✅ **Adapter Selection** - Choose which network adapters to monitor
✅ **Real-time Display** - Automatically updates when LLDP packets arrive
✅ **All Network Adapters** - Shows all active adapters with IP, MAC, speed
✅ **Portable .exe** - No installation required
✅ **User-friendly GUI** - Color-coded for quick overview

## 📸 What the App Shows

The table displays:
- **Network Adapter** - Adapter name
- **Switch Name** - Switch hostname (via LLDP)
- **Switch Port** - Port number on switch (e.g., "GigabitEthernet1/0/24")
- **IP Address** - Your IP address
- **MAC Address** - Your MAC address
- **Status** - "LLDP Active" when switch information is received

## 📋 Prerequisites

### 1. Install Npcap
The app requires **Npcap** for packet capturing:

📥 **Download:** https://npcap.com/#download

**Installation:**
1. Download Npcap installer
2. During installation: ✅ Enable "Install Npcap in WinPcap API-compatible Mode"
3. Complete installation

### 2. Run as Administrator
The app **must** be started with administrator privileges:

- **Right-click** on `NetworkPortDetector.exe`
- Select **"Run as administrator"**

## 🚀 How to Use

### Step 1: Start the App
1. Run `NetworkPortDetector.exe` as administrator
2. The app displays all network adapters

### Step 2: Start LLDP Capture
1. Click **"LLDP Erfassung starten"** (Start LLDP Capture)
2. A dialog opens - **select the network adapter(s)** you want to monitor
   - All active adapters are pre-selected by default
   - You can select single or multiple adapters
   - Buttons: "Select All" / "Select None"
3. Click **"OK"**
4. The app starts receiving LLDP packets
5. Wait 30-60 seconds (LLDP packets are sent every ~30s)

### Step 3: View Results
- Adapters with **green background + bold text** = LLDP received
- Switch name and port are displayed in the table
- Click on an adapter to see detailed information

## 🎨 Color Coding

| Color | Meaning |
|-------|---------|
| 🟢 **Green + Bold** | LLDP active - switch info available |
| 🟡 **Yellow** | Adapter active, no LLDP received |
| ⚪ **Gray** | Adapter inactive/disconnected |

## 🔨 Build Instructions (for Developers)

### Prerequisites
- .NET 8.0 SDK or higher
- Windows 10/11

### Compile

**Option 1 - Using build.bat:**
```bash
cd src
build.bat
```

**Option 2 - Manual:**
```bash
cd src
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true
```

The compiled .exe will be in:
```
src\bin\Release\net8.0-windows\win-x64\publish\NetworkPortDetector.exe
```

## 🐛 Troubleshooting

### "No network devices found"
**Problem:** Npcap not installed or app not running as admin

**Solution:**
1. Install Npcap from https://npcap.com
2. Run app as administrator

### "No LLDP received"
**Problem:** Switch not sending LLDP

**Solution:**
1. Check if LLDP is enabled on the switch:
   - **Cisco:** `lldp run` (global config)
   - **HP/Aruba:** LLDP usually active by default
   - **Others:** See switch documentation

2. Wait 30-60 seconds (LLDP interval)

3. Alternative: Check on the switch:
   ```
   # Cisco
   show cdp neighbors detail
   show lldp neighbors detail

   # HP/Aruba
   show lldp info remote-device
   ```

### App shows "Access Denied"
**Problem:** No administrator privileges

**Solution:**
- Right-click on .exe → "Run as administrator"

## 🔧 Technical Details

- **Protocol:** LLDP (IEEE 802.1AB)
- **Libraries:** SharpPcap 6.2.5, PacketDotNet 1.4.7
- **Framework:** .NET 8.0
- **Packet Capture:** Npcap/WinPcap
- **LLDP Filter:** `ether proto 0x88cc`
- **File Size:** ~171 MB (includes .NET Runtime)

## ✅ Supported Switches

Works with **all switches** that send LLDP:

- Cisco (LLDP or CDP)
- HP / HPE / Aruba
- Juniper
- Dell
- Netgear
- Ubiquiti
- All other LLDP-capable switches

## 💡 Notes

- LLDP packets are typically sent every **30 seconds**
- The app can monitor **multiple adapters simultaneously**
- Data is processed **locally only** (no network connection needed)
- The .exe contains the complete .NET Runtime (hence ~171 MB)

## ⌨️ Keyboard Shortcuts

- **F5** - Refresh adapter list

## 📄 License

This project uses open-source libraries:
- **SharpPcap** - LGPL
- **PacketDotNet** - LGPL
- **Npcap** - See npcap.com for license

MIT License - See [LICENSE](LICENSE) file

## 🤝 Contributing

Contributions welcome! See [CONTRIBUTING.md](docs/CONTRIBUTING.md)

## 📞 Support

For issues or questions:
1. Check the [Troubleshooting](#-troubleshooting) section
2. Create an issue on GitHub

---

**Happy switch port hunting! 🔌**
