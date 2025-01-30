using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using DG.Tweening;

namespace Rhythem.Songs {
    public class ScorableNote : MonoBehaviour
    {
        public NoteType noteType;
        public DesiredHand noteHand;
        public Vector2 notePosition;

        [Title("Assign Me :3")]
        public Color leftHandColor;
        public Color rightHandColor;
        public Vector3 obstacleRotationSpeed;

        public Renderer noteMesh;
        public List<Renderer> obstacleMeshOptions;
        [HideInInspector] public Renderer currentMesh;
        private Renderer _nextMesh;
        private Material _starMaterial;
        
        public float measureTime = 0f;
        private Collider _col;
        public float targetHitTime = 0f;

        public Animator animator;
        public float animSpeed = 1f;
        private IEnumerator DoStarSpinOnSpawn;


        private void Awake()
        {
            SpawnNote();
            

        }

        public void Update()
        {
            if (noteType == NoteType.Obstacle)
            {
                transform.Rotate(obstacleRotationSpeed);
            }
        }

        public void OnNoteHit()
        {
            if (currentMesh != null)
            {
                currentMesh.enabled = false;
                _col.enabled = false;
            }
        }

        private void SpawnNote()
        {
            for (int i = 0; i < noteMesh.sharedMaterials.Length; i++)
            {
                if (noteMesh.sharedMaterials[i].name == "m_StarBubble")
                {
                    _starMaterial = new Material(noteMesh.sharedMaterials[i]);
                    noteMesh.sharedMaterial = _starMaterial;
                    break;
                }
            }
            _col ??= GetComponent<Collider>();
            _col.enabled = false;
            animator.SetFloat("playSpeed", animSpeed);
            noteMesh.enabled = false;
            foreach (var m in obstacleMeshOptions)
            {
                m.enabled = false;
            }
        }

        public void ResetNote(Note noteData, float currentTime)
        {
            noteType = noteData.noteType;
            if (noteType == NoteType.Note)
            {
                noteHand = noteData.hand;
                _starMaterial.SetColor("_Star_Color", noteHand == DesiredHand.Left ? leftHandColor : rightHandColor);
                _nextMesh = noteMesh;
                transform.DOScale(0.35f, measureTime - 0.4f).OnComplete(()=>
                {
                    animator.SetTrigger("DoSpin");
                    transform.DOScale(1f, 0.5f).SetEase(Ease.OutBounce);
                });
            }
            else if (noteType == NoteType.Obstacle && obstacleMeshOptions.Count > 0)
            {
                obstacleRotationSpeed = new Vector3(Random.Range(-0.5f, 0.6f), Random.Range(-0.5f, 0.6f), Random.Range(-0.5f, 0.6f));
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

            transform.eulerAngles = new Vector3(0f, 90f, 0f);

            _col.enabled = true;
            targetHitTime = Time.time + (2f * measureTime); // hard coded to be 1/4 of the way around the ring
            animator.SetFloat("playSpeed", animSpeed);
            
        }

        private IEnumerator DoNoteSpinOnDelay(float delay)
        {
            if (noteMesh == null || animator == null)
            {
                yield return null;
            }
            yield return new WaitForSeconds(delay);
            animator.SetTrigger("DoSpin");
        }
    }
}