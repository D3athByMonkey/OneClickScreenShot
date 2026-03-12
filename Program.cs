namespace OneClickScreenShot;

static class Program
{
    private const string MutexName = "OneClickScreenShot_SingleInstance";

    [STAThread]
    static void Main()
    {
        using var mutex = new System.Threading.Mutex(true, MutexName, out bool createdNew);
        if (!createdNew)
        {
            MessageBox.Show(
                "OneClickScreenShot is already running.\nCheck the system tray.",
                "Already Running",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            return;
        }

        ApplicationConfiguration.Initialize();
        Application.Run(new MainForm());
    }
}
