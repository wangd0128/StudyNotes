using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenchmarkTest.BenchMark
{
    public class SpanBenchmark2
    {
        public string str = "";


        [GlobalSetup]
        public void Init()
        {
            str = "2022 4 1";
            Console.WriteLine("开始");
        }

        [Benchmark]
        public (int year, int month, int day) Substring()
        {
            var year = int.Parse(str.Substring(0, 4));
            var month = int.Parse(str.Substring(5, 1));
            var day = int.Parse(str.Substring(7, 1));
            return (year, month, day);
        }

        [Benchmark]
        public (int year, int month, int day) Span()
        {
            var span = str.AsSpan();
            var year = int.Parse(span.Slice(0, 4));
            var month = int.Parse(span.Slice(5, 1));
            var day = int.Parse(span.Slice(7, 1));
            return (year, month, day);
        }
    }
}
