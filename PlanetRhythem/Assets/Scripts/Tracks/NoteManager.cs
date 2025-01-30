using Rhythem.Songs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

namespace Rhythem.Tracks
{
    /// <summary>
    /// this should probably just be in SongSession.cs but i cant fathom that right now so here we are fuckin'... faking it 'till we making it
    /// </summary>
    public class NoteManager : MonoBehaviour
    {
        public GameObject testMeasure;
        public GameObject testBeat;
        public GameObject testNote;
        public int currentMeasure = 0;
        public int currentBeat = 0;
        public int currentNote = 0;

        [HideInInspector] public Song song;
        private int _currentMeasure = 0;
        
        private float _noteTime;

        private List<Measure> _queuedMeasures;
        private List<ScorableNote> _activeNotes = new();
        private IEnumerator _noteSpawnAsync;
        private int _lastNoteSpawned = 0;
        private BoxCollider playSpaceSize;
        public NoteDeadzoneController deadzoneController;

        void Start()
        {
            playSpaceSize = GetComponent<BoxCollider>();
            
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

        void OnNoteHitByPlayer(ScorableNote note, ScoreZone zone)
        {

        }

        public void InitializeNoteList(GameObject notePrefab, Transform noteHighwayParent, Transform spawnLocation, int numberOfSafeNotes)
        {
            float animationSpeed = 1 / (song.bpm / 60f / (song.beatsPerMeasure));
            _noteTime = (song.bpm / 60f) / (song.beatsPerMeasure) / (song.subdivisionsPerBeat);

            for (int i = 0; i < numberOfSafeNotes; i++)
            {
                var newNoteGO = Instantiate(notePrefab, position: spawnLocation.position, rotation: spawnLocation.rotation);
                newNoteGO.name = $"ScorableNote_{i}";
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
                QueueNewMeasure(song.measures[i]);
            }
            _noteSpawnAsync = DoNoteSpawnPerMeasure(_queuedMeasures[0]);
            StartCoroutine(_noteSpawnAsync);
        }

        public void QueueNewMeasure(Measure measure)
        {
            if (_queuedMeasures.Count > 4)
            {
                _queuedMeasures.RemoveAt(0);
            }
            _queuedMeasures.Add(measure);
        }

        private IEnumerator DoNoteSpawnPerMeasure(Measure measure)
        {
            testMeasure.transform.DOScale(0.2f, _noteTime * song.beatsPerMeasure * song.subdivisionsPerBeat).OnComplete(() =>
            {
                testMeasure.transform.DOScale(1f, 0f);
            });
            for (int i = 0; i < measure.beats.Count; i++)
            {
                testBeat.transform.DOScale(0.2f, _noteTime * song.subdivisionsPerBeat).OnComplete(() =>
                {
                    testBeat.transform.DOScale(1f, 0f);
                });

                int currentNoteSubdiv = 0;
                for (int j = 0; j < song.subdivisionsPerBeat; j++)
                {
                    testNote.transform.DOScale(0.2f, _noteTime).OnComplete(() =>
                    {
                        testNote.transform.DOScale(1f, 0f);
                    });
                    try
                    {
                        foreach (var note in measure.beats[i].notes)
                        {
                            if (note.noteTime == currentNoteSubdiv)
                            {
                                if (_lastNoteSpawned >= _activeNotes.Count)
                                {
                                    _lastNoteSpawned = 0;
                                }
                                _activeNotes[_lastNoteSpawned].gameObject.SetActive(true);
                                var xoff = note.notePositionX * playSpaceSize.size.x - (playSpaceSize.size.x / 2f);
                                var yoff = note.notePositionY * playSpaceSize.size.y;
                                var noteOffset = new Vector3(0f, yoff, xoff);
                                _activeNotes[_lastNoteSpawned].notePosition = noteOffset;
                                _activeNotes[_lastNoteSpawned].gameObject.transform.position = transform.position + noteOffset;
                                _activeNotes[_lastNoteSpawned].gameObject.transform.rotation = transform.rotation;
                                _activeNotes[_lastNoteSpawned++].ResetNote(note, Time.time);
                                break;
                            }
                        }
                    }
                    catch
                    {
                    }
                    currentNoteSubdiv++;
                    currentNote = currentNoteSubdiv;
                    yield return new WaitForSeconds(_noteTime);
                }
                currentBeat++;
                currentNote = 0;
            }
            if (++_currentMeasure < song.measures.Count)
            {
                _noteSpawnAsync = DoNoteSpawnPerMeasure(song.measures[_currentMeasure]);
                currentMeasure = _currentMeasure;
                currentBeat = 0;
                StartCoroutine(_noteSpawnAsync);
            }
        }
    }
}