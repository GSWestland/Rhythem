using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Rhythem.Songs;
using Sirenix.OdinInspector;

namespace Rhythem.Play
{
    public class PlayerWand : MonoBehaviour
    {

        public DesiredHand desiredHand;
        public Transform lineRenderStartTransform;
        public ParticleSystem galaxyTrail;
        public bool inMenuMode = true;

        private Color _handColor;


        private LineRenderer _selectionAssistant;
        public UnityEvent<DesiredHand, ScorableNote, ScoreZone> OnNoteHit;

        void Start()
        {
            _selectionAssistant = lineRenderStartTransform.GetComponent<LineRenderer>();
            _handColor = desiredHand == DesiredHand.Left ? GameManager.Instance.player.leftHandColor : GameManager.Instance.player.rightHandColor;
            var gTrail = galaxyTrail.trails;
            gTrail.colorOverLifetime = _handColor;
        }

        void Update()
        {
            if (inMenuMode)
            {
                List<Vector3> linePts = new();
                linePts.Add(lineRenderStartTransform.position);
                linePts.Add(lineRenderStartTransform.position + (lineRenderStartTransform.up * 4f));
                _selectionAssistant.SetPositions(linePts.ToArray());
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Note")
            {
                var note = other.GetComponent<ScorableNote>();
                if (note != null)
                {
                    var scoreZone = ScoreZone.Miss;
                    if (note.noteHand == desiredHand)
                    {
                        var hitTimeDelta = Math.Abs(Time.time - note.targetHitTime);
                        if (hitTimeDelta < 0.02f)
                        {
                            scoreZone = ScoreZone.Stellar;
                        }
                        else if (hitTimeDelta < 0.05f)
                        {
                            scoreZone = ScoreZone.Great;
                        }
                        else if (hitTimeDelta < 0.085f)
                        {
                            scoreZone = ScoreZone.Good;
                        }
                        else if (hitTimeDelta < 0.11f)
                        {
                            scoreZone = ScoreZone.Close;
                        }
                    }
                    OnNoteHit.Invoke(desiredHand, other.gameObject.GetComponent<ScorableNote>(), scoreZone);
                    AudioManager.Instance.PlayNoteHitOneShot(note);
                }
            }
        }
    }
}