using ManagedBass;

namespace AudiyoLib;

public static partial class Thrower {
    /// <summary>
    /// Handle potential throw relative to BASS function output
    /// </summary>
    /// <param name="result">The result of the BASS function</param>
    /// <param name="actionName">The label of the action</param>
    public static void BassAttempt(bool result, string actionName) {
        if (!result)
            throw new Exception("BASS failed to " + actionName + ": " + Bass.LastError);
    }
}