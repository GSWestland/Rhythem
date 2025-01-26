using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using Rhythem.Songs;
using Rhythem.TrackEditor;
using Sirenix.OdinInspector;

namespace Rhythem.Tracks
{
    public class HighwayController : MonoBehaviour
    {
        [Title("Testing Fields")]
        public Beatmap testBeatmap;
        public GameObject notePrefab;
        public int activeNoteLimit = 50;

        public Transform ringPivot;
        public int measuresPerRotation = 8;

        public int score = 0;

        [Title("Events")]
        public EventReference popSFXEvent;
        public EventReference missSFXEvent;
        public EventReference musicEvent;

        private NoteManager _noteManager;
        private Song _song;

        void Start()
        {
            if (_noteManager == null)
            {
                _noteManager = GetComponentInChildren<NoteManager>();
            }

            SetupSong(testBeatmap);
        }

        void Update()
        {

        }

        void FixedUpdate()
        {
            UpdateRing();
        }

        public void SetupSong(Beatmap beatmap)
        {
            _song = beatmap.DeserializeSongData();
            _noteManager.song = _song;
            _noteManager.Cleanup();
            _noteManager.InitializeNoteList(notePrefab, ringPivot, activeNoteLimit);
            _noteManager.InitializeMeasures();
        }

        void UpdateRing()
        {
            ringPivot.Rotate(new Vector3(0, (_song.bpm / 60f) * 360f / _song.beatsPerMeasure / measuresPerRotation * Time.fixedDeltaTime, 0));
        }
    }
}