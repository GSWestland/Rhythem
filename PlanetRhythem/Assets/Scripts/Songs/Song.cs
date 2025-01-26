using System.Collections.Generic;
using UnityEngine;

namespace Rhythem.Songs
{
    [System.Serializable]
    public class Song
    {
        public string songTitle;
        public string artist;
        [SerializeField] public List<Measure> measures;
    }
}