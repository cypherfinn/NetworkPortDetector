# Projektstruktur

## 📁 Verzeichnisbaum

```
NetworkPortDetector-GitHub/
│
├── src/                          # Quellcode
│   ├── MainForm.cs              # Haupt-GUI und LLDP-Logik
│   ├── Program.cs               # Einstiegspunkt
│   ├── NetworkPortDetector.csproj  # Projekt-Konfiguration
│   └── build.bat                # Build-Script für Windows
│
├── docs/                         # Dokumentation
│   ├── INSTALLATION.md          # Installations-Anleitung
│   ├── RELEASE.md               # Release Notes
│   ├── CONTRIBUTING.md          # Contribution Guidelines
│   └── STRUCTURE.md             # Diese Datei
│
├── NetworkPortDetector.exe      # Fertige portable Anwendung
├── README.md                    # Haupt-Dokumentation
├── LICENSE                      # MIT Lizenz
└── .gitignore                   # Git Ignore Konfiguration
```

## 📄 Datei-Beschreibungen

### Quellcode (`src/`)

#### `MainForm.cs` (29 KB)
Die Hauptklasse der Anwendung:
- **GUI-Initialisierung**: Layout, Buttons, ListView, TextBox
- **LLDP-Erfassung**: Paketerfassung mit SharpPcap
- **Adapter-Auswahl**: Dialog zur Auswahl der Netzwerkadapter
- **LLDP-Parsing**: Extraktion von Switch-Name und Port
- **Echtzeit-Updates**: Automatische GUI-Aktualisierung

**Wichtige Klassen:**
- `MainForm`: Haupt-Fenster
- `SwitchPortInfo`: Datenmodell für Switch-Informationen
- `DeviceItem`: Wrapper für Netzwerkadapter

#### `Program.cs` (354 Bytes)
Der Einstiegspunkt:
- Initialisiert Windows Forms
- Startet die MainForm
- STAThread-Konfiguration

#### `NetworkPortDetector.csproj` (743 Bytes)
Projekt-Konfiguration:
- Target Framework: .NET 8.0 Windows
- NuGet-Pakete: SharpPcap 6.2.5, PacketDotNet 1.4.7
- Build-Konfiguration: Portable Single-File

#### `build.bat` (446 Bytes)
Build-Script:
- Stellt NuGet-Pakete wieder her
- Kompiliert portable .exe
- Zeigt Ausgabepfad

### Dokumentation (`docs/`)

#### `INSTALLATION.md`
Schritt-für-Schritt Installations- und Kompilieranleitung

#### `RELEASE.md`
Versionshinweise, Features, Bekannte Probleme

#### `CONTRIBUTING.md`
Guidelines für Contributions, Code-Style, PR-Prozess

#### `STRUCTURE.md`
Diese Datei - Projektstruktur-Übersicht

### Root-Dateien

#### `NetworkPortDetector.exe` (171 MB)
Fertige portable Anwendung:
- Enthält .NET 8.0 Runtime
- Self-contained (keine Installation nötig)
- x64 Windows Binary

#### `README.md`
Haupt-Dokumentation mit:
- Features
- Verwendung
- Fehlerbehebung
- Quick Start

#### `LICENSE`
MIT-Lizenz mit Hinweisen auf verwendete Libraries

#### `.gitignore`
Git-Konfiguration zum Ausschluss von:
- Build-Artefakten
- Visual Studio Dateien
- Temporären Dateien

## 🔧 Technische Details

### Dependencies

**NuGet-Pakete:**
- `SharpPcap 6.2.5` - Paketerfassung
- `PacketDotNet 1.4.7` - Paket-Parsing

**Framework:**
- .NET 8.0 Windows

**Externe Software:**
- Npcap (für Paketerfassung)

### Build-Output

Nach erfolgreicher Kompilierung:
```
src/
└── bin/
    └── Release/
        └── net8.0-windows/
            └── win-x64/
                └── publish/
                    └── NetworkPortDetector.exe  (171 MB)
```

### Code-Metriken

- **Zeilen Code:** ~750 Zeilen C#
- **Klassen:** 3 Hauptklassen
- **Methoden:** ~30 Methoden
- **Complexity:** Niedrig-Mittel

## 🚀 Entwicklungs-Workflow

1. **Änderungen machen** in `src/`
2. **Testen** mit `dotnet run`
3. **Kompilieren** mit `build.bat`
4. **Testen** der .exe
5. **Commit & Push**

## 📦 Release-Prozess

1. Version in `.csproj` aktualisieren
2. `RELEASE.md` aktualisieren
3. Mit `build.bat` kompilieren
4. .exe testen
5. GitHub Release erstellen
6. .exe hochladen

## 🔍 Code-Highlights

### LLDP-Parsing
```csharp
// TLV Types: 1=Chassis ID, 2=Port ID, 3=TTL, 4=Port Desc, 5=System Name, 6=System Desc
if (tlvType == 5) // System Name
{
    info.SwitchName = Encoding.UTF8.GetString(tlv.Bytes, 2, tlv.Bytes.Length - 2);
}
```

### Adapter-Auswahl
```csharp
var selectedDevices = ShowAdapterSelectionDialog(devices);
if (selectedDevices == null || selectedDevices.Count == 0)
{
    return; // User cancelled
}
```

### Paketerfassung
```csharp
device.Filter = "ether proto 0x88cc";
device.OnPacketArrival += Device_OnPacketArrival;
device.StartCapture();
```

---

Für weitere Informationen siehe die anderen Dokumentations-Dateien.
