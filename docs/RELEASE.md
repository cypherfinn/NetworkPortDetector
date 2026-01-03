# Release Notes

## Version 1.0.0

### ✨ Features

- **LLDP-Erfassung**: Automatische Erkennung von Switch-Namen und Port-Nummern
- **Adapter-Auswahl**: Wählen Sie gezielt, welche Netzwerkadapter überwacht werden sollen
- **Echtzeit-Updates**: Live-Anzeige von LLDP-Paketen
- **Portable**: Keine Installation erforderlich - einfach ausführen
- **Benutzerfreundlich**: Intuitive GUI mit Farbcodierung

### 📦 Download

Die portable .exe-Datei ist in diesem Repository enthalten:

**Datei:** `NetworkPortDetector.exe`
**Größe:** ~171 MB (enthält .NET Runtime)
**Platform:** Windows 10/11 (64-bit)

### 🚀 Schnellstart

1. **Npcap installieren** von https://npcap.com
2. **NetworkPortDetector.exe** als Administrator ausführen
3. **"LLDP Erfassung starten"** klicken
4. **Adapter auswählen** im Dialog
5. **Warten** auf LLDP-Pakete (30-60 Sekunden)

### ⚠️ Wichtige Hinweise

- **Administrator-Rechte erforderlich** für Paketerfassung
- **Npcap muss installiert sein** (kostenlos)
- **LLDP muss am Switch aktiviert sein**
- Erste LLDP-Pakete können bis zu 60 Sekunden dauern

### 🔧 Technische Details

- **.NET Version:** 8.0
- **Runtime:** Self-contained (im .exe enthalten)
- **Architektur:** x64
- **Dependencies:** SharpPcap 6.2.5, PacketDotNet 1.4.7

### 📋 Systemanforderungen

- **OS:** Windows 10/11 (64-bit)
- **RAM:** Mindestens 256 MB verfügbar
- **Disk:** 200 MB freier Speicherplatz
- **Software:** Npcap (https://npcap.com)
- **Rechte:** Administrator-Rechte

### 🐛 Bekannte Probleme

- Bei manchen Netzwerkadaptern kann die Erfassung fehlschlagen → Versuchen Sie einen anderen Adapter
- CDP (Cisco Discovery Protocol) wird derzeit nicht vollständig geparst

### 📝 Changelog

**Version 1.0.0 (2026-01-03)**
- Erste öffentliche Version
- LLDP-Erfassung implementiert
- Adapter-Auswahl-Dialog hinzugefügt
- Portable .exe Build

### 🔜 Geplante Features

- CDP (Cisco Discovery Protocol) vollständige Unterstützung
- Export der Ergebnisse (CSV/JSON)
- Automatische Switch-Port-Dokumentation
- Dark Mode
- Multi-Language Support

### 🤝 Feedback

Probleme oder Feature-Requests? Erstellen Sie ein Issue auf GitHub!

---

**Download:** Laden Sie `NetworkPortDetector.exe` aus diesem Repository herunter
