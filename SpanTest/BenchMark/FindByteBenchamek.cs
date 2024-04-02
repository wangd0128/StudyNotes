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
    public class FindByteBenchamek
    {

        private byte[] data;
        private byte[] pattern;

        [GlobalSetup]
        public void Init()
        {
            data = new byte[] { 1, 2, 3, 1, 5, 6, 1, 5, 9};
            pattern = new byte[] { 1, 5};
        }

        [Benchmark]
        public int array()
        {
            for (int i = 0; i <= data.Length - pattern.Length; i++)
            {
                bool found = true;
                for (int j = 0; j < pattern.Length; j++)
                {
                    if (data[i + j] != pattern[j])
                    {
                        found = false;
                        break;
                    }
                }
                if (found)
                {
                    return i;
                }
            }
            return -1;
        }

        [Benchmark]
        public int span()
        {
            Span<byte> span = data;
            for (int i = 0; i <= data.Length - pattern.Length; i++)
            {
                if (span.Slice(i, pattern.Length).SequenceEqual(pattern))
                {
                    return i;
                }
            }
            return -1;
        }

        [Benchmark]
        public int AsSpan()
        {
            var span = data.AsSpan();
            for (int i = 0; i <= data.Length - pattern.Length; i++)
            {
                if (span.Slice(i, pattern.Length).SequenceEqual(pattern))
                {
                    return i;
                }
            }
            return -1;
        }


    }
}
