using chapter_07.Engine.States;

using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;

namespace chapter_07.Engine.Sound
{
    public class SoundManager
    {
        private int _soundtrackIndex = -1;
        private List<SoundEffectInstance> _soundtracks = new List<SoundEffectInstance>();
        private Dictionary<Type, SoundEffect> _soundBank = new Dictionary<Type, SoundEffect>();

        public void SetSoundtrack(List<SoundEffectInstance> tracks)
        {
            _soundtracks = tracks;
            _soundtrackIndex = _soundtracks.Count - 1;
        }

        public void OnNotify(BaseGameStateEvent gameEvent) 
        {
            if (_soundBank.ContainsKey(gameEvent.GetType()))
            {
                var sound = _soundBank[gameEvent.GetType()];
                sound.Play();
            }
        }

        public void PlaySoundtrack()
        {
            var nbTracks = _soundtracks.Count;

            if (nbTracks <= 0)
            {
                return;
            }

            var currentTrack = _soundtracks[_soundtrackIndex];
            var nextTrack = _soundtracks[(_soundtrackIndex + 1) % nbTracks];

            if (currentTrack.State == SoundState.Stopped)
            {
                nextTrack.Play();
                _soundtrackIndex++;

                if (_soundtrackIndex >= _soundtracks.Count)
                {
                    _soundtrackIndex = 0;
                }
            }
        }

        public void RegisterSound(BaseGameStateEvent gameEvent, SoundEffect sound)
        {
            _soundBank.Add(gameEvent.GetType(), sound);
        }
    }
}
