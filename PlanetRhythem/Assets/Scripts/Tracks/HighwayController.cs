using Sirenix.OdinInspector;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using Rhythem.Songs;
using Rhythem.Play;
using Rhythem.TrackEditor;
using Rhythem.Util;
using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace Rhythem.Tracks
{
    public class HighwayController : MonoBehaviour
    {
        public static HighwayController s;
        [Title("Testing Fields")]
        public Beatmap testBeatmap;

        [Title("Notes Setup")]
        public GameObject notePrefab;
        public int activeNoteLimit = 50;
        public int measuresPerRotation = 8;
        public float failTime = 1.5f;

        public Transform ringPivot;
        public Transform notesSpawnStart;



        [Title("FMOD Events")]

        private FMOD.Studio.EVENT_CALLBACK songCallback;
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

            //FMOD Setup

            songCallback = new FMOD.Studio.EVENT_CALLBACK(PlayFileCallBackUsingAudioFile);

            EventManager.Startup();
            player = GameManager.Instance.VRRig.GetComponent<Player>();
            player.OnNoteHit.AddListener(DoNoteHit);
            _noteManager.deadzoneController.OnNoteMissed.AddListener(DoNoteMissed);
            _noteManager.deadzoneController.OnNoteMissed.AddListener(player.DoMissedNoteAction);
        }

        public void PlayClipInFmod(AudioClip audioclip)
            {
            //UnityEngine.Debug.Log("PlayClipinFmod " + audioclip.name);
            // Get the sample data from the clip and assign in to an array 
            float[] audioclip_data = new float[audioclip.samples * audioclip.channels];
            audioclip.GetData(audioclip_data, 0);

            // We no longer require the 'key' parameter as it was used to access the FMOD audio table. 
            // string key = audioclip.name; 

            // We are getting the information out of the Unity Audio clip passed into the function 
            SoundRequirements sound_requirements = new SoundRequirements(
                // The name of the clip we are playing, makes it easier to identify when in code  
                audioclip.name,
                // Parameters required to create sound exit info: https://fmod.com/docs/2.02/api/core-api-system.html#fmod_createsoundexinfo
                audioclip.samples, 
                audioclip.channels, 
                FMOD.SOUND_FORMAT.PCMFLOAT, 
                audioclip.frequency, 
                // The sample data that will be copied into the sound when we create it 
                audioclip_data);
            //

            var audioClipInstance = FMODUnity.RuntimeManager.CreateInstance(musicEvent);

            // Instead of passing in the key we will pass in the sound requirements that we have created. 
            GCHandle stringHandle = GCHandle.Alloc(sound_requirements);
            audioClipInstance.setUserData(GCHandle.ToIntPtr(stringHandle));

            // The callback to make the create the sound and assign it to the instance 
            audioClipInstance.setCallback(songCallback);
                
            // Play the sound 
            audioClipInstance.start();

            // Release the memory, however if you would like to access parameters or other functions of the instane, 
            // you don't have to release it now. 
            audioClipInstance.release();
            }

            [AOT.MonoPInvokeCallback(typeof(FMOD.Studio.EVENT_CALLBACK))]
        static FMOD.RESULT PlayFileCallBackUsingAudioFile(FMOD.Studio.EVENT_CALLBACK_TYPE type, IntPtr instPrt, IntPtr paramsPrt)
        {
#if UNITY_EDITOR
            Debug.Log("PlayFileCallBackUsingAudioFile event type " + type.ToString());
#endif
            FMOD.Studio.EventInstance inst = new FMOD.Studio.EventInstance(instPrt);

            if (!inst.isValid())
            {
                return FMOD.RESULT.ERR_EVENT_NOTFOUND;
            }

            // Retrieving the user data from the instance
            inst.getUserData(out IntPtr clipDataPtr);
            GCHandle clipHandle = GCHandle.FromIntPtr(clipDataPtr);
            // Assinging the our data to a new struct so we can access all the information 
            SoundRequirements clip = clipHandle.Target as SoundRequirements;

            // Depending on the callback type will decide what happens next 
            switch (type)
            {
                case FMOD.Studio.EVENT_CALLBACK_TYPE.CREATE_PROGRAMMER_SOUND:
                    {
                        // This is what we will use to pass the sound back out to our instance 
                        var param = (FMOD.Studio.PROGRAMMER_SOUND_PROPERTIES)Marshal.PtrToStructure(paramsPrt, typeof(FMOD.Studio.PROGRAMMER_SOUND_PROPERTIES));

                        // Retrieve the masterGroup, or the channel group you wish to play the clip too 
                        ERRCHECK(FMODUnity.RuntimeManager.CoreSystem.getMasterChannelGroup(out FMOD.ChannelGroup masterGroup), "Failed to get masterGroup from core system");

                        // Calculating the length of the audio clip by the samples and channels 
                        uint lenBytes = (uint)(clip.samples * clip.channels * sizeof(float));

                        // Sound exit info to be used when creating the sound 
                        FMOD.CREATESOUNDEXINFO soundInfo = new FMOD.CREATESOUNDEXINFO();
                        soundInfo.cbsize = Marshal.SizeOf(typeof(FMOD.CREATESOUNDEXINFO));
                        soundInfo.length = lenBytes;
                        soundInfo.format = FMOD.SOUND_FORMAT.PCMFLOAT;
                        soundInfo.defaultfrequency = clip.defaultFrequency;
                        soundInfo.numchannels = clip.channels;

                        // Creating the sound using soundInfo
                        FMOD.RESULT result = ERRCHECK(FMODUnity.RuntimeManager.CoreSystem.createSound(clip.name, FMOD.MODE.OPENUSER, ref soundInfo, out FMOD.Sound sound), "Failed to create sound");
                        if (result != FMOD.RESULT.OK)
                            return result;

                        // Now we have created our sound, we need to give it the sample data from the audio clip 
                        result = ERRCHECK(sound.@lock(0, lenBytes, out IntPtr ptr1, out IntPtr ptr2, out uint len1, out uint len2), "Failed to lock sound");
                        if (result != FMOD.RESULT.OK)
                            return result;

                        Marshal.Copy(clip.sampleData, 0, ptr1, (int)(len1 / sizeof(float)));
                        if (len2 > 0)
                            Marshal.Copy(clip.sampleData, (int)(len1 / sizeof(float)), ptr2, (int)(len2 / sizeof(float)));

                        result = ERRCHECK(sound.unlock(ptr1, ptr2, len1, len2), "Failed to unlock sound");
                        if (result != FMOD.RESULT.OK)
                            return result;

                        ERRCHECK(sound.setMode(FMOD.MODE.DEFAULT), "Failed to set the sound mode");


                        if (result == FMOD.RESULT.OK)
                        {
                            param.sound = sound.handle;
                            param.subsoundIndex = -1;
                            // Passing the sound back out again to be played 
                            Marshal.StructureToPtr(param, paramsPrt, false);
                        }
                        else
                            return result;
                    }
                    break;
                case FMOD.Studio.EVENT_CALLBACK_TYPE.DESTROY_PROGRAMMER_SOUND:
                    {
                        var param = (FMOD.Studio.PROGRAMMER_SOUND_PROPERTIES)Marshal.PtrToStructure(paramsPrt, typeof(FMOD.Studio.PROGRAMMER_SOUND_PROPERTIES));
                        var sound = new FMOD.Sound(param.sound);
                        FMOD.RESULT result = ERRCHECK(sound.release(), "Failed to release sound");
                        if (result != FMOD.RESULT.OK)
                            return result;
                    }
                    break;
                case FMOD.Studio.EVENT_CALLBACK_TYPE.DESTROYED:
                    clipHandle.Free();
                    break;
            }
            return FMOD.RESULT.OK;
        }
    	private static FMOD.RESULT ERRCHECK(FMOD.RESULT result, string failMsg)
        {
#if UNITY_EDITOR
            if (result != FMOD.RESULT.OK)
            {
                Debug.Log(failMsg + " with result: " + result);
                return result;
            }
#endif
            return result;
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

        public void StartSong(AudioClip songFile)
        {
            PlayClipInFmod(songFile);
        }

        public void DoNoteHit(ScorableNote note, ScoreZone scoreZone)
        {
            if (scoreZone == ScoreZone.Miss)
            {
                _noteManager.deadzoneController.OnNoteMissed.Invoke(note);
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

        public void DoNoteMissed(ScorableNote note)
        {
            PlayOneShot(missSFXEvent, note.transform.position);
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
            _noteManager.InitializeNoteList(notePrefab, ringPivot, notesSpawnStart, activeNoteLimit);
            _noteManager.InitializeMeasures();
            _songStartAsync = DoSongStartWithDelay(testBeatmap.audioFile);
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
            
            //UNITY AUDIO VERSION
            /*
            audioSource.playOnAwake = false;
            audioSource.clip = songFile;
            yield return new WaitForSeconds(_song.bpm / 60 * (measuresPerRotation / 4));
            audioSource.Play();
            */

            //FMOD VERSION
            yield return new WaitForSeconds(_song.startWaitTime);
            yield return new WaitForSeconds(_song.bpm / 60 * (measuresPerRotation / 4));
            StartSong(songFile);

        }
    }
}

