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
        //数组类型是有 8个字节的对象 用于 存储其他的属性的
        int size = GetBytes(() =>
        {
            int[] test = new int[0];
        });
        var size1 = Marshal.SizeOf(typeof(Test));
        var size2 = Unsafe.SizeOf<Test>();
        Console.WriteLine($"GetBytes {size}");
        Console.WriteLine($"Marshal.SizeOf {size1}");
        Console.WriteLine($"Unsafe.SizeOf {size2}");

        Thread thread = new Thread(() => {
            Console.WriteLine($"{Thread.CurrentThread.IsBackground}");
        });
        thread.Start();

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