using BenchmarkDotNet.Attributes;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenchmarkTest.BenchMark
{
    public class Rgb2Gray
    {

        public object lockObj = new object();

        public string filename = "F:\\Github\\Images\\test";
        public string suffix = @".jpg";


        [Benchmark]
        public void Tradition()
        {
            lock (lockObj)
            {
                using (Bitmap curBitmap = new Bitmap($"{filename}{suffix}"))
                {
                    ConvertToGray(curBitmap, 0, curBitmap.Width, 0, curBitmap.Height);
                    //curBitmap.Save($"{filename}_gray{suffix}");
                }
            }


        }

        [Benchmark]
        public void MultiThread()
        {
            lock (lockObj)
            {
                var piece_row = 3;
                var piece_col= 4;
                using (Bitmap curBitmap = new Bitmap($"{filename}{suffix}"))
                {
                    var piece_width = curBitmap.Width % piece_col > 0 ? (curBitmap.Width / piece_col + 1) : curBitmap.Width / piece_col;
                    var piece_height = curBitmap.Height % piece_row > 0 ? (curBitmap.Height / piece_row + 1) : curBitmap.Height / piece_row;
                    var col_start = 0;
                    var row_start = 0;
                    var tasks = new List<Task>();   
                    for (int m = 0; m < piece_col; m++)
                    {
                        var col_len = m == piece_col - 1 ? (curBitmap.Width - col_start) : piece_width;
                        for (int n = 0; n < piece_row; n++)
                        {
                            var row_len = n == piece_row - 1 ? (curBitmap.Height - row_start) : piece_height;

                            tasks.Add(Task.Run(() =>
                            {
                                var _col_start = col_start;
                                var _col_len = col_len;
                                var _row_start = row_start;
                                var _row_len = row_len;
                                ConvertToGray(curBitmap, _col_start, _col_len, _row_start, _row_len);
                            }));
                            row_start = n == piece_row - 1 ? 0 : (row_start + row_len);
                        }
                        col_start = m == piece_col - 1 ? 0 : (col_start + col_len);

                    }
                    Task.WaitAll(tasks.ToArray());
                    curBitmap.Save($"{filename}_gray{suffix}");
                }
            }

        }


        private void ConvertToGray(Bitmap curBitmap, int col_start, int col_len, int row_start, int row_len)
        {
            for (int i = col_start; i < col_start + col_len; i++)
            {
                for (int j = row_start; j < row_start + row_len; j++)
                {
                    var curColor = curBitmap.GetPixel(i, j);//获取该像素点的RGB颜色值
                    var ret = (int)(curColor.R * 0.299 + curColor.G * 0.587 + curColor.B * 0.114);//利用公式计算灰度值
                    curBitmap.SetPixel(i, j, Color.FromArgb(ret, ret, ret));//设置该像素的灰度值  R=G=B=ret
                }
            }
        }
    }

    public class SimulatedBitmap
    {

        public SimulatedBitmap(byte[,] R, byte[,] G, byte[,] B)
        {
            this.R = R;
            this.G = G;
            this.B = B;
            this.Height = R.GetLength(0);
            this.Height = R.Length;
        }
        public byte[,] R { get; set; }

        public byte[,] G { get; set; }

        public byte[,] B { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }
    }
}
