using System.Runtime.CompilerServices;

internal class Program
{
    internal static void Main(string[] args)
    {
        DoSth();
        Console.ReadLine();
    }

    internal static async void DoSth()
    {
        Console.WriteLine("1");
        await "1";
        Test test = new Test();
        Console.WriteLine("2");
        await test;
        Console.WriteLine("3");
    }
}

public class TestAwaiter : INotifyCompletion
{
    public bool IsCompleted { get; set; }

    public void OnCompleted(Action continuation)
    {
        Console.WriteLine("OnCompleted");
        continuation?.Invoke();
    }

    public void GetResult()
    {
        Console.WriteLine("GetResult");
    }
}

public class Test
{
    public TestAwaiter GetAwaiter()
    {
        return new TestAwaiter();
        //return Task.FromResult(0);
    }
}

public static class StringExtensions
{
    public static TestAwaiter GetAwaiter(this string str)
    {
        return new TestAwaiter();
        //return Task.FromResult(0);
    }
}