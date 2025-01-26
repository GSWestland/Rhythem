using UnityEngine;
using System.Collections.Generic;
using Rhythem.Core;
using Rhythem.TrackEditor;
using Rhythem.Songs;

namespace Rhythem
{
    public class SongTrackManager : Session
    {
        public Beatmap beatmap;
        private Song song;

        void Start()
        {
            song = beatmap.DeserializeSongData();
        }

        void Update()
        {

        }

        public override void Initialize()
        {

        }
    }
}