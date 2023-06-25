using ManagedBass;

namespace Audiyo.Library;

public class Class1 {
    public static void Test() {
        Bass.Init();
        Console.WriteLine(Bass.LastError);
        Bass.Free();
    }
}
