
namespace AudiyoLib;

public static partial class Audiyo {
    public static class Debug {
        /// <summary>
        /// Whether to log debug messages to the console
        /// </summary>
        public static bool EnableDebugLogging { get; set; } = false;

        /// <summary>
        /// Debug log history
        /// </summary>
        public static List<string> DebugLogHistory { get; set; } = new();

        /// <summary>
        /// Log a debug message
        /// </summary>
        /// <param name="message">The message to log</param>
        public static void Log(string message) {
            if (EnableDebugLogging)
                Console.WriteLine("[audiyo] DEBUG: " + message);
            DebugLogHistory.Add(message);
        }
    }
}