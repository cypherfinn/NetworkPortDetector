# GitHub Repository Checkliste

Verwenden Sie diese Checkliste beim Einrichten des GitHub Repositories.

## 📋 Vor dem Upload

- [x] Alle Dateien auf persönliche Informationen geprüft
- [x] .gitignore erstellt
- [x] LICENSE Datei vorhanden
- [x] README.md vollständig
- [x] .exe kompiliert und getestet
- [x] Dokumentation vollständig

## 🔧 Repository Einstellungen

Nach dem Erstellen des Repositories auf GitHub:

### Basis-Einstellungen

- [ ] Repository-Name: `network-port-detector` oder ähnlich
- [ ] Beschreibung: "Portable Windows app to detect switch ports via LLDP"
- [ ] Topics hinzufügen: `lldp`, `network`, `switch`, `port-detection`, `windows`, `csharp`, `dotnet`
- [ ] Website: (Optional) Link zur Dokumentation

### Features aktivieren

- [ ] Issues aktivieren
- [ ] Projects deaktivieren (optional)
- [ ] Wiki deaktivieren (Dokumentation ist in `docs/`)
- [ ] Discussions aktivieren (optional)

### Branches

- [ ] `main` als Standard-Branch
- [ ] Branch protection für `main`:
  - [ ] Require pull request reviews
  - [ ] Require status checks to pass

### GitHub Actions

- [ ] Actions aktivieren
- [ ] Workflow `build.yml` wird automatisch erkannt
- [ ] Ersten Build überprüfen

## 📦 Release erstellen

### Schritt 1: Tag erstellen

```bash
git tag -a v1.0.0 -m "Initial release"
git push origin v1.0.0
```

### Schritt 2: GitHub Release

1. Gehen Sie zu "Releases" → "Create a new release"
2. Tag: `v1.0.0`
3. Release title: `Network Port Detector v1.0.0`
4. Beschreibung aus `docs/RELEASE.md` kopieren
5. Assets hochladen:
   - [ ] `NetworkPortDetector.exe`
   - [ ] (Optional) Source code wird automatisch hinzugefügt

### Schritt 3: Release veröffentlichen

- [ ] "This is a pre-release" NICHT aktivieren
- [ ] "Create a discussion for this release" aktivieren (optional)
- [ ] "Publish release" klicken

## 📝 README Badges hinzufügen

Fügen Sie am Anfang der README.md hinzu:

```markdown
[![Build](https://github.com/YOUR-USERNAME/network-port-detector/actions/workflows/build.yml/badge.svg)](https://github.com/YOUR-USERNAME/network-port-detector/actions/workflows/build.yml)
[![Downloads](https://img.shields.io/github/downloads/YOUR-USERNAME/network-port-detector/total)](https://github.com/YOUR-USERNAME/network-port-detector/releases)
[![Release](https://img.shields.io/github/v/release/YOUR-USERNAME/network-port-detector)](https://github.com/YOUR-USERNAME/network-port-detector/releases/latest)
```

Ersetzen Sie `YOUR-USERNAME` mit Ihrem GitHub-Benutzernamen.

## 🔒 Sicherheit

- [ ] Security Policy erstellen (optional)
- [ ] Dependabot aktivieren
- [ ] Code scanning aktivieren (optional)

## 📢 Community

### Issue Templates erstellen

Erstellen Sie `.github/ISSUE_TEMPLATE/`:

1. `bug_report.md` - Bug Report Template
2. `feature_request.md` - Feature Request Template

### Pull Request Template

Erstellen Sie `.github/PULL_REQUEST_TEMPLATE.md`

## 🌟 Promotion

Nach dem Release:

- [ ] Reddit Post in r/sysadmin oder r/networking
- [ ] LinkedIn Post (optional)
- [ ] Twitter/X Post (optional)

## ✅ Finale Überprüfung

Vor der Veröffentlichung:

- [ ] README.md öffnet sich korrekt
- [ ] Alle Links funktionieren
- [ ] .exe ist herunterladbar
- [ ] Build-Workflow läuft erfolgreich
- [ ] Lizenz ist korrekt
- [ ] Keine persönlichen Informationen im Code

## 📊 Nach dem Release

- [ ] GitHub Stars beobachten
- [ ] Issues beantworten
- [ ] Pull Requests reviewen
- [ ] Nächste Version planen

---

**Viel Erfolg mit Ihrem GitHub Repository! 🚀**
