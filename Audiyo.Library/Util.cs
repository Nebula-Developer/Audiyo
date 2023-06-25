
namespace AudiyoLib;

public static class Util {
    /// <summary>
    /// Perform an action after a delay as a task.
    /// </summary>
    /// <param name="delay">The delay before performing the action</param>
    /// <param name="action">The action to perform</param>
    public static void PerformIn(int delay, Action action) {
        new Task(() => {
            Task.Delay(delay).Wait();
            action();
        }).Start();
    }
}
