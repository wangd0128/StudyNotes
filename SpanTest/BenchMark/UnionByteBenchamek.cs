using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BenchmarkTest.BenchMark
{
    public class UnionByteBenchamek
    {

        private byte[] data1;
        private byte[] data2;
        private int len1;
        private int len2;
        private int len;

        [GlobalSetup]
        public void Init()
        {
            data1 = new byte[100];
            data2 = new byte[100];
            len1 = data1.Length;
            len2 = data2.Length;
            len = data1.Length + data2.Length;
        }

        [Benchmark]
        public void Copy()
        {
            var bytes = new byte[len];
            data1.CopyTo(bytes, 0);
            data2.CopyTo(bytes, len1);
            //return bytes;
        }

        [Benchmark]
        public void ArrayCopy()
        {
            var bytes = new byte[len];
            Array.Copy(data1, 0, bytes, 0, len1);
            Array.Copy(data2, 0, bytes, len1, len2);
            //return bytes;
        }

        [Benchmark]
        public void Union()
        {
            var a = data1.Union(data2);
        }

        [Benchmark]
        public void Span()
        {
            Span<byte> combined = new byte[len];
            data1.CopyTo(combined);
            data2.CopyTo(combined.Slice(len1, len2));
            //return combined.ToArray();
        }

        [Benchmark]
        public void SpanToArray()
        {
            Span<byte> combined = new byte[len];
            data1.CopyTo(combined);
            data2.CopyTo(combined.Slice(len1, len2));
            combined.ToArray();
        }
    }
}
