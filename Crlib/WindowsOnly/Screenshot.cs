using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace REVUnit.Crlib.WindowsOnly
{
    public static class Screenshot
    {
        public static Rectangle GetWindowRealSize(IntPtr handle)
        {
            Native.DwmGetWindowAttribute(handle, 9, out Native.Rect r, Marshal.SizeOf<Native.Rect>());
            return new Rectangle(r.X, r.Y, r.Width, r.Height);
        }

        public static Bitmap Take(IntPtr windowHandle)
        {
            return Take(GetWindowRealSize(windowHandle));
        }

        public static Bitmap TakeScreen()
        {
            return Take(Screen.PrimaryScreen.Bounds);
        }

        public static Bitmap Take(Rectangle rect)
        {
            Point location = rect.Location;
            location.Offset(1, 1);
            var blockRegionSize = new Size(rect.Size.Width - 3, rect.Size.Height - 3);
            var bitmap = new Bitmap(blockRegionSize.Width, blockRegionSize.Height);
            using Graphics graphics = Graphics.FromImage(bitmap);
            graphics.CopyFromScreen(location, default, blockRegionSize);
            Bitmap result = bitmap;
            return result;
        }

        public static Bitmap TakeForeground()
        {
            return Take(Native.GetForegroundWindow());
        }
    }
}