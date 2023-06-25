using ManagedBass;

namespace AudiyoTests;

public class Tests {
    [Fact]
    public void Initialise() {
        Audiyo.Debug.EnableDebugLogging = true;
        Audiyo.Initialise();
    }
}
