using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using Rhythem.Songs;
using Rhythem.TrackEditor;
using Sirenix.OdinInspector;
using System.Collections;

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


        [Title("FMOD Events")]
        public EventReference popSFXEvent;
        public EventReference perfectSFXEvent;
        public EventReference missSFXEvent;

        public EventReference songFailSFXEvent;
        public EventReference songCompleteSFXEvent;
        public EventReference obstacleAsteroidSFXEvent;
        public EventReference obstacleIceSFXEvent;
        public EventReference musicEvent;

        public float failTime = 1.5f;

        private EventInstance activeSong;
        
        private IEnumerator _songStartAsync;

        private NoteManager _noteManager;
        private Song _song;
        private AudioSource _audioSource;
        public AudioSource audioSource
        {
            get
            {
                if(_audioSource == null && (_audioSource = GetComponent<AudioSource>()) == null)
                {
                    _audioSource = gameObject.AddComponent<AudioSource>();
                }
                return _audioSource;
            }
        }

        void Start()
        {
            if (_noteManager == null)
            {
                _noteManager = GetComponentInChildren<NoteManager>();
            }

            SetupSong(testBeatmap);

            EventManager.Startup();
        }

        void Update()
        {

        }

        void FixedUpdate()
        {
            UpdateRing();
        }

        public void PlayOneShot(EventReference sound, Vector3 worldPos)
        {
            RuntimeManager.PlayOneShot(sound, worldPos);
        }

        public void StartSong(EventReference song)
        {
            activeSong = RuntimeManager.CreateInstance(song);
            RuntimeManager.AttachInstanceToGameObject(activeSong, Camera.main.transform);

        }

        public IEnumerator SongWin()
        {
            PlayOneShot(songCompleteSFXEvent, Camera.main.transform.position);
            //YOU WIN MENU
            yield return null;
        }

        public IEnumerator SongFail()
        {
            activeSong.setParameterByName("Song failed", 1f);
            PlayOneShot(songFailSFXEvent, Camera.main.transform.position);
            yield return new WaitForSeconds(failTime);
            //YOU FAILED MENU
        }

        public void SetupSong(Beatmap beatmap)
        {
            _song = beatmap.DeserializeSongData();
            _noteManager.song = _song;
            _noteManager.Cleanup();
            _noteManager.InitializeNoteList(notePrefab, ringPivot, activeNoteLimit);
            _noteManager.InitializeMeasures();
            _songStartAsync = DoSongStartWithDelay(testBeatmap.audioFile);
            StartCoroutine(_songStartAsync);
        }

        void UpdateRing()
        {
            ringPivot.Rotate(new Vector3(0, (_song.bpm / 60f) * 360f / _song.beatsPerMeasure / measuresPerRotation * Time.fixedDeltaTime, 0));
        }

        IEnumerator DoSongStartWithDelay(AudioClip songFile)
        {
            if (songFile == null)
            {
                yield return null;
            }
            
            //UNITY AUDIO VERSION
            /*
            audioSource.playOnAwake = false;
            audioSource.clip = songFile;
            yield return new WaitForSeconds(_song.bpm / 60 * (measuresPerRotation / 4));
            audioSource.Play();
            */

            //FMOD VERSION
            yield return new WaitForSeconds(_song.bpm / 60 * (measuresPerRotation / 4));
            StartSong(musicEvent);

        }
    }
}