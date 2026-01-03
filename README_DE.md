# Network Port Detector - LLDP Switch Port Finder

> 🇩🇪 **Deutsche Version** | 🇬🇧 [English Version](README.md)

Eine portable Windows-Anwendung zur **automatischen** Erkennung von Switch-Port-Verbindungen über LLDP (Link Layer Discovery Protocol).

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![.NET](https://img.shields.io/badge/.NET-8.0-blue)](https://dotnet.microsoft.com/download/dotnet/8.0)
[![Platform](https://img.shields.io/badge/platform-Windows-lightgrey)](https://www.microsoft.com/windows)
[![Release](https://img.shields.io/github/v/release/cypherfinn/NetworkPortDetector)](https://github.com/cypherfinn/NetworkPortDetector/releases/latest)

## 📥 Download

**[⬇️ Download NetworkPortDetector.exe (v1.0.0)](https://github.com/cypherfinn/NetworkPortDetector/releases/latest/download/NetworkPortDetector.exe)**

Oder besuchen Sie die [Releases-Seite](https://github.com/cypherfinn/NetworkPortDetector/releases) für alle Versionen.

## 🎯 Funktionen

✅ **Automatische LLDP-Erfassung** - Zeigt Switch-Name und Port-Nummer an
✅ **Adapter-Auswahl** - Wählen Sie gezielt einzelne oder mehrere Adapter aus
✅ **Echtzeit-Anzeige** - Aktualisiert sich automatisch bei neuen LLDP-Paketen
✅ **Alle Netzwerkadapter** - Zeigt alle aktiven Adapter mit IP, MAC, Geschwindigkeit
✅ **Portable .exe** - Keine Installation erforderlich
✅ **Übersichtliche GUI** - Farbcodierung für schnelle Übersicht

## 📸 Screenshot

Die App zeigt in der Tabelle:
- **Netzwerkadapter** - Name des Adapters
- **Switch Name** - Hostname des Switches (via LLDP)
- **Switch Port** - Port-Nummer am Switch (z.B. "GigabitEthernet1/0/24")
- **IP Adresse** - Ihre IP-Adresse
- **MAC Adresse** - Ihre MAC-Adresse
- **Status** - "LLDP Aktiv" wenn Switch-Informationen empfangen wurden

## 📋 Voraussetzungen

### 1. Npcap installieren
Die App benötigt **Npcap** zum Erfassen von Netzwerkpaketen:

📥 **Download:** https://npcap.com/#download

**Installation:**
1. Npcap-Installer herunterladen
2. Bei Installation: ✅ "Install Npcap in WinPcap API-compatible Mode" aktivieren
3. Installation abschließen

### 2. Als Administrator ausführen
Die App **muss** mit Administrator-Rechten gestartet werden:

- **Rechtsklick** auf `NetworkPortDetector.exe`
- **"Als Administrator ausführen"** wählen

## 🚀 Verwendung

### Schritt 1: App starten
1. `NetworkPortDetector.exe` als Administrator ausführen
2. Die App zeigt alle Netzwerkadapter an

### Schritt 2: LLDP-Erfassung starten
1. Klicken Sie auf **"LLDP Erfassung starten"**
2. Ein Dialog öffnet sich - **wählen Sie den/die gewünschten Netzwerkadapter aus**
   - Standardmäßig sind alle aktiven Adapter vorausgewählt
   - Sie können einen einzelnen Adapter wählen oder mehrere
   - Buttons: "Alle auswählen" / "Keine auswählen"
3. Klicken Sie auf **"OK"**
4. Die App beginnt, LLDP-Pakete zu empfangen
5. Warten Sie 30-60 Sekunden (LLDP-Pakete werden alle ~30s gesendet)

### Schritt 3: Ergebnisse ablesen
- Adapter mit **grünem Hintergrund + Fettdruck** = LLDP empfangen
- Switch-Name und Port werden in der Tabelle angezeigt
- Klicken Sie auf einen Adapter für Details

## 🎨 Farbcodierung

| Farbe | Bedeutung |
|-------|-----------|
| 🟢 **Grün + Fett** | LLDP aktiv - Switch-Info verfügbar |
| 🟡 **Gelb** | Adapter aktiv, kein LLDP empfangen |
| ⚪ **Grau** | Adapter inaktiv/getrennt |

## 🔨 Build-Anleitung (für Entwickler)

### Voraussetzungen
- .NET 8.0 SDK oder höher
- Windows 10/11

### Kompilieren

**Option 1 - Mit build.bat:**
```bash
build.bat
```

**Option 2 - Manuell:**
```bash
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true
```

Die fertige .exe befindet sich dann in:
```
bin\Release\net8.0-windows\win-x64\publish\NetworkPortDetector.exe
```

## 🐛 Fehlerbehebung

### "Keine Netzwerkgeräte gefunden"
**Problem:** Npcap ist nicht installiert oder App läuft nicht als Admin

**Lösung:**
1. Npcap von https://npcap.com installieren
2. App als Administrator starten

### "Kein LLDP empfangen"
**Problem:** Switch sendet kein LLDP

**Lösung:**
1. Prüfen Sie, ob LLDP auf dem Switch aktiviert ist:
   - **Cisco:** `lldp run` (global config)
   - **HP/Aruba:** LLDP ist meist standardmäßig aktiv
   - **Andere:** Siehe Switch-Dokumentation

2. Warten Sie 30-60 Sekunden (LLDP-Intervall)

3. Alternative: Prüfen Sie auf dem Switch:
   ```
   # Cisco
   show cdp neighbors detail
   show lldp neighbors detail

   # HP/Aruba
   show lldp info remote-device
   ```

### App zeigt "Zugriff verweigert"
**Problem:** Keine Administrator-Rechte

**Lösung:**
- Rechtsklick auf .exe → "Als Administrator ausführen"

## 🔧 Technische Details

- **Protokoll:** LLDP (IEEE 802.1AB)
- **Bibliotheken:** SharpPcap 6.2.5, PacketDotNet 1.4.7
- **Framework:** .NET 8.0
- **Paketerfassung:** Npcap/WinPcap
- **LLDP-Filter:** `ether proto 0x88cc`
- **Dateigröße:** ~171 MB (inkl. .NET Runtime)

## ✅ Unterstützte Switches

Die App funktioniert mit **allen Switches**, die LLDP senden:

- Cisco (LLDP oder CDP)
- HP / HPE / Aruba
- Juniper
- Dell
- Netgear
- Ubiquiti
- Alle anderen LLDP-fähigen Switches

## 💡 Hinweise

- LLDP-Pakete werden typischerweise alle **30 Sekunden** gesendet
- Die App kann **mehrere Adapter gleichzeitig** überwachen
- Daten werden **nur lokal** verarbeitet (keine Netzwerkverbindung nötig)
- Die .exe enthält die komplette .NET Runtime (daher ~171 MB)

## ⌨️ Tastenkürzel

- **F5** - Adapter-Liste aktualisieren

## 📄 Lizenz

Dieses Projekt verwendet Open-Source-Bibliotheken:
- **SharpPcap** - LGPL
- **PacketDotNet** - LGPL
- **Npcap** - Eigene Lizenz (siehe npcap.com)

## 🤝 Mitwirkende

Erstellt mit Claude Code (Anthropic)

## 📞 Support

Bei Problemen oder Fragen:
1. Prüfen Sie die [Fehlerbehebung](#-fehlerbehebung)
2. Erstellen Sie ein Issue auf GitHub

---

**Viel Erfolg beim Finden Ihrer Switch-Ports! 🔌**
