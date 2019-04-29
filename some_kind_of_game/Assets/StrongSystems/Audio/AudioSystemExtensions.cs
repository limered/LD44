using System;
using StrongSystems.Audio.Actions;
using UniRx;

namespace StrongSystems.Audio
{
    public static class AudioSystemExtensions
    {
        public static void Play(this string soundName, string tag = null)
        {
            MessageBroker.Default.Publish(new AudioActSFXPlay { Name = soundName, Tag = tag });
        }

        public static void Start(this string soundName)
        {
            MessageBroker.Default.Publish(new AudioActMusicStart {Name = soundName, CrossFadeTime = TimeSpan.MaxValue});
        }

        public static void SetVolume(this string soundName, float volume)
        {
            MessageBroker.Default.Publish(new AudioActSFXSetVolume(volume));
        }
    }
}