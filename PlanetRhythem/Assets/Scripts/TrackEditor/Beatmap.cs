using UnityEngine;
using Sirenix.OdinInspector;

namespace Rhythem.TrackEditor {
    /// <summary>
    /// Container to hold all song data before editing begins.
    /// Create a new instance of this, fill in your data, and then load it into the TrackEditorManager
    /// </summary>
    [CreateAssetMenu(fileName = "Beatmap", menuName = "Rhythem/Beatmap")]
    public class Beatmap : ScriptableObject
    {
        [Title("Track Card Info")]
        public string songTitle;
        public string artist;
        public int bpm;
        [Title("Track Editor Info")]

        public int beats;
        public int subdivisions;
        public int numberOfMeasures;
        public AudioClip songFile;
        public string trackDataPath;
    }
}