using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace OneClickScreenShot;

/// <summary>
/// Captures the currently active (foreground) window and saves it as a PNG file.
/// </summary>
internal static class ScreenshotCapture
{
    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

    [DllImport("user32.dll")]
    private static extern bool IsIconic(IntPtr hWnd);

    [StructLayout(LayoutKind.Sequential)]
    private struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    /// <summary>
    /// Captures the foreground window. Falls back to the primary screen if there is
    /// no foreground window or the window is minimized.
    /// </summary>
    /// <param name="savePath">Directory in which to save the PNG file.</param>
    /// <returns>Full path of the saved file.</returns>
    public static string CaptureActiveWindow(string savePath)
    {
        IntPtr hwnd = GetForegroundWindow();

        if (hwnd != IntPtr.Zero && !IsIconic(hwnd) && GetWindowRect(hwnd, out RECT rect))
        {
            int width  = rect.Right  - rect.Left;
            int height = rect.Bottom - rect.Top;

            if (width > 0 && height > 0)
            {
                using var bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
                using var g   = Graphics.FromImage(bmp);
                g.CopyFromScreen(rect.Left, rect.Top, 0, 0,
                                 new Size(width, height),
                                 CopyPixelOperation.SourceCopy);
                return SaveBitmap(bmp, savePath);
            }
        }

        // Fallback: capture primary screen
        return CaptureScreen(savePath);
    }

    /// <summary>Captures the entire primary screen.</summary>
    public static string CaptureScreen(string savePath)
    {
        Rectangle bounds = Screen.PrimaryScreen?.Bounds ?? SystemInformation.VirtualScreen;
        using var bmp = new Bitmap(bounds.Width, bounds.Height, PixelFormat.Format32bppArgb);
        using var g   = Graphics.FromImage(bmp);
        g.CopyFromScreen(bounds.Left, bounds.Top, 0, 0,
                         bounds.Size, CopyPixelOperation.SourceCopy);
        return SaveBitmap(bmp, savePath);
    }

    // Saves the bitmap as a lossless PNG. Creates the directory if it does not exist.
    private static string SaveBitmap(Bitmap bitmap, string savePath)
    {
        Directory.CreateDirectory(savePath);
        string fileName = $"Screenshot_{DateTime.Now:yyyy-MM-dd_HH-mm-ss-fff}.png";
        string fullPath = Path.Combine(savePath, fileName);
        bitmap.Save(fullPath, ImageFormat.Png);
        return fullPath;
    }
}
