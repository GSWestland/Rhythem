using Rhythem.Songs;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Rhythem.Play
{
    public class Player : MonoBehaviour
    {
        [Title("Input Actions")]
        InputAction confirm;
        InputAction back;
        InputAction navigate;
        InputAction pause;

        [Title("Events")]
        public UnityEvent<ScorableNote, ScoreZone> OnNoteHit;
        //public UnityEvent OnUiItemSelected;

        [Title("Player-Related Members")]
        public int energy = 100;
        public int score = 0;

        void Start()
        {
            confirm = InputSystem.actions.FindAction("Player/Interact");
            back = InputSystem.actions.FindAction("Player/Back");
            navigate = InputSystem.actions.FindAction("Player/Navigate");
            pause = InputSystem.actions.FindAction("Player/Pause");

        }

        void Update()
        {

        }

        public void DoSongStartPlayerSetup()
        {
            energy = Globals.STARTING_ENERGY;
            score = 0;
        }

        public void DoHitNoteAction(ScorableNote note, ScoreZone zone)
        {

        }

        public void DoMissedNoteAction(ScorableNote note)
        {
            energy -= 10;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Note")
            {
                var note = other.GetComponent<ScorableNote>();
                if (note != null)
                {
                    var hitTimeDelta = Math.Abs(Time.time - note.targetHitTime);
                    var scoreZone = new ScoreZone();
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
                    else
                    {
                        scoreZone = ScoreZone.Miss;
                    }

                    OnNoteHit.Invoke(note, scoreZone);
                }
            }
        }
    }
}