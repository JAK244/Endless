using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Endless.Managers
{
    public class AudioSettings
    {
        public static float MusicVolume { get; private set; } = 1.0f;
        public static float SfxVolume { get; private set; } = 1.0f;

        public static void SetMusicVolume(float value)
        {
            MusicVolume = MathHelper.Clamp(value, 0f, 1f);
            MediaPlayer.Volume = MusicVolume;
        }

        public static void SetSfxVolume(float value)
        {
            SfxVolume = MathHelper.Clamp(value, 0f, 1f);
            SoundEffect.MasterVolume = SfxVolume;
        }
    }
}
