using AudiyoLib;

using ManagedBass;
using ManagedBass.Mix;
using ManagedBass.Fx;

namespace AudiyoTests;

public class Tests {
    public static List<string> Samples = new List<string>();
    public static string GetRandomSample => Samples[new Random().Next(0, Samples.Count)];

    [Fact]
    public void Initialise() {
        Samples = System.IO.Directory.GetFiles("../../../Samples").ToList();
        
        Audiyo.Debug.EnableDebugLogging = true;
        Audiyo.Initialise();
        
        AudioStream stream = new AudioStream();
        stream.CreateAndSwitchHandle(GetRandomSample);
        
        float volume = 1;

        while (true) {
            ConsoleKey key = Console.ReadKey().Key;

            if (key == ConsoleKey.DownArrow) stream.SetVolume(volume -= 0.1f);
            else if (key == ConsoleKey.UpArrow) stream.SetVolume(volume += 0.1f);

            if (key == ConsoleKey.RightArrow) stream.CrossfadeHandle(stream.CreateHandle(GetRandomSample), endValue: volume);
            else if (key == ConsoleKey.LeftArrow) stream.CrossfadeHandle(stream.CreateHandle(GetRandomSample), endValue: volume);

            Console.WriteLine("-----------------");
            for (int i = 0; i < stream.Handles.Count(); i++) {
                Console.WriteLine($"Handle {stream.Handles.ElementAt(i).Key}: {stream.Handles.ElementAt(i).Value}");
            }
        }
    }
}