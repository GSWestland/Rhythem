using UnityEngine;
using Sirenix.OdinInspector;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using Rhythem.Songs;

namespace Rhythem.TrackEditor
{
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
        public int bPM;
        [Title("Track Editor Info")]
        public int subdivisions;
        public int numberOfMeasures;
        public AudioClip songFile;
        [HideInInspector]
        public string trackDataPath;

        public List<Measure> measures = new List<Measure>();

        public void DoJsonTrackDataSetup()
        {
            if (songTitle == null || bPM == 0 || subdivisions == 0 || numberOfMeasures == 0 )
            {
                Debug.LogWarning($"Track {songTitle} is missing parameters for Data generation. Please add missing data before continuing.");
                return;
            }
            if (trackDataPath == "" || trackDataPath == null || trackDataPath.Length == 0)
            {
                var spacelessSong = songTitle.Replace(" ", "");
                trackDataPath = $"{Globals.JSON_DATA_PATH}/{spacelessSong}.json";

            }
        }

        /// <summary>
        /// Creates JSON data file from song data
        /// </summary>
        public void SerializeSongData()
        {
            
        }

        /// <summary>
        /// Gets Json data and sets up data for reading track
        /// </summary>
        public void DeserializeSongData()
        {
            var jsonText = File.ReadAllText(trackDataPath);
            var jObject = JsonConvert.DeserializeObject<Beatmap>(jsonText);
            Debug.Log(jObject);
            //songData = ;
            //foreach (var beat in songMeasuresObject["beats"])
            //{
            //    Debug.Log(beat);
            //}
        }
    }
}