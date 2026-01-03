# Installation und Einrichtung

## 📦 Portable .exe (Empfohlen)

Die einfachste Methode ist die Verwendung der fertigen .exe:

### Schritt 1: Npcap installieren

1. Laden Sie Npcap herunter: https://npcap.com/#download
2. Führen Sie den Installer aus
3. **Wichtig:** Aktivieren Sie "Install Npcap in WinPcap API-compatible Mode"
4. Schließen Sie die Installation ab

### Schritt 2: App herunterladen

1. Laden Sie `NetworkPortDetector.exe` aus diesem Repository herunter
2. Speichern Sie die Datei an einem beliebigen Ort (z.B. Desktop)

### Schritt 3: App starten

1. **Rechtsklick** auf `NetworkPortDetector.exe`
2. Wählen Sie **"Als Administrator ausführen"**
3. Die App startet und zeigt alle Netzwerkadapter an

### Schritt 4: LLDP erfassen

1. Klicken Sie auf **"LLDP Erfassung starten"**
2. Wählen Sie die gewünschten Netzwerkadapter im Dialog
3. Klicken Sie auf **"OK"**
4. Warten Sie 30-60 Sekunden auf LLDP-Pakete

✅ **Fertig!** Die App zeigt nun Switch-Namen und Port-Nummern an.

---

## 🔨 Aus Quellcode kompilieren

Wenn Sie die App selbst kompilieren möchten:

### Voraussetzungen

- Windows 10/11 (64-bit)
- .NET 8.0 SDK: https://dotnet.microsoft.com/download/dotnet/8.0
- Git (optional)
- Npcap für Tests

### Schritte

1. **Repository klonen**
   ```bash
   git clone <repository-url>
   cd NetworkPortDetector-GitHub
   ```

2. **In src-Verzeichnis wechseln**
   ```bash
   cd src
   ```

3. **Dependencies wiederherstellen**
   ```bash
   dotnet restore
   ```

4. **Kompilieren**

   **Option A - Mit build.bat:**
   ```bash
   build.bat
   ```

   **Option B - Manuell:**
   ```bash
   dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true
   ```

5. **Fertige .exe finden**

   Die .exe befindet sich in:
   ```
   bin\Release\net8.0-windows\win-x64\publish\NetworkPortDetector.exe
   ```

### Entwicklungsmodus

Für Entwicklung und Tests:

```bash
cd src
dotnet run
```

**Hinweis:** Auch im Entwicklungsmodus müssen Sie die App als Administrator starten!

---

## 🐛 Troubleshooting

### "Keine Netzwerkgeräte gefunden"

**Lösung:**
- Npcap installieren
- App als Administrator ausführen
- Windows Firewall prüfen

### ".NET Runtime fehlt"

Die portable .exe enthält die Runtime. Falls Probleme auftreten:
- .NET 8.0 Runtime installieren: https://dotnet.microsoft.com/download/dotnet/8.0

### Build-Fehler "NU1100"

**Problem:** NuGet kann Pakete nicht herunterladen

**Lösung:**
```bash
dotnet nuget add source https://api.nuget.org/v3/index.json --name nuget.org
dotnet restore
```

### "Zugriff verweigert" beim Starten

**Lösung:**
- Rechtsklick auf .exe → "Als Administrator ausführen"
- Windows SmartScreen: "Weitere Informationen" → "Trotzdem ausführen"

---

## 📋 Systemanforderungen

### Minimum
- **OS:** Windows 10 (64-bit)
- **RAM:** 256 MB verfügbar
- **Disk:** 200 MB frei
- **Software:** Npcap

### Empfohlen
- **OS:** Windows 11 (64-bit)
- **RAM:** 512 MB verfügbar
- **Disk:** 500 MB frei
- **Software:** Npcap (neueste Version)

---

## ℹ️ Weitere Hilfe

- [README](../README.md) - Hauptdokumentation
- [Release Notes](RELEASE.md) - Versionshinweise
- [Contributing](CONTRIBUTING.md) - Mitwirken

Bei weiteren Fragen erstellen Sie bitte ein Issue auf GitHub.
