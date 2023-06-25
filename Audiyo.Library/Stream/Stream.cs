using System;
using System.Collections.Generic;
using System.Linq;
using ManagedBass;

namespace AudiyoLib;

public partial class AudioStream {
    // Useful for debug logs
    private string inStreamStr => " in stream " + this.GetHashCode();

    /// <summary>
    /// Array of BASS handles for this stream.
    /// </summary>
    public Dictionary<int, int> Handles { get; private set; }

    /// <summary>
    /// The current index of the stream.
    /// </summary>
    public int CurrentIndex { get; private set; }

    /// <summary>
    /// The current BASS handle, relative to <see cref="CurrentIndex"/>.
    /// </summary>
    public int CurrentHandle => Handles[CurrentIndex];

    /// <summary>
    /// Attempt to run an action, only if the spesified index is valid.
    /// </summary>
    /// <param name="index">The index to check</param>
    /// <param name="action">The action to run</param>
    public void AttemptAction(int index, Action action) {
        if (Handles.ContainsKey(index))
            action();
    }

    /// <summary>
    /// Attempt to run an action as a task, only if the spesified index is valid.
    /// </summary>
    /// <param name="index">The index to check</param>
    /// <param name="action">The action to run</param>
    public void AttemptTaskedAction(int index, Action action) {
        if (Handles.ContainsKey(index))
            new Task(action).Start();
    }

    /// <summary>
    /// Dispose of the stream at the spesified index.
    /// </summary>
    /// <param name="index">The index to dispose of</param>
    /// <param name="delay">The delay before disposing</param>
    public void DisposeHandle(int index, int delay = 0) {
        AttemptTaskedAction(index, () => {
            if (delay > 0) Task.Delay(delay).Wait();
            Audiyo.Debug.Log("Disposing of handle " + index + inStreamStr + ".");
            Bass.StreamFree(Handles[index]);
            Handles.Remove(index);
            if (CurrentIndex == index) CurrentIndex = -1;
        });
    }

    private int handleKey = 0;

    /// <summary>
    /// Create a new handle for the spesified file.
    /// </summary>
    /// <param name="file">The file to create a handle for</param>
    /// <param name="flags">The flags to use when creating the handle</param>
    /// <returns>The index of the new handle</returns>
    public int CreateHandle(string file, BassFlags flags = BassFlags.Default) {
        Audiyo.Debug.Log("Creating new handle for " + file + inStreamStr + ".");
        int BASSHandle = Bass.CreateStream(file, Flags: flags);
        int key = handleKey++;
        Handles.Add(key, BASSHandle);
        return key;
    }

    /// <summary>
    /// Creates a new handle, and makes it the current handle.
    /// </summary>
    /// <param name="file">The file to create a handle for</param>
    /// <param name="flags">The flags to use when creating the handle</param>
    /// <param name="play">Whether or not to play the handle after creating it</param>
    /// <returns>The index of the new handle</returns>
    public int CreateAndSwitchHandle(string file, BassFlags flags = BassFlags.Default, bool play = true) {
        int newHandle = CreateHandle(file, flags);
        if (play) SwitchHandle(newHandle);
        return newHandle;
    }

    /// <summary>
    /// Switch playback to the spesified index.
    /// </summary>
    /// <param name="index">The index to switch to</param>
    /// <param name="play">Whether or not to play the handle after switching</param>
    public void SwitchHandle(int index, bool play = true) {
        AttemptAction(index, () => {
            if (CurrentHandle != -1) Bass.ChannelPause(CurrentHandle);
            CurrentIndex = index;
            if (play) Bass.ChannelPlay(CurrentHandle);
        });
    }
    
    /// <summary>
    /// Fade in playback to the spesified index.
    /// </summary>
    /// <param name="index">The index to fade in to</param>
    /// <param name="duration">The duration of the fade</param>
    /// <param name="endValue">The end value of the fade</param>
    public void FadeInHandle(int index, int duration = 1000, float endValue = 1) {
        AttemptAction(index, () => {
            if (CurrentHandle != -1) Bass.ChannelPause(CurrentHandle);
            CurrentIndex = index;
            Bass.ChannelSetAttribute(CurrentHandle, ChannelAttribute.Volume, 0);
            Bass.ChannelSlideAttribute(CurrentHandle, ChannelAttribute.Volume, endValue, duration);
            Bass.ChannelPlay(CurrentHandle);
        });
    }

    /// <summary>
    /// Crossfade playback to the spesified index.
    /// </summary>
    /// <param name="index">The index to crossfade to</param>
    /// <param name="duration">The duration of the crossfade</param>
    /// <param name="endValue">The end value of the crossfade</param>
    /// <param name="disposeOld">Whether or not to dispose of the old handle</param>
    /// <param name="pauseOld">Whether or not to pause the old handle</param>
    public void CrossfadeHandle(int index, int duration = 1000, float endValue = 1, bool disposeOld = true, bool pauseOld = false) {
        AttemptAction(index, () => {
            if (CurrentHandle != -1) {
                int oldHandle = CurrentHandle, oldIndex = CurrentIndex;
                Bass.ChannelSlideAttribute(oldHandle, ChannelAttribute.Volume, 0, duration);

                if (disposeOld)
                    Util.PerformIn(duration, () => DisposeHandle(oldIndex));
                else if (pauseOld)
                    Util.PerformIn(duration, () => Pause(oldIndex));
            }

            CurrentIndex = index;
            Bass.ChannelSetAttribute(CurrentHandle, ChannelAttribute.Volume, 0);
            Bass.ChannelSlideAttribute(CurrentHandle, ChannelAttribute.Volume, endValue, duration);
            Bass.ChannelPlay(CurrentHandle);
        });
    }

    /// <summary>
    /// Base <see cref="AudioStream"/> constructor.
    /// </summary>
    public AudioStream() {
        Handles = new Dictionary<int, int>();
    }

    /// <summary>
    /// <see cref="AudioStream"/> constructor with a single file.
    /// </summary>
    /// <param name="file">The file to create a handle for</param>
    /// <param name="flags">The flags to use when creating the handle</param>
    /// <param name="play">Whether or not to play the handle after creating it</param>
    public AudioStream(string file, BassFlags flags = BassFlags.Default, bool play = false) {
        Handles = new Dictionary<int, int>();
        CreateAndSwitchHandle(file, flags);
        if (play) Play();
    }
}
