using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using Rhythem.Songs;
using Rhythem.TrackEditor;
using Rhythem.Play;
using Sirenix.OdinInspector;

namespace Rhythem.Tracks
{
    public class HighwayController : MonoBehaviour
    {
        [Title("Testing Fields")]
        public Beatmap testBeatmap;
        public GameObject notePrefab;

        private Song song;

        private List<Measure> activeMeasures = new();
        private List<GameObject> activeNotes = new();

        public Transform ringPivot;
        public int measuresPerRotation = 8;

        public EventReference popSFXEvent;
        public EventReference missSFXEvent;
        public EventReference musicEvent;

        public int score = 0;

        private int currentMeasure = 1;
        private int currentBeat = 1;
        private int currentNote = 1;

        private float measureTime = 0f;
        private float currentTime = 0f;

        void Start()
        {
            SetupSong(testBeatmap);
        }

        void Update()
        {
            if (song == null)
            {
                return;
            }
            if (currentMeasure >= song.measures.Count)
            {
                return;
            }
            currentTime += Time.deltaTime;
            if (currentTime / currentBeat / currentNote > measureTime / song.beatsPerMeasure / song.subdivisionsPerBeat)
            {
                //Debug.Log($"MEASURE {currentMeasure}--BEAT {currentBeat}--NOTE {currentNote}");
                Debug.Log(song.measures[currentMeasure - 1].beats[currentBeat - 1].notes[currentNote - 1]);
                currentNote++;
            }
            if (currentTime / currentBeat > measureTime / song.beatsPerMeasure)
            {
                currentBeat++;
                currentNote = 1;
            }
            if (currentTime > measureTime)
            {
                currentMeasure++;
                currentTime = 0f;
                currentBeat = 1;
            }
        }

        void FixedUpdate()
        {
            if (song == null)
            {
                return;
            }
            UpdateRing();

        }

        public void SetupSong(Beatmap beatmap)
        {
            song = beatmap.DeserializeSongData();
            activeMeasures.Clear();
            activeNotes.Clear();
            for (int i = 0; i < 4; i++)
            {
                activeMeasures.Add(song.measures[i]);
            }
            measureTime = song.bpm / 60f;

        }

        void UpdateRing()
        {
            ringPivot.Rotate(new Vector3(0, (song.bpm / 60f) * 360f / song.beatsPerMeasure / measuresPerRotation * Time.fixedDeltaTime, 0));
        }

        void QueueMeasure()
        {

        }
    }
}