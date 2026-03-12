namespace OneClickScreenShot;

partial class MainForm
{
    private System.ComponentModel.IContainer components = null!;

    // Save-location controls
    private GroupBox grpSaveLocation = null!;
    private TextBox  txtSavePath     = null!;
    private Button   btnBrowse       = null!;

    // Hotkey controls
    private GroupBox grpHotkey    = null!;
    private CheckBox chkCtrl      = null!;
    private CheckBox chkAlt       = null!;
    private CheckBox chkShift     = null!;
    private Label    lblPlus      = null!;
    private ComboBox cmbKey       = null!;
    private Label    lblHotkeyHint = null!;

    // Action buttons
    private Button btnCaptureNow = null!;
    private Button btnStartTray  = null!;

    // Status bar
    private Label lblStatus = null!;

    // System-tray
    private NotifyIcon        notifyIcon       = null!;
    private ContextMenuStrip  contextMenuTray  = null!;
    private ToolStripMenuItem menuItemOpen     = null!;
    private ToolStripMenuItem menuItemCapture  = null!;
    private ToolStripSeparator menuSeparator   = null!;
    private ToolStripMenuItem menuItemExit     = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null)
            components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();

        // ── Instantiate ──────────────────────────────────────────────── //
        grpSaveLocation = new GroupBox();
        txtSavePath     = new TextBox();
        btnBrowse       = new Button();

        grpHotkey     = new GroupBox();
        chkCtrl       = new CheckBox();
        chkAlt        = new CheckBox();
        chkShift      = new CheckBox();
        lblPlus       = new Label();
        cmbKey        = new ComboBox();
        lblHotkeyHint = new Label();

        btnCaptureNow = new Button();
        btnStartTray  = new Button();

        lblStatus = new Label();

        notifyIcon      = new NotifyIcon(components);
        contextMenuTray = new ContextMenuStrip(components);
        menuItemOpen    = new ToolStripMenuItem();
        menuItemCapture = new ToolStripMenuItem();
        menuSeparator   = new ToolStripSeparator();
        menuItemExit    = new ToolStripMenuItem();

        // ── Form ─────────────────────────────────────────────────────── //
        SuspendLayout();

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode       = AutoScaleMode.Font;
        ClientSize          = new Size(496, 296);
        FormBorderStyle     = FormBorderStyle.FixedSingle;
        MaximizeBox         = false;
        StartPosition       = FormStartPosition.CenterScreen;
        Text                = "OneClickScreenShot";
        Icon                = SystemIcons.Application;

        // ── Save-location group ───────────────────────────────────────── //
        grpSaveLocation.Text     = "Save Location";
        grpSaveLocation.Location = new Point(12, 12);
        grpSaveLocation.Size     = new Size(472, 58);
        grpSaveLocation.TabIndex = 0;

        txtSavePath.Location = new Point(10, 24);
        txtSavePath.Size     = new Size(370, 23);
        txtSavePath.TabIndex = 0;
        txtSavePath.Anchor   = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;

        btnBrowse.Text     = "Browse…";
        btnBrowse.Location = new Point(388, 22);
        btnBrowse.Size     = new Size(74, 27);
        btnBrowse.TabIndex = 1;
        btnBrowse.Click   += btnBrowse_Click;

        grpSaveLocation.Controls.Add(txtSavePath);
        grpSaveLocation.Controls.Add(btnBrowse);

        // ── Hotkey group ──────────────────────────────────────────────── //
        grpHotkey.Text     = "Keyboard Shortcut";
        grpHotkey.Location = new Point(12, 82);
        grpHotkey.Size     = new Size(472, 112);
        grpHotkey.TabIndex = 1;

        chkCtrl.Text      = "Ctrl";
        chkCtrl.Location  = new Point(10, 30);
        chkCtrl.AutoSize  = true;
        chkCtrl.TabIndex  = 0;

        chkAlt.Text     = "Alt";
        chkAlt.Location = new Point(68, 30);
        chkAlt.AutoSize = true;
        chkAlt.TabIndex = 1;

        chkShift.Text     = "Shift";
        chkShift.Location = new Point(116, 30);
        chkShift.AutoSize = true;
        chkShift.TabIndex = 2;

        lblPlus.Text      = "+";
        lblPlus.Location  = new Point(170, 32);
        lblPlus.AutoSize  = true;

        cmbKey.Location      = new Point(188, 28);
        cmbKey.Size          = new Size(140, 23);
        cmbKey.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbKey.TabIndex      = 3;
        cmbKey.Items.AddRange(new object[]
        {
            "F1","F2","F3","F4","F5","F6",
            "F7","F8","F9","F10","F11","F12",
            "PrintScreen","Scroll","Pause",
            "Insert","Delete","Home","End",
            "PageUp","PageDown"
        });

        lblHotkeyHint.Text      = "Select one or more modifier keys and a key for the global shortcut.";
        lblHotkeyHint.Location  = new Point(10, 68);
        lblHotkeyHint.Size      = new Size(452, 32);
        lblHotkeyHint.ForeColor = SystemColors.GrayText;

        grpHotkey.Controls.Add(chkCtrl);
        grpHotkey.Controls.Add(chkAlt);
        grpHotkey.Controls.Add(chkShift);
        grpHotkey.Controls.Add(lblPlus);
        grpHotkey.Controls.Add(cmbKey);
        grpHotkey.Controls.Add(lblHotkeyHint);

        // ── Action buttons ────────────────────────────────────────────── //
        btnCaptureNow.Text     = "Capture Now";
        btnCaptureNow.Location = new Point(12, 208);
        btnCaptureNow.Size     = new Size(120, 36);
        btnCaptureNow.TabIndex = 2;
        btnCaptureNow.Click   += btnCaptureNow_Click;

        btnStartTray.Text     = "Start && Minimize to Tray";
        btnStartTray.Location = new Point(316, 208);
        btnStartTray.Size     = new Size(168, 36);
        btnStartTray.TabIndex = 3;
        btnStartTray.Click   += btnStartTray_Click;

        // ── Status label ──────────────────────────────────────────────── //
        lblStatus.Text      = "Ready.";
        lblStatus.Location  = new Point(12, 258);
        lblStatus.Size      = new Size(472, 20);
        lblStatus.ForeColor = SystemColors.GrayText;

        // ── Context menu ──────────────────────────────────────────────── //
        menuItemOpen.Text    = "Open";
        menuItemOpen.Click  += menuItemOpen_Click;

        menuItemCapture.Text   = "Take Screenshot Now";
        menuItemCapture.Click += menuItemCapture_Click;

        menuItemExit.Text   = "Exit";
        menuItemExit.Click += menuItemExit_Click;

        contextMenuTray.Items.Add(menuItemOpen);
        contextMenuTray.Items.Add(menuItemCapture);
        contextMenuTray.Items.Add(menuSeparator);
        contextMenuTray.Items.Add(menuItemExit);

        // ── Notify icon ───────────────────────────────────────────────── //
        notifyIcon.Text             = "OneClickScreenShot";
        notifyIcon.Icon             = SystemIcons.Application;
        notifyIcon.ContextMenuStrip = contextMenuTray;
        notifyIcon.DoubleClick     += notifyIcon_DoubleClick;
        // Visible is set to true only when the app minimises to tray.

        // ── Wire form ─────────────────────────────────────────────────── //
        Controls.Add(grpSaveLocation);
        Controls.Add(grpHotkey);
        Controls.Add(btnCaptureNow);
        Controls.Add(btnStartTray);
        Controls.Add(lblStatus);

        ResumeLayout(false);
    }
}
