using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenchmarkTest.BenchMark
{
    public class SpanBenchmark
    {
        private const int N = 1000000;
        private int[] data;

        [GlobalSetup]
        public void GlobalSetup()
        {
            data = new int[N];
            for (int i = 0; i < N; i++)
            {
                data[i] = i;
            }
        }

        [Benchmark]
        public int ArrayAccess()
        {
            int sum = 0;
            for (int i = 0; i < data.Length; i++)
            {
                sum += data[i];
            }
            return sum;
        }

        [Benchmark]
        public int SpanAccess()
        {
            int sum = 0;
            Span<int> span = data;
            foreach (var item in span)
            {
                sum += item;
            }
            return sum;
        }
    }
}
