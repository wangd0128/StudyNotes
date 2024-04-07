using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BenchmarkTest.BenchMark
{
    [MemoryDiagnoser]
    public class SubString
    {
        public string str;

        [GlobalSetup]
        public void Init()
        {
            str = "123333333333444444555555555666666666666666666666666666666666666666666666666666";
        }

        [Benchmark]
        public string Str_Sub()
        {
            return str.Substring(0, 10) + str.Substring(14, 5) + str.Substring(17, 2);
        }
        [Benchmark]
        public string Span_Sub()
        {
            ReadOnlySpan<char> span = str.AsSpan();
            unsafe
            {
                var a = &span;
                
            }
            string subString1 = new string(span.Slice(0, 10));
            string subString2 = new string(span.Slice(14, 3));
            string subString3 = new string(span.Slice(15, 1));
            return subString1 + subString2 + subString3;
        }
        [Benchmark]
        public string Point_Sub()
        {
            unsafe
            {
                fixed (char* ptr = str)
                {
                    string subString1 = new string(ptr, 0, 10);
                    string subString2 = new string(ptr + 14, 0, 3);
                    string subString3 = new string(ptr + 15, 0, 1);
                    return subString1 + subString2 + subString3;
                }

            }

        }
    }
}
