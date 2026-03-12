# OneClickScreenShot

A lightweight Windows tray application that lets you capture the **active window** with a configurable global keyboard shortcut and automatically saves each screenshot as a high-quality PNG to a folder of your choice.

---

## Features

| Feature | Details |
|---|---|
| **Active-window capture** | Captures only the currently focused window (falls back to the full primary screen when no window is active). |
| **Lossless PNG output** | Every screenshot is saved as a 32-bit ARGB PNG — no compression artefacts. |
| **Configurable save folder** | Choose any folder via the Browse button; the app creates it if it does not exist. |
| **Global hotkey** | Register any combination of Ctrl / Alt / Shift + a function key or special key. The shortcut works system-wide, even when the app is minimised to the tray. |
| **System-tray operation** | Runs quietly in the background after you click *Start & Minimize to Tray*. Double-click the tray icon to reopen the settings window. |
| **Single instance** | Only one copy of the app can run at a time. |

---

## Requirements

- Windows 10 or 11
- [.NET 10 Runtime](https://dotnet.microsoft.com/download/dotnet/10.0) (Desktop / WinForms)

---

## Getting Started

1. **Download** the latest release from the [Releases](../../releases) page and run `OneClickScreenShot.exe`.
2. **Set a save location** — click *Browse…* to pick a folder (defaults to `Pictures\OneClickScreenShot`).
3. **Configure the hotkey** — check one or more modifier keys (Ctrl / Alt / Shift) and choose a key from the dropdown. The default is **Ctrl + PrintScreen**.
4. Click **Start & Minimize to Tray** — the app registers the hotkey and hides to the system tray.
5. Focus the window you want to capture and press your hotkey. A balloon notification confirms each saved file.

### Tray icon context menu

| Item | Action |
|---|---|
| Open | Reopen the settings window |
| Take Screenshot Now | Capture the active window immediately |
| Exit | Quit the application |

---

## Building from Source

```bash
git clone https://github.com/D3athByMonkey/OneClickScreenShot.git
cd OneClickScreenShot
dotnet build
dotnet run
```

> **Note:** The project targets `net10.0-windows`. `EnableWindowsTargeting` is set in the `.csproj`, so it also compiles on Linux/macOS CI runners, although the resulting binary only runs on Windows.

---

## Screenshots

_Screenshots of the app itself will appear here once initial releases are published._

---

## License

See [LICENSE](LICENSE) for details.
