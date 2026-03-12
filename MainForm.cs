namespace OneClickScreenShot;

/// <summary>
/// Main settings window for OneClickScreenShot.
/// Allows the user to configure the save location and keyboard shortcut,
/// then minimise the app to the system tray.
/// </summary>
public partial class MainForm : Form
{
    private HotkeyManager? _hotkeyManager;
    private bool _runningInTray;

    // ------------------------------------------------------------------ //
    //  Construction
    // ------------------------------------------------------------------ //

    public MainForm()
    {
        InitializeComponent();
        txtSavePath.Text = DefaultSavePath();
        // Default hotkey: Ctrl + PrintScreen
        chkCtrl.Checked  = true;
        chkAlt.Checked   = false;
        chkShift.Checked = false;
        cmbKey.SelectedIndex = cmbKey.Items.IndexOf("PrintScreen");
    }

    private static string DefaultSavePath() =>
        Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
            "OneClickScreenShot");

    // ------------------------------------------------------------------ //
    //  UI event handlers
    // ------------------------------------------------------------------ //

    private void btnBrowse_Click(object sender, EventArgs e)
    {
        using var dlg = new FolderBrowserDialog
        {
            Description  = "Select folder to save screenshots",
            SelectedPath = txtSavePath.Text,
            UseDescriptionForTitle = true
        };
        if (dlg.ShowDialog(this) == DialogResult.OK)
            txtSavePath.Text = dlg.SelectedPath;
    }

    private async void btnCaptureNow_Click(object sender, EventArgs e)
    {
        if (!ValidateSavePath()) return;

        // Minimise so our window is not the active window being captured.
        btnCaptureNow.Enabled = false;
        WindowState = FormWindowState.Minimized;
        await Task.Delay(300);

        try
        {
            string path = ScreenshotCapture.CaptureActiveWindow(txtSavePath.Text);
            WindowState = FormWindowState.Normal;
            SetStatus($"Saved: {path}");
            notifyIcon.ShowBalloonTip(2500, "Screenshot Saved",
                Path.GetFileName(path), ToolTipIcon.Info);
        }
        catch (Exception ex)
        {
            WindowState = FormWindowState.Normal;
            MessageBox.Show(this, $"Screenshot failed:\n{ex.Message}",
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            btnCaptureNow.Enabled = true;
        }
    }

    private void btnStartTray_Click(object sender, EventArgs e)
    {
        if (!ValidateSavePath()) return;

        uint mods = BuildModifiers();
        if (mods == 0)
        {
            MessageBox.Show(this,
                "Please select at least one modifier key (Ctrl, Alt or Shift).",
                "Modifier Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (!Enum.TryParse<Keys>(cmbKey.SelectedItem?.ToString(), out Keys hotKey))
            hotKey = Keys.PrintScreen;

        _hotkeyManager ??= new HotkeyManager(Handle);
        _hotkeyManager.HotkeyPressed -= OnHotkeyPressed;
        _hotkeyManager.HotkeyPressed += OnHotkeyPressed;

        if (_hotkeyManager.Register(mods, hotKey))
        {
            _runningInTray = true;
            notifyIcon.Visible = true;
            SetStatus($"Running in tray — hotkey active ({BuildHotkeyLabel(mods, hotKey)}).");
            HideToTray();
        }
        else
        {
            MessageBox.Show(this,
                "Could not register the hotkey — it may already be in use.\n" +
                "Try a different key combination.",
                "Hotkey Registration Failed",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // ------------------------------------------------------------------ //
    //  Hotkey callback
    // ------------------------------------------------------------------ //

    private void OnHotkeyPressed(object? sender, EventArgs e)
    {
        try
        {
            string path = ScreenshotCapture.CaptureActiveWindow(txtSavePath.Text);
            SetStatus($"Saved: {path}");
            notifyIcon.ShowBalloonTip(2500, "Screenshot Saved",
                Path.GetFileName(path), ToolTipIcon.Info);
        }
        catch (Exception ex)
        {
            notifyIcon.ShowBalloonTip(3000, "Screenshot Failed",
                ex.Message, ToolTipIcon.Error);
        }
    }

    // ------------------------------------------------------------------ //
    //  Tray icon
    // ------------------------------------------------------------------ //

    private void notifyIcon_DoubleClick(object sender, EventArgs e) => ShowFromTray();

    private void menuItemOpen_Click(object sender, EventArgs e) => ShowFromTray();

    private void menuItemCapture_Click(object sender, EventArgs e) => OnHotkeyPressed(this, EventArgs.Empty);

    private void menuItemExit_Click(object sender, EventArgs e)
    {
        _runningInTray = false;
        Application.Exit();
    }

    private void HideToTray()
    {
        ShowInTaskbar = false;
        WindowState   = FormWindowState.Minimized;
    }

    private void ShowFromTray()
    {
        ShowInTaskbar = true;
        WindowState   = FormWindowState.Normal;
        Show();
        Activate();
    }

    // ------------------------------------------------------------------ //
    //  Overrides
    // ------------------------------------------------------------------ //

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        if (_runningInTray && e.CloseReason == CloseReason.UserClosing)
        {
            // Keep running in tray; just hide the window.
            e.Cancel = true;
            HideToTray();
        }
        else
        {
            _hotkeyManager?.Dispose();
            notifyIcon.Visible = false;
        }
        base.OnFormClosing(e);
    }

    protected override void WndProc(ref Message m)
    {
        _hotkeyManager?.ProcessMessage(ref m);
        base.WndProc(ref m);
    }

    // ------------------------------------------------------------------ //
    //  Helpers
    // ------------------------------------------------------------------ //

    private bool ValidateSavePath()
    {
        if (string.IsNullOrWhiteSpace(txtSavePath.Text))
        {
            MessageBox.Show(this, "Please set a save location first.",
                "Save Location Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }
        return true;
    }

    private uint BuildModifiers()
    {
        uint mods = 0;
        if (chkCtrl.Checked)  mods |= HotkeyManager.MOD_CONTROL;
        if (chkAlt.Checked)   mods |= HotkeyManager.MOD_ALT;
        if (chkShift.Checked) mods |= HotkeyManager.MOD_SHIFT;
        return mods;
    }

    private string BuildHotkeyLabel(uint mods, Keys key)
    {
        var parts = new System.Collections.Generic.List<string>();
        if ((mods & HotkeyManager.MOD_CONTROL) != 0) parts.Add("Ctrl");
        if ((mods & HotkeyManager.MOD_ALT)     != 0) parts.Add("Alt");
        if ((mods & HotkeyManager.MOD_SHIFT)   != 0) parts.Add("Shift");
        parts.Add(key.ToString());
        return string.Join("+", parts);
    }

    private void SetStatus(string text)
    {
        if (InvokeRequired)
            Invoke(SetStatus, text);
        else
            lblStatus.Text = text;
    }
}
