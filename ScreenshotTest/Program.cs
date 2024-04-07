using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

internal class Program
{
    [DllImport("user32.dll")]
    public static extern bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    public static extern IntPtr GetWindowDC(IntPtr hwnd);

    [DllImport("gdi32.dll")]
    public static extern bool DeleteDC(IntPtr hdc);

    [DllImport("user32.dll")]
    public static extern bool ReleaseDC(IntPtr hWnd, IntPtr hDc);

    [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
    public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);

    [DllImport("user32.dll")]
    public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);


    static void Main()
    {
        var hwnd = FindWindow(null, "雷神加速器");                                                                                                                     // bitmap.Save("screenshot.png", ImageFormat.Png);
                                                                                                                            // bitmap.Dispose();
        if (true)
        {// 将找到的第一个具有 UI 的进程的窗口置于前台并最大化
            IntPtr mainWindowHandle = hwnd;
            SetForegroundWindow(mainWindowHandle);
            //ShowWindow(mainWindowHandle, 3); // 3 代表 SW_MAXIMIZE
            IntPtr windowDC = GetWindowDC(hwnd);
            //// 等待一段时间确保窗口已经最大化
            //System.Threading.Thread.Sleep(1000);
            RECT windowRect = new RECT();
            GetWindowRect(hwnd, ref windowRect);
            int width = windowRect.Right - windowRect.Left;
            int height = windowRect.Bottom - windowRect.Top;
            
            // 截图
            using (Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb))
            {
                Console.WriteLine($"width:{width}, height:{height}, size:{bitmap.Size}");
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    graphics.CopyFromScreen(windowRect.Left, windowRect.Top, 0, 0, bitmap.Size);
                }
                bitmap.Save("screenshot.png", ImageFormat.Png);
            }
            ReleaseDC(hwnd, windowDC);
            // 还原窗口
            ShowWindow(mainWindowHandle, 9); // 9 代表 SW_RESTORE
        }

    }

 

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int Left;                             //最左坐标
        public int Top;                             //最上坐标
        public int Right;                           //最右坐标
        public int Bottom;                        //最下坐标
    }


 




}