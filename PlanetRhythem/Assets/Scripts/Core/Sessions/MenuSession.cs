using Rhythem.Play;
using UnityEngine;

namespace Rhythem
{
    public class MenuSession : Session
    {
        public override void Initialize()
        {
            base.Initialize();
            GameManager.Instance.player.SetInputModule<PlayerSongPlayInputModule>();
        }
    }
}