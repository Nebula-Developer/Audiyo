using System;
using System.Collections.Generic;
using System.Linq;
using ManagedBass;

namespace AudiyoLib;

public partial class AudioStream {
    /// <summary>
    /// Pause the current handle.
    /// </summary>
    public void Pause() => Pause(CurrentIndex);

    /// <summary>
    /// Pause the handle at the spesified index.
    /// </summary>
    /// <param name="index">The index to pause</param>
    public void Pause(int index) => AttemptAction(index, () => Bass.ChannelPause(Handles[index]));

    /// <summary>
    /// Play the current handle.
    /// </summary>
    public void Play() => Play(CurrentIndex);

    /// <summary>
    /// Play the handle at the spesified index.
    /// </summary>
    /// <param name="index">The index to play</param>
    public void Play(int index) => AttemptAction(index, () => Bass.ChannelPlay(Handles[index]));

    /// <summary>
    /// Stop the current handle, halting playback.
    /// </summary>
    public void Stop() => Stop(CurrentIndex);

    /// <summary>
    /// Stop the handle at the spesified index, halting playback.
    /// </summary>
    /// <param name="index">The index to stop</param>
    public void Stop(int index) => AttemptAction(index, () => Bass.ChannelStop(Handles[index]));

    /// <summary>
    /// Get the volume of the current handle.
    /// </summary>
    /// <returns>The volume of the current handle</returns>
    public float GetVolume() => GetVolume(CurrentIndex);

    /// <summary>
    /// Get the volume of the handle at the spesified index.
    /// </summary>
    /// <param name="index">The index to get the volume of</param>
    /// <returns>The volume of the handle at the spesified index</returns>
    public float GetVolume(int index) => (float)Bass.ChannelGetAttribute(Handles[index], ChannelAttribute.Volume);

    /// <summary>
    /// Set the volume of the current handle.
    /// </summary>
    /// <param name="volume">The volume to set (0-1)</param>
    public void SetVolume(float volume) => SetVolume(CurrentIndex, volume);

    /// <summary>
    /// Set the volume of the handle at the spesified index.
    /// </summary>
    /// <param name="index">The index to set the volume of</param>
    /// <param name="volume">The volume to set (0-1)</param>
    public void SetVolume(int index, float volume) => AttemptAction(index, () => Bass.ChannelSetAttribute(Handles[index], ChannelAttribute.Volume, volume));

    /// <summary>
    /// Get the length of the current handle.
    /// </summary>
    /// <returns>The length of the current handle</returns>
    public double GetLength() => GetLength(CurrentIndex);

    /// <summary>
    /// Get the length of the handle at the spesified index.
    /// </summary>
    /// <param name="index">The index to get the length of</param>
    /// <returns>The length of the handle at the spesified index</returns>
    public double GetLength(int index) => Bass.ChannelBytes2Seconds(Handles[index], Bass.ChannelGetLength(Handles[index]));

    /// <summary>
    /// Get the playback position of the current handle.
    /// </summary>
    /// <returns>The playback position of the current handle</returns>
    public double GetPosition() => GetPosition(CurrentIndex);

    /// <summary>
    /// Get the playback position of the handle at the spesified index.
    /// </summary>
    /// <param name="index">The index to get the playback position of</param>
    /// <returns>The playback position of the handle at the spesified index</returns>
    public double GetPosition(int index) => Bass.ChannelBytes2Seconds(Handles[index], Bass.ChannelGetPosition(Handles[index]));

    /// <summary>
    /// Set the playback position of the current handle.
    /// </summary>
    /// <param name="position">The playback position to set</param>
    public void SetPosition(double position) => SetPosition(CurrentIndex, position);

    /// <summary>
    /// Set the playback position of the handle at the spesified index.
    /// </summary>
    /// <param name="index">The index to set the playback position of</param>
    /// <param name="position">The playback position to set</param>
    public void SetPosition(int index, double position) => AttemptAction(index, () => Bass.ChannelSetPosition(Handles[index], Bass.ChannelSeconds2Bytes(Handles[index], position)));

    /// <summary>
    /// Get the current state of the current handle.
    /// </summary>
    /// <returns>The current state of the current handle</returns>
    public PlaybackState GetState() => GetState(CurrentIndex);

    /// <summary>
    /// Get the current state of the handle at the spesified index.
    /// </summary>
    /// <param name="index">The index to get the current state of</param>
    /// <returns>The current state of the handle at the spesified index</returns>
    public PlaybackState GetState(int index) => (PlaybackState)Bass.ChannelIsActive(Handles[index]);

    /// <summary>
    /// Fade an attribute of the current handle.
    /// </summary>
    /// <param name="attribute">The attribute to fade</param>
    /// <param name="value">The value to fade to (relative to attribute)</param>
    /// <param name="time">The time to fade over (in ms)</param>
    public void FadeAttribute(ChannelAttribute attribute, float value, int time) => FadeAttribute(CurrentIndex, attribute, value, time);

    /// <summary>
    /// Fade an attribute of the handle at the spesified index.
    /// </summary>
    /// <param name="index">The index to fade the volume of</param>
    /// <param name="attribute">The attribute to fade</param>
    /// <param name="value">The value to fade to (relative to attribute)</param>
    /// <param name="time">The time to fade over (in ms)</param>
    public void FadeAttribute(int index, ChannelAttribute attribute, float value, int time) => Bass.ChannelSlideAttribute(Handles[index], attribute, value, time);

    /// <summary>
    /// Fade the volume of the current handle.
    /// </summary>
    /// <param name="volume">The volume to fade to (0-1)</param>
    /// <param name="time">The time to fade over (in ms)</param>
    public void FadeVolume(float volume, int time) => FadeVolume(CurrentIndex, volume, time);

    /// <summary>
    /// Fade the volume of the handle at the spesified index.
    /// </summary>
    /// <param name="index">The index to fade the volume of</param>
    /// <param name="volume">The volume to fade to (0-1)</param>
    /// <param name="time">The time to fade over (in ms)</param>
    public void FadeVolume(int index, float volume, int time) => FadeAttribute(index, ChannelAttribute.Volume, volume, time);

    /// <summary>
    /// Get the frequency of the current handle.
    /// </summary>
    /// <returns>The frequency of the current handle</returns>
    public float GetFrequency() => GetFrequency(CurrentIndex);

    /// <summary>
    /// Get the frequency of the handle at the spesified index.
    /// </summary>
    /// <param name="index">The index to get the frequency of</param>
    /// <returns>The frequency of the handle at the spesified index</returns>
    public float GetFrequency(int index) => (float)Bass.ChannelGetAttribute(Handles[index], ChannelAttribute.Frequency);
}
