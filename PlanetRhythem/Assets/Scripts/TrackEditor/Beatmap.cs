using UnityEngine;
using Sirenix.OdinInspector;
using System;
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
        public int bpm;
        [Title("Track Editor Info")]
        public int beatsPerMeasure;
        public int subdivisionsPerBeat;
        public int numberOfMeasures;
        public AudioClip audioFile;
        [HideInInspector]
        public string trackDataPath;

        public void DoJsonTrackDataSetup()
        {
            if (songTitle == null || bpm == 0 || subdivisionsPerBeat == 0 || numberOfMeasures == 0 )
            {
                Debug.LogWarning($"Track {songTitle} is missing parameters for Data generation. Please add missing data before continuing.");
                return;
            }
            if (trackDataPath == "" || trackDataPath == null || trackDataPath.Length == 0)
            {
                var spacelessSong = songTitle.Replace(" ", "");
                trackDataPath = $"{Globals.JSON_DATA_PATH}/{spacelessSong}.json";
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// Creates JSON data file from song data
        /// </summary>
        public void SerializeSongData(Song song)
        {
            DoJsonTrackDataSetup();
            if (trackDataPath == null)
            {
                return;
            }
            using (StreamWriter outputFile = new StreamWriter(trackDataPath))
            {
                string jsonOut = JsonConvert.SerializeObject(song, Formatting.Indented);
                outputFile.Write(jsonOut);
            }
        }

        /// <summary>
        /// Gets Json data and sets up data for reading track
        /// </summary>
        public Song DeserializeSongData()
        {
            var jsonText = File.ReadAllText(trackDataPath);
            var song = JsonConvert.DeserializeObject<Song>(jsonText);
            return song;
        }

        public void FillSongWithDummyData()
        {
            var song = new Song();
            song.songTitle = songTitle;
            song.artist = artist;
            song.bpm = bpm;
            song.beatsPerMeasure = beatsPerMeasure;
            song.subdivisionsPerBeat = subdivisionsPerBeat;
            var measures = new List<Measure>();
            for (int i = 0; i < numberOfMeasures; i++)
            {
                var newMeasure = new Measure();
                for (int j = 0; j < beatsPerMeasure; j++)
                {
                    var newBeat = new Beat();
                    for (int k = 0; k < subdivisionsPerBeat; k++)
                    {
                        var newNote = new Note();
                        float x = UnityEngine.Random.Range(0.0f, 1.0f);
                        float y = UnityEngine.Random.Range(0.0f, 1.0f);
                        newNote.SetNoteInfo(NoteType.Note, new Vector2(x, y), DesiredHand.Left);
                        newBeat.notes.Add(newNote);
                    }
                    newMeasure.beats.Add(newBeat);
                }
                measures.Add(newMeasure);
            }
            song.measures = measures;
            SerializeSongData(song);
        }
    }
}