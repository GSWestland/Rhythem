using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Rhythem.Songs {
    public class ScorableNote : MonoBehaviour
    {
        public NoteType noteType;
        public DesiredHand noteHand;
        public Vector2 notePosition;

        [Title("Assign Me :3")]
        public Color leftHandColor;
        public Color rightHandColor;
        public MeshRenderer noteMesh;
        public List<MeshRenderer> obstacleMeshOptions;

        private MeshRenderer _currentMesh;
        private MeshRenderer _nextMesh;

        private void Awake()
        {
            noteMesh.enabled = false;
            foreach (var m in obstacleMeshOptions)
            {
                m.enabled = false;
            }
        }

        void Update()
        {

        }

        private void OnCollisionEnter(Collision collision)
        {
            //do some animation or something?
            if (collision.collider.tag == "NoteDeadzone")
            {
                Debug.Log("NOTE PASSED");
                gameObject.SetActive(false);
            }
        }

        public void ResetNote(Note noteData, Vector2 playSpaceSize)
        {
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
            var playSpaceXMod = playSpaceSize.x * 0.5f;
            notePosition.x = noteData.notePositionX * playSpaceXMod;
            notePosition.y = noteData.notePositionY * playSpaceSize.y;

            Vector3 offsetPosition = transform.position;
            offsetPosition.x += notePosition.x;
            offsetPosition.y += notePosition.y;
            transform.position = offsetPosition;
            transform.parent = transform;
        }
    }
}