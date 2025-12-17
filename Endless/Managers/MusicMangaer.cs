using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Endless.Managers
{
    public static class MusicMangaer
    {
        public static Song CurrentSong { get; private set; }

        public static void Play(Song song)
        {
            if (CurrentSong == song)
                return;

            CurrentSong = song;
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(song);
        }
    }
}
