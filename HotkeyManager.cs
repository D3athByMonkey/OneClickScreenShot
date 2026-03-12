using System.Runtime.InteropServices;

namespace OneClickScreenShot;

/// <summary>
/// Registers and manages a single system-wide (global) hotkey.
/// </summary>
internal sealed class HotkeyManager : IDisposable
{
    // Posted to the window that registered the hotkey.
    private const int WM_HOTKEY = 0x0312;

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

    // Modifier flag constants (winuser.h)
    public const uint MOD_ALT     = 0x0001;
    public const uint MOD_CONTROL = 0x0002;
    public const uint MOD_SHIFT   = 0x0004;
    public const uint MOD_WIN     = 0x0008;
    /// <summary>Prevents key-repeat messages while the key is held down.</summary>
    public const uint MOD_NOREPEAT = 0x4000;

    private readonly IntPtr _hwnd;
    // Use a fixed, stable ID in the user-defined range (0x0000–0xBFFF).
    private const int HotkeyId = 0x0001;
    private bool _isRegistered;
    private bool _disposed;

    /// <summary>Raised on the UI thread when the registered hotkey is pressed.</summary>
    public event EventHandler? HotkeyPressed;

    public HotkeyManager(IntPtr hwnd)
    {
        _hwnd = hwnd;
    }

    /// <summary>
    /// Registers the hotkey. Unregisters any previously registered hotkey first.
    /// </summary>
    /// <param name="modifiers">Combination of MOD_* constants.</param>
    /// <param name="key">Virtual key code.</param>
    /// <returns>True if registration succeeded.</returns>
    public bool Register(uint modifiers, Keys key)
    {
        Unregister();

        // Use a stable, fixed ID in the user-defined range (0x0000–0xBFFF).
        _isRegistered = RegisterHotKey(_hwnd, HotkeyId, modifiers | MOD_NOREPEAT, (uint)key);
        return _isRegistered;
    }

    /// <summary>Unregisters the current hotkey, if any.</summary>
    public void Unregister()
    {
        if (_isRegistered)
        {
            UnregisterHotKey(_hwnd, HotkeyId);
            _isRegistered = false;
        }
    }

    /// <summary>
    /// Call this from <see cref="Form.WndProc"/> to dispatch WM_HOTKEY messages.
    /// </summary>
    public bool ProcessMessage(ref Message m)
    {
        if (m.Msg == WM_HOTKEY && m.WParam.ToInt32() == HotkeyId)
        {
            HotkeyPressed?.Invoke(this, EventArgs.Empty);
            return true;
        }
        return false;
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            Unregister();
            _disposed = true;
        }
    }
}
