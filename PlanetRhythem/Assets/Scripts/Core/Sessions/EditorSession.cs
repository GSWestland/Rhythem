using Rhythem.Play;
using Rhythem.TrackEditor;
using UnityEngine;

namespace Rhythem
{
    public class EditorSession : Session
    {
        public Beatmap editorBeatmap;
        
        public override void Initialize()
        {
            base.Initialize();
            GameManager.Instance.player.SetInputModule<PlayerTrackEditInputModule>();
        }
    }
}