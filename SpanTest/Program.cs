using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using BenchmarkTest.BenchMark;


internal class Program
{
    private static void Main(string[] args)
    {
        //InvertColor test = new InvertColor();
        //test.Init();

        //var bitmap = test.Span();
        //test.Save(bitmap, "44");
        var summary = BenchmarkRunner.Run<InvertColor>();
        Console.ReadLine();
    }
}

