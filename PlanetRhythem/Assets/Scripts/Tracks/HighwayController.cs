using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using Rhythem.Songs;
using Rhythem.Play;
using Rhythem.TrackEditor;
using Sirenix.OdinInspector;
using System.Collections;

namespace Rhythem.Tracks
{
    public class HighwayController : MonoBehaviour
    {
        public static HighwayController s;
        [Title("Testing Fields")]
        public Beatmap testBeatmap;
        public GameObject notePrefab;
        public int activeNoteLimit = 50;

        public Transform ringPivot;
        public int measuresPerRotation = 8;

        public float failTime = 1.5f;


        [Title("FMOD Events")]
        public EventReference popSFXEvent;
        public EventReference perfectSFXEvent;
        public EventReference missSFXEvent;

        public EventReference songFailSFXEvent;
        public EventReference songCompleteSFXEvent;
        public EventReference obstacleAsteroidSFXEvent;
        public EventReference obstacleIceSFXEvent;
        public EventReference musicEvent;


        private EventInstance activeSong;
        
        private IEnumerator _songStartAsync;

        private Player player;
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

        void Awake()
        {
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

            EventManager.Startup();
            player = GameManager.Instance.VRRig.GetComponent<Player>();
            player.OnNoteHit.AddListener(PlayNoteHitSound);
            _noteManager.OnNoteMissed.AddListener(PlayNoteMissedSound);
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

        public void PlayNoteHitSound(ScorableNote note, ScoreZone scoreZone)
        {
            if (scoreZone == ScoreZone.Miss)
            {
                _noteManager.OnNoteMissed.Invoke(note);
            }

            switch (note.noteType)
            {
                case NoteType.Note:
                    
                    Debug.Log(scoreZone.ToString());
                    PlayOneShot(perfectSFXEvent, note.transform.position);
                    break;
                case NoteType.Obstacle:
                    var meshname = note.currentMesh.name;
                    if (meshname.Contains("Ice"))
                    {
                        PlayOneShot(obstacleIceSFXEvent, note.transform.position);
                    }
                    else if (meshname.Contains("Asteroids"))
                    {
                        PlayOneShot(obstacleAsteroidSFXEvent, note.transform.position);
                    }
                    break;
                default:
                    break;
            }
        }

        public void PlayNoteMissedSound(ScorableNote note)
        {
            PlayOneShot(missSFXEvent, note.transform.position);
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