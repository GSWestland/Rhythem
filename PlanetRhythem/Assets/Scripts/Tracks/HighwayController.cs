using Sirenix.OdinInspector;
using UnityEngine;
using FMODUnity;
using Rhythem.Core;
using Rhythem.Songs;
using Rhythem.Play;
using Rhythem.TrackEditor;
using System.Collections;

namespace Rhythem.Tracks
{
    public class HighwayController : MonoBehaviour
    {
        public static HighwayController s;
        [Title("Testing Fields")]
        public Beatmap testBeatmap;
        [SerializeField] private float timePerSong = 0f;
        [SerializeField] private float timePerMeasure = 0f;
        [SerializeField] private float timePerBeat = 0f;
        [SerializeField] private float timePerNote = 0f;

        [Title("Notes Setup")]
        public GameObject notePrefab;
        public int activeNoteLimit = 50;
        public int measuresPerRotation = 8;
        public float failTime = 1.5f;
        public Transform ringPivot;
        public Transform notesSpawnStart;


        //public SongSession songSession;
        private IEnumerator _songStartAsync;

        private Player player;
        private NoteManager _noteManager;
        private AudioManager audioManager;
        private Song _song;


        void Awake()
        {
            audioManager = AudioManager.Instance;
            //songSession = SessionsManager.Instance.GetCurrentSession<SongSession>();

            if(s != null)
            {
                Debug.LogError("Error: More than 1 Highway Controller in scene");
                return;
            }
            s = this;
        }

        void Start()
        {
            if (_noteManager == null)
            {
                _noteManager = GetComponentInChildren<NoteManager>();
            }
            SetupSong(testBeatmap);

            timePerSong = testBeatmap.audioFile.length - testBeatmap.silenceAtStartOfTrack;
            timePerMeasure = timePerSong / testBeatmap.bPM / 60f;
            timePerBeat = timePerMeasure / testBeatmap.beatsPerMeasure;
            timePerNote = timePerBeat / testBeatmap.subdivisionsPerBeat;

            player = GameManager.Instance.player;

            SubscribeToSongSessionActions();
            
        }
        public void SubscribeToSongSessionActions()
        {
            player.leftHand.OnNoteHit.AddListener(DoNoteHit);
            player.rightHand.OnNoteHit.AddListener(DoNoteHit);
            _noteManager.deadzoneController.OnNoteMissed.AddListener(player.OnMissedNoteAction);
        }

        public void UnsubscribeToSongSessionActions()
        {
            player.leftHand.OnNoteHit.RemoveListener(DoNoteHit);
            player.rightHand.OnNoteHit.RemoveListener(DoNoteHit);
            _noteManager.deadzoneController.OnNoteMissed.RemoveListener(player.OnMissedNoteAction);

        }

        private void OnDisable()
        {
            UnsubscribeToSongSessionActions();
        }

        private void OnDestroy()
        {
            UnsubscribeToSongSessionActions();
        }

        void Update()
        {

        }

        void FixedUpdate()
        {
            UpdateRing();
        }

        public void DoNoteHit(DesiredHand hand, ScorableNote note, ScoreZone scoreZone)
        {
            Debug.Log(scoreZone.ToString());

            if (scoreZone == ScoreZone.Miss)
            {
                _noteManager.deadzoneController.OnNoteMissed.Invoke(note);
            }
        }

        public IEnumerator SongWin()
        {
            audioManager.PlayOneShot(audioManager.songCompleteSFXEvent, Camera.main.transform.position);
            //YOU WIN MENU
            yield return null;
        }

        public IEnumerator SongFail()
        {
            audioManager.activeSong.setParameterByName("Song failed", 1f);
            audioManager.PlayOneShot(audioManager.songFailSFXEvent, Camera.main.transform.position);
            yield return new WaitForSeconds(failTime);
            //YOU FAILED MENU
        }

        public void SetupSong(Beatmap beatmap)
        {
            _song = beatmap.DeserializeSongData();
            _noteManager.song = _song;
            _noteManager.Cleanup();
            _noteManager.InitializeNoteList(notePrefab, ringPivot, notesSpawnStart, activeNoteLimit);
            _songStartAsync = DoSongStartWithDelay(testBeatmap.audioFile);
            //Debug.Log("Trying to start async function...");
            StartCoroutine(_songStartAsync);
        }

        void UpdateRing()
        {
            if (ringPivot == null && (ringPivot = GameObject.Find("Ring").transform) == null)
            {
                return;
            }
            ringPivot.Rotate(new Vector3(0, (_song.bpm / 60f) * 360f / _song.beatsPerMeasure / measuresPerRotation * Time.fixedDeltaTime, 0));
        }

        IEnumerator DoSongStartWithDelay(AudioClip songFile)
        {
            if (songFile == null)
            {
                Debug.LogError("NO SONG TO PLAY");
                yield return null;
            }

            //FMOD VERSION
            Debug.Log("Waiting to start...");
            yield return new WaitForSeconds(_song.startWaitTime); //this doesn't belong here, we need to pass the NoteManager the song.startWaitTime in a new coroutine that handles ALL measures' note spawning so we can delay that process by the appropriate amount, instead of handling each measure as a coroutine
            _noteManager.InitializeMeasures();
            var waitTime = _song.bpm / 60f * (measuresPerRotation / 4f);
            yield return new WaitForSeconds(waitTime);
            Debug.Log("attempting to start song...");
            audioManager.StartSong(songFile);

        }
    }
}

