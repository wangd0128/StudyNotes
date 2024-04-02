using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenchmarkTest.Keywords
{
    public class PartitionerTest: IKeywordsTest
    {
        public void test()
        {
            var source = new List<string> { "100", "200", "300" };
            var a = Partitioner.Create(0, 10);
            Console.WriteLine(string.Join(",", a));
        }
    }
}
