using Rhythem.TrackEditor;
using UnityEngine;
using System.Collections.Generic;

namespace Rhythem.Tracks
{/// <summary>
/// Serialized asset for storing lists of songs in folders
/// </summary>
    [CreateAssetMenu(fileName = "Tracklist", menuName = "Rhythem/Tracklist")]
    public class Tracklist : ScriptableObject
    {
        public string tracklistName;
        public Texture tracklistThumbnail;
        public List<Beatmap> tracks;
    }
}