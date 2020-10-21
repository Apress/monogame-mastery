using Microsoft.Xna.Framework.Input.Touch;

namespace chapter_10.Engine.Sound
{
    public class SoundAttributes
    {
        //   volume:
        //     Volume, ranging from 0.0 (silence) to 1.0 (full volume). Volume during playback
        //     is scaled by SoundEffect.MasterVolume.
        //
        //   pitch:
        //     Pitch adjustment, ranging from -1.0 (down an octave) to 0.0 (no change) to 1.0
        //     (up an octave).
        //
        //   pan:
        //     Panning, ranging from -1.0 (left speaker) to 0.0 (centered), 1.0 (right speaker).

        public float Volume { get; set; }
        public float Pitch { get; set; }
        public float Pan { get; set; }

        public SoundAttributes(float volume, float pitch, float pan)
        {
            Volume = volume;
            Pitch = pitch;
            Pan = pan;
        }
    }
}
