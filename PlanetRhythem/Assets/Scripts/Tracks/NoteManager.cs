using Rhythem.Songs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Rhythem.Tracks
{
    /// <summary>
    /// this should probably just be in SongSession.cs but i cant fathom that right now so here we are fuckin'... faking it 'till we making it
    /// </summary>
    public class NoteManager : MonoBehaviour
    {
        [HideInInspector] public Song song;
        private int _currentMeasure = 0;
        
        private float _noteTime;

        private List<Measure> _queuedMeasures;
        private List<ScorableNote> _activeNotes = new();
        private IEnumerator _noteSpawnAsync;
        private int _lastNoteSpawned = 0;
        private Vector2 playSpaceSize = new();

        [HideInInspector] public UnityEvent<ScorableNote> OnNoteMissed;

        void Start()
        {
            playSpaceSize = GetComponent<BoxCollider>().size;
        }

        void Update()
        {

        }

        private void OnDisable()
        {
            Cleanup();
        }

        public void Cleanup()
        {
            _queuedMeasures?.Clear();
            _activeNotes?.Clear();
        }

        public void InitializeNoteList(GameObject notePrefab, Transform noteHighwayParent, int numberOfSafeNotes)
        {
            float animationSpeed = 1 / (song.bpm / 60f / (song.beatsPerMeasure - 1));
            _noteTime = (song.bpm / 60f) / (song.beatsPerMeasure - 1) / (song.subdivisionsPerBeat - 1);

            for (int i = 0; i < numberOfSafeNotes; i++)
            {
                var newNoteGO = Instantiate(notePrefab);
                var sn = newNoteGO.GetComponent<ScorableNote>();
                sn.measureTime = song.bpm / 60f;
                sn.animSpeed = animationSpeed;
                newNoteGO.transform.position = transform.position;
                newNoteGO.transform.rotation = transform.rotation;
                newNoteGO.transform.SetParent(noteHighwayParent, true);
                _activeNotes.Add(sn);
            }
        }

        public void InitializeMeasures()
        {
            _queuedMeasures = new();
            for (int i = 0; i < 4; i++)
            {
                _queuedMeasures.Add(song.measures[i]);
            }
            _noteSpawnAsync = DoNoteSpawnPerMeasure(_queuedMeasures[0]);
            StartCoroutine(_noteSpawnAsync);
        }

        public void QueueNewMeasure(Measure measure)
        {
            if (_queuedMeasures.Count < 1)
            {
                return;
            }
            _queuedMeasures.RemoveAt(0);
            _queuedMeasures.Add(measure);
        }

        private IEnumerator DoNoteSpawnPerMeasure(Measure measure)
        {
            //Debug.Log($"Current Measure: {_currentMeasure}");
            for (int i = 0; i < measure.beats.Count; i++)
            {
                int currentNoteSubdiv = 0;
                for (int j = 0; j < song.subdivisionsPerBeat; j++)
                {
                    try
                    {
                        foreach (var note in measure.beats[i].notes)
                        {
                            if (note.noteTime == currentNoteSubdiv++)
                            {
                                if (_lastNoteSpawned >= _activeNotes.Count)
                                {
                                    _lastNoteSpawned = 0;
                                }
                                _activeNotes[_lastNoteSpawned++].gameObject.SetActive(true);
                                _activeNotes[_lastNoteSpawned].gameObject.transform.position = transform.position;
                                _activeNotes[_lastNoteSpawned].gameObject.transform.rotation = transform.rotation;
                                _activeNotes[_lastNoteSpawned].ResetNote(note, Time.time, playSpaceSize);
                                break;
                            }
                        }
                    }
                    catch
                    {
                    }
                    yield return new WaitForSeconds(_noteTime);
                }
            }
            if (++_currentMeasure < song.measures.Count)
            {
                _noteSpawnAsync = DoNoteSpawnPerMeasure(song.measures[_currentMeasure]);
                StartCoroutine(_noteSpawnAsync);
            }
        }
    }
}