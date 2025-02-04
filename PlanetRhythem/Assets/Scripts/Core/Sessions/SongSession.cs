using Rhythem.Songs;
using Rhythem.TrackEditor;
using UnityEngine;

namespace Rhythem.Core
{
    public class SongSession : Session
    {
        public Beatmap beatmap;
        public Song song;

        public int score;
        public int energy;

        public override void Initialize()
        {
            base.Initialize();
            if (beatmap != null)
            {
                song = beatmap.DeserializeSongData();
            }
            score = 0;
            energy = Globals.STARTING_ENERGY;
        }

        public override void EndSession()
        {
            base.EndSession();
            //store score in some persistent data
        }
    }
}