using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using BenchmarkTest.BenchMark;


internal class Program
{
    private static void Main(string[] args)
    {
        var summary = BenchmarkRunner.Run<Ergodic>();
        Console.ReadLine();
    }
}

