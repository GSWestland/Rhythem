using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using FMODUnity;

namespace Rhythem.Songs {
    public class ScorableNote : MonoBehaviour
    {
        public NoteType noteType;
        public DesiredHand noteHand;
        public Vector2 notePosition;

        [Title("Assign Me :3")]
        public Color leftHandColor;
        public Color rightHandColor;

        public Renderer noteMesh;
        public List<Renderer> obstacleMeshOptions;
        [HideInInspector] public Renderer currentMesh;
        private Renderer _nextMesh;
        private Material _starMaterial;
        
        public float measureTime = 0f;
        private Collider _col;
        public float targetHitTime = 0f;

        public float animSpeed = 1f;
        private Animator _animator;
        public Animator animator
        {
            get
            {
                if (_animator == null && (_animator = GetComponent<Animator>()) == null)
                {
                    Debug.LogError($"No animator available on ScorableNote{gameObject.GetInstanceID()}");
                    return null;
                }
                return _animator;
            }
        }

        private void Awake()
        {

            for (int i = 0; i <  noteMesh.sharedMaterials.Length; i++)
            {
                if (noteMesh.sharedMaterials[i].name == "m_StarBubble")
                {
                    _starMaterial = new Material(noteMesh.sharedMaterials[i]);
                    noteMesh.sharedMaterial = _starMaterial;
                    break;
                }
            }
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
                if (currentMesh != null)
                {
                    currentMesh.enabled = false;
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
            animator.SetFloat("playSpeed", animSpeed);
            _col.enabled = true;
            targetHitTime = Time.time + (2 * measureTime); // hard coded to be 1/4 of the way around the ring
            noteType = noteData.noteType;
            if (noteType == NoteType.Note)
            {
                noteHand = noteData.hand;
                _starMaterial.SetColor("_Star_Color", noteHand == DesiredHand.Left ? leftHandColor : rightHandColor);
                _nextMesh = noteMesh;
                
            }
            else if (noteType == NoteType.Obstacle && obstacleMeshOptions.Count > 0)
            {
                _nextMesh = obstacleMeshOptions[new System.Random().Next(0, obstacleMeshOptions.Count)];
            }
            if (_nextMesh != null)
            {
                if (currentMesh != null)
                {
                    currentMesh.enabled = false;
                }
                _nextMesh.enabled = true;
                currentMesh = _nextMesh;
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