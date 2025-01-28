using System.Collections.Generic;
using UnityEngine;

namespace Rhythem.Songs
{
    [System.Serializable]
    public class Song
    {
        public string songTitle;
        public string artist;
        public int bpm;
        public int beatsPerMeasure;
        public int subdivisionsPerBeat;
        public float startWaitTime;
        [SerializeField] public List<Measure> measures;
    }
}