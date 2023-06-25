using AudiyoLib;

namespace AudiyoTests.ConsoleTests;

public static class Program {
    public static List<string> Samples = new List<string>();
    public static string GetRandomSample => Samples[new Random().Next(0, Samples.Count)];

    public static void Main(String[] args) {
        Samples = System.IO.Directory.GetFiles("./Samples").ToList();

        Console.Clear();
        
        Audiyo.Debug.EnableDebugLogging = false;
        Audiyo.Initialise();

        string song = GetRandomSample;

        AudioStream stream = new AudioStream();
        stream.CreateAndSwitchHandle(song);

        bool playing = true;

        float Lerp(float a, float b, float t) => a + (b - a) * t;

        float frequency = stream.GetFrequency();

        void UpdatePosition() {
            double length = stream.GetLength();
            double position = stream.GetPosition();
            double progress = position / length;

            // Progressively lower the frequency as the song progresses:
            // float lerping = Lerp(frequency, frequency / 2, (float)progress);
            // ManagedBass.Bass.ChannelSetAttribute(stream.CurrentHandle, ManagedBass.ChannelAttribute.Frequency, lerping);

            int fill = (int)((float)Console.WindowWidth * progress);
            int empty = Console.WindowWidth - fill;

            string bar = new string('█', fill) + new string('░', empty);
            Console.SetCursorPosition(0, 0);
            string substring = song.Substring(song.LastIndexOf('/') + 1);
            substring = substring.Substring(0, substring.LastIndexOf('.'));
            Console.Write(bar + "\n" + substring + "         " + "\n" + Math.Round(position, 2) + " / " + Math.Round(length, 2) + " (" + Math.Round(progress * 100, 2) + "%)" + "         ");
        }

        Thread inputThread = new Thread(() => {
            while (true) {
                ConsoleKey key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.LeftArrow) {
                    stream.SetPosition(stream.GetPosition() - 5);
                } else if (key == ConsoleKey.RightArrow) {
                    stream.SetPosition(stream.GetPosition() + 5);
                } else if (key == ConsoleKey.Spacebar) {
                    if (playing) {
                        stream.Pause();
                    } else {
                        stream.Play();
                    }

                    playing = !playing;
                } else if (key == ConsoleKey.Enter) {
                    song = GetRandomSample;
                    stream.CreateAndSwitchHandle(song);
                } else if (key == ConsoleKey.L) {
                    song = GetRandomSample;
                    stream.CrossfadeHandle(stream.CreateHandle(song));
                }

                UpdatePosition();
            }
        });

        inputThread.Start();

        while (true) {
            UpdatePosition();
            Thread.Sleep(500);
        }
    }
}
