using Rhythem.Songs;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SpatialTracking;

namespace Rhythem.Play
{
    public class Player : MonoBehaviour
    {
        [Title("Player-Specific References")]
        public TrackedPoseDriver head;
        public PlayerWand leftHand;
        public PlayerWand rightHand;
        private InputModule _inputModule;
        public InputModule inputModule
        {
            get
            {
                if ( _inputModule == null && (_inputModule = GetComponent<InputModule>()) == null)
                {
                    Debug.LogError("No InputModule found on player. fix that.");
                    return null;
                }
                return _inputModule;
            }
        }

        [Title("Player-Related Members")]
        public int energy = 100;
        public int score = 0;


        void Start()
        {
            leftHand.OnNoteHit.AddListener(OnHitNoteAction);
            rightHand.OnNoteHit.AddListener(OnHitNoteAction);
        }

        void Update()
        {

        }

        public void DoSongStartPlayerSetup()
        {
            energy = Globals.STARTING_ENERGY;
            score = 0;
        }

        public void OnHitNoteAction(ScorableNote note, ScoreZone zone)
        {
            if(note.noteType == NoteType.Obstacle)
            {
                energy -= 15;
                score -= 50;
            }
            else if (note.noteType == NoteType.Note)
            {
                energy += 3;
                switch (zone)
                {
                    case ScoreZone.Stellar:
                        score += 250;
                        break;
                    case ScoreZone.Great:
                        score += 150;
                        break;
                    case ScoreZone.Good:
                        score += 100;
                        break;
                    case ScoreZone.Close:
                        score += 10;
                        break;
                    case ScoreZone.Miss:
                        score -= 10;
                        break;
                    default:
                        break;
                }
            }
        }

        public void OnMissedNoteAction(ScorableNote note)
        {
            energy -= 10;
        }
    }
}