using Rhythem.Play;
using Rhythem.Songs;
using Rhythem.TrackEditor;
using Rhythem.Tracks;
using UnityEngine;
using UnityEngine.Events;

namespace Rhythem
{
    public class SongSession : Session
    {
        public Beatmap beatmap;
        public Song song;

        public int score { get; private set; }
        public int energy { get; private set; }

        public int notesStellar;
        public int notesGreat;
        public int notesGood;
        public int notesClose;
        public int notesMiss;

        public UnityEvent OnSongFailed;

        public override void Initialize()
        {
            base.Initialize();
            beatmap = GameManager.Instance.CurrentBeatmap;
            GameManager.Instance.player.SetInputModule<PlayerSongPlayInputModule>();
            song = beatmap.DeserializeSongData();
            score = 0;
            energy = GameManager.Instance.scoreProfile.energyStartValue;
            notesStellar = 0;
            notesGreat = 0;
            notesGood = 0;
            notesClose = 0;
            notesMiss = 0;
        }

        public override void EndSession()
        {
            base.EndSession();
            //store score in sme persistent data
        }

        public void AddToScore(int points)
        {
            score += points;
        }

        public void AddToEnergy(int changeAmount)
        {
            energy += changeAmount;
            CheckSongFailure();
        }

        public void CheckSongFailure()
        {
            if (energy <= 0)
            {
                Debug.Log("SONG FAILED WOOF");
                OnSongFailed.Invoke();
            }
        }
    }
}