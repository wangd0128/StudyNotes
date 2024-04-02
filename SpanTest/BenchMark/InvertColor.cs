using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using static System.Runtime.InteropServices.JavaScript.JSType;
using BenchmarkDotNet.Attributes;
using OpenCvSharp;
using Point = System.Drawing.Point;

namespace BenchmarkTest.BenchMark
{
    public class InvertColor
    {
        public string filename = "F:\\Github\\Images\\test";
        public string suffix = @".png";
        public object lockObj = new object();
        public Bitmap thispic;


        [GlobalSetup]
        public void Init()
        {
            thispic = new Bitmap($"{filename}{suffix}");
        }


        [Benchmark]
        public Bitmap Tradition()
        {
            Bitmap src = new Bitmap(Image.FromHbitmap(thispic.GetHbitmap())); // 加载图像
            BitmapData srcdat = src.LockBits(new Rectangle(System.Drawing.Point.Empty, src.Size), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb); // 锁定位图
            unsafe // 不安全代码
            {
                byte* pix = (byte*)srcdat.Scan0; // 像素首地址
                for (int i = 0; i < srcdat.Stride * srcdat.Height; i++)
                    pix[i] = (byte)(255 - pix[i]);
            }
            src.UnlockBits(srcdat); // 解锁
            return src;
        }


        //[Benchmark]
        public Bitmap MultiThread()
        {
            Bitmap src = new Bitmap(Image.FromHbitmap(thispic.GetHbitmap())); // 加载图像
            BitmapData srcdat = src.LockBits(new Rectangle(Point.Empty, src.Size), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb); // 锁定位图
            
            Parallel.ForEach(Partitioner.Create(0, srcdat.Height), (H) =>
            {
                int X, Y, Width, Height, Stride;
                Width = srcdat.Width;
                Height = srcdat.Height;
                Stride = srcdat.Stride;
                var scan0 = srcdat.Scan0;
                unsafe
                {
                    byte* pix = (byte*)scan0; // 像素首地址
                    for (Y = H.Item1; Y < H.Item2; Y++)
                    {
                        int CurP = Y * Stride;
                        for (X = 0; X < Width; X++)
                        {
                            pix[CurP] = (byte)(255 - pix[CurP]);
                            pix[CurP+1] = (byte)(255 - pix[CurP+1]);
                            pix[CurP+2] = (byte)(255 - pix[CurP+2]);
                            CurP += 3;
                        }
                    }
                }
                
            });
            src.UnlockBits(srcdat); // 解锁
            return src;
        }

        [Benchmark]
        public Bitmap Tradition2()
        {
            Bitmap src = new Bitmap(Image.FromHbitmap(thispic.GetHbitmap())); // 加载图像
            BitmapData srcdat = src.LockBits(new Rectangle(Point.Empty, src.Size), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb); // 锁定位图
            var scan0 = srcdat.Scan0;
            for (int i = 0; i < srcdat.Stride * srcdat.Height; i++)
                Marshal.WriteByte(scan0 + i, (byte)(255 - Marshal.ReadByte(scan0 + i)));
            src.UnlockBits(srcdat); // 解锁
            return src;
        }

        [Benchmark]
        public Bitmap Span()
        {
            Bitmap src = new Bitmap(Image.FromHbitmap(thispic.GetHbitmap())); // 加载图像
            BitmapData srcdat = src.LockBits(new Rectangle(Point.Empty, src.Size), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb); // 锁定位图
            unsafe
            {
                var spanBytes = new Span<byte>(srcdat.Scan0.ToPointer(), srcdat.Stride * srcdat.Height);
                for (int i = 0; i < spanBytes.Length; i++)
                    spanBytes[i] = (byte)(255 - spanBytes[i]);
            }
            src.UnlockBits(srcdat); // 解锁
            return src;
        }

        //[Benchmark]
        public Bitmap MultiThread2()
        {

            Bitmap Bmp = new Bitmap(Image.FromHbitmap(thispic.GetHbitmap())); // 加载图像
            BitmapData BmpData = Bmp.LockBits(new Rectangle(0, 0, Bmp.Width, Bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            
            Parallel.ForEach(Partitioner.Create(0, BmpData.Height), (H) =>
            {
                int X, Y, Width, Height, Stride;
                IntPtr Scan0, CurP;
                Width = BmpData.Width;
                Height = BmpData.Height;
                Stride = BmpData.Stride;
                Scan0 = BmpData.Scan0;
                for (Y = H.Item1; Y < H.Item2; Y++)
                {
                    CurP = Scan0 + Y * Stride;
                    for (X = 0; X < Width; X++)
                    {
                        byte b = (byte)(255 - Marshal.ReadByte(CurP));
                        byte g = (byte)(255 - Marshal.ReadByte(CurP + 1));
                        byte r = (byte)(255 - Marshal.ReadByte(CurP + 2));
                        Marshal.WriteByte(CurP, b);
                        Marshal.WriteByte(CurP + 1, g);
                        Marshal.WriteByte(CurP + 2, r);
                        CurP += 3;
                    }
                }
            });
            Bmp.UnlockBits(BmpData);
            return Bmp;
        }

        public void opencv()
        {
            Mat image = Cv2.ImRead($"{filename}{suffix}", ImreadModes.Color); // 读取彩色图像
            Cv2.BitwiseNot(image, image); // 颜色反转
            image.SaveImage($"{filename}_invert_{"3333"}{suffix}");
            Cv2.WaitKey(0); // 等待按键
        }

        public void Save(Bitmap bitmap, string savefile)
        {
            bitmap.Save($"{filename}_invert_{savefile}{suffix}");
        }
    }
}
