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
    public class Ergodic
    {
        public string str;

        [GlobalSetup]
        public void Init()
        {
            str = "123333333333444444555555555666666666666666a6666666666b66666666666666666666666666";
        }

        [Benchmark]
        public string Str_Ergodic()
        {
            var new_str = new char[str.Length];
            for (int i = 0; i < str.Length; i++)
            {
                new_str[i] = char.ToUpper(str[i]);
            }
            return new string(new_str);
        }
        [Benchmark]
        public string Span_Ergodic()
        {
            var chs = str.ToCharArray();
            Span<char> span = chs;
            for (int i = 0; i < span.Length; i++)
            {
                span[i] = char.ToUpper(span[i]);
            }
            return new string(chs);
        }
        [Benchmark]
        public string Point_Ergodic()
        {
            unsafe
            {
                fixed (char* ptr = str)
                {
                    for (int i = 0; i < str.Length; i++)
                    {
                        ptr[i] = char.ToUpper(ptr[i]);
                    }
                    return str;
                }

            }

        }
    }
}
