using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
public class Test
{
    public int MyProperty { get; set; }

    public long MyProperty1 { get; set; }

    public int MyProperty2 { get; set; }

}
internal class Program
{
    private static void Main(string[] args)
    {
        int size = GetBytes(() =>
        {
            var test = new Test();
        });
        var size1 = Marshal.SizeOf(typeof(Test));
        var size2 = Unsafe.SizeOf<Test>();
        Console.WriteLine($"GetBytes {size}");
        Console.WriteLine($"Marshal.SizeOf {size1}");
        Console.WriteLine($"Unsafe.SizeOf {size2}");
        Console.ReadLine();
    }

    private static int GetBytes(Action action)
    {
        GC.Collect();
        GC.WaitForFullGCComplete();
        var start = GC.GetAllocatedBytesForCurrentThread();
        action?.Invoke();
        GC.Collect();
        GC.WaitForFullGCComplete();
        var end = GC.GetAllocatedBytesForCurrentThread();
        return (int)(end - start);
    }
}