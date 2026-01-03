# Installation and Setup

> 🇩🇪 [German Version](INSTALLATION.md) | 🇬🇧 **English Version**

## 📦 Portable .exe (Recommended)

The easiest method is using the pre-built executable:

### Step 1: Install Npcap

1. Download Npcap: https://npcap.com/#download
2. Run the installer
3. **Important:** Enable "Install Npcap in WinPcap API-compatible Mode"
4. Complete the installation

### Step 2: Download the App

1. Download `NetworkPortDetector.exe` from this repository
2. Save the file anywhere (e.g., Desktop)

### Step 3: Start the App

1. **Right-click** on `NetworkPortDetector.exe`
2. Select **"Run as administrator"**
3. The app starts and displays all network adapters

### Step 4: Capture LLDP

1. Click **"LLDP Erfassung starten"** (Start LLDP Capture)
2. Select the desired network adapters in the dialog
3. Click **"OK"**
4. Wait 30-60 seconds for LLDP packets

✅ **Done!** The app now shows switch names and port numbers.

---

## 🔨 Compile from Source

If you want to compile the app yourself:

### Prerequisites

- Windows 10/11 (64-bit)
- .NET 8.0 SDK: https://dotnet.microsoft.com/download/dotnet/8.0
- Git (optional)
- Npcap for testing

### Steps

1. **Clone repository**
   ```bash
   git clone <repository-url>
   cd NetworkPortDetector-GitHub
   ```

2. **Navigate to src directory**
   ```bash
   cd src
   ```

3. **Restore dependencies**
   ```bash
   dotnet restore
   ```

4. **Compile**

   **Option A - Using build.bat:**
   ```bash
   build.bat
   ```

   **Option B - Manual:**
   ```bash
   dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true
   ```

5. **Find the compiled .exe**

   The .exe is located at:
   ```
   bin\Release\net8.0-windows\win-x64\publish\NetworkPortDetector.exe
   ```

### Development Mode

For development and testing:

```bash
cd src
dotnet run
```

**Note:** Even in development mode, you must run as administrator!

---

## 🐛 Troubleshooting

### "No network devices found"

**Solution:**
- Install Npcap
- Run app as administrator
- Check Windows Firewall

### ".NET Runtime missing"

The portable .exe contains the runtime. If problems occur:
- Install .NET 8.0 Runtime: https://dotnet.microsoft.com/download/dotnet/8.0

### Build error "NU1100"

**Problem:** NuGet cannot download packages

**Solution:**
```bash
dotnet nuget add source https://api.nuget.org/v3/index.json --name nuget.org
dotnet restore
```

### "Access Denied" when starting

**Solution:**
- Right-click on .exe → "Run as administrator"
- Windows SmartScreen: "More info" → "Run anyway"

---

## 📋 System Requirements

### Minimum
- **OS:** Windows 10 (64-bit)
- **RAM:** 256 MB available
- **Disk:** 200 MB free
- **Software:** Npcap

### Recommended
- **OS:** Windows 11 (64-bit)
- **RAM:** 512 MB available
- **Disk:** 500 MB free
- **Software:** Npcap (latest version)

---

## ℹ️ Further Help

- [README](../README.md) - Main documentation
- [Release Notes](RELEASE.md) - Version information
- [Contributing](CONTRIBUTING.md) - How to contribute

For further questions, please create an issue on GitHub.
