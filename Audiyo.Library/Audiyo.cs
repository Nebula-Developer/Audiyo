using ManagedBass;
using ManagedBass.Fx;
using ManagedBass.Mix;

namespace AudiyoLib;

public static partial class Audiyo {
    /// <summary>
    /// Initialise BASS and Audiyo
    /// </summary>
    public static void Initialise() {
        if (!Bass.Init())
            throw new Exception("BASS failed to initialise: " + Bass.LastError);

        Debug.Log("BassMix Version: " + BassMix.Version);
        Debug.Log("BassFx Version: " + BassFx.Version);
        Debug.Log("Bass Version: " + Bass.Version);
    }
}
