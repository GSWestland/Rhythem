using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Rhythem.Songs {
    public class ScorableNote : MonoBehaviour
    {
        public NoteType noteType;
        public DesiredHand noteHand;
        public Vector2 notePosition;
        public float measureTime = 0f;

        [Title("Assign Me :3")]
        public Color leftHandColor;
        public Color rightHandColor;
        public MeshRenderer noteMesh;
        public List<MeshRenderer> obstacleMeshOptions;

        private MeshRenderer _currentMesh;
        private MeshRenderer _nextMesh;
        private Collider _col;
        private ScoreZone _scoreZone;
        private float _targetHitTime = 0f;

        private void Awake()
        {
            _col = GetComponent<Collider>();
            noteMesh.enabled = false;
            foreach (var m in obstacleMeshOptions)
            {
                m.enabled = false;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "NoteDeadzone")
            {
                if (_currentMesh != null)
                {
                    _currentMesh.enabled = false;
                    _col.enabled = false;
                }
                if (noteType == NoteType.Note)
                {
                    Debug.Log("MISSED NOTE");
                }
            }
            else if (other.tag == "playerHand")
            {
                if (noteType == NoteType.Note)
                {

                    //get controller hand and compare against this note's hand
                }
                else if (noteType == NoteType.Obstacle)
                {
                    //womp womp
                }
            }
        }

        public void ResetNote(Note noteData, float currentTime, Vector2 playSpaceSize)
        {
            _col.enabled = true;
            _targetHitTime = Time.time + (2 * measureTime); // hard coded to be 1/4 of the way around the ring
            noteType = noteData.noteType;
            if (noteType == NoteType.Note)
            {
                noteHand = noteData.hand;
                noteMesh.material.color = noteHand == DesiredHand.Left ? leftHandColor : rightHandColor;
                _nextMesh = noteMesh;
                
            }
            else if (noteType == NoteType.Obstacle && obstacleMeshOptions.Count > 0)
            {
                _nextMesh = obstacleMeshOptions[new System.Random().Next(0, obstacleMeshOptions.Count)];
            }
            if (_nextMesh != null)
            {
                if (_currentMesh != null)
                {
                    _currentMesh.enabled = false;
                }
                _nextMesh.enabled = true;
                _currentMesh = _nextMesh;
                _nextMesh = null;
            }
            notePosition.x = (noteData.notePositionX - playSpaceSize.x * 0.5f) * playSpaceSize.x;
            notePosition.y = noteData.notePositionY * playSpaceSize.y;

            Vector3 offsetPosition = transform.position;
            offsetPosition.x += notePosition.x;
            offsetPosition.y += notePosition.y;
            transform.position = offsetPosition;
            transform.parent = transform;
        }
    }
}