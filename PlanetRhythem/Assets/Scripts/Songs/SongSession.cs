using UnityEngine;
using System.Collections.Generic;
using Rhythem.Core;
using Rhythem.TrackEditor;
using Rhythem.Songs;
using Rhythem.Tracks;

namespace Rhythem.Play
{
    public class SongSession : Session
    {
        public Beatmap beatmap;
        private Song song;
        private HighwayController highwayController;

        void Start()
        {
        }

        void Update()
        {

        }

        public override void Initialize()
        {
            song = beatmap.DeserializeSongData();
            if (song.measures.Count > 0)
            {
                highwayController.SetupSong(beatmap);
            }
        }
    }
}