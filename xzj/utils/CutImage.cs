using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace xzj.utils
{
    class CutImage
    {
       // 与 Win32接口通信
        [DllImport("USER32.dll")]
        private static extern Int32 SendMessage(IntPtr hWnd, int msg, int wParam, IntPtr lParam);
        private const int WM_USER = 0x400;
        private const int EM_FORMATRANGE = WM_USER + 57;
        private const double inch = 14.4; // 与象素的换算。
        // 再是所需要的结构体的定义：
        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
        // 望文生义吧
        [StructLayout(LayoutKind.Sequential)]
        private struct CHARRANGE
        {
            public int cpMin;
            public int cpMax;
        }
        [StructLayout(LayoutKind.Sequential)]
        private struct FORMATRANGE
        {
            public IntPtr hdc;
            public IntPtr hdcTarget;
            public RECT rc;
            public RECT rcPage;
            public CHARRANGE chrg;
        }
        // 将RTB上的内容格式化后画出来
        public static Bitmap RtbToBitmap(RichTextBox rtb)
        {
            // 这个地方是可打印的大小
            return RtbToBitmap(rtb, rtb.Bounds.Width, rtb.Bounds.Height);
        }
        public static Bitmap RtbToBitmap(RichTextBox rtb, int width, int height)
        {
            Bitmap bmp = new Bitmap(width, height);
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    bmp.SetPixel(i, j, Color.White);
                }
            }
            using (Graphics gr = Graphics.FromImage(bmp))
            {
                System.IntPtr hDC = gr.GetHdc(); // 屏幕做为画源
                FORMATRANGE fmtRange;
                RECT rect;
                int fromAPI;
                rect.Top = 0; rect.Left = 0;
                rect.Bottom = (int)(bmp.Height + (bmp.Height * (bmp.HorizontalResolution / 100)) * inch);
                rect.Right = (int)(bmp.Width + (bmp.Width * (bmp.VerticalResolution / 100)) * inch);
                fmtRange.chrg.cpMin = 0;
                fmtRange.chrg.cpMax = -1;
                fmtRange.hdc = hDC;
                fmtRange.hdcTarget = hDC;
                fmtRange.rc = rect;
                fmtRange.rcPage = rect;
                int wParam = 1;
                System.IntPtr lParam = System.Runtime.InteropServices.Marshal.AllocCoTaskMem(Marshal.SizeOf(fmtRange));
                System.Runtime.InteropServices.Marshal.StructureToPtr(fmtRange, lParam, false);
                fromAPI = SendMessage(rtb.Handle, EM_FORMATRANGE, wParam, lParam);
                System.Runtime.InteropServices.Marshal.FreeCoTaskMem(lParam);
                fromAPI = SendMessage(rtb.Handle, EM_FORMATRANGE, wParam, new IntPtr(0));
                gr.ReleaseHdc(hDC);
            }
            return bmp;
        }
    }
}
