using UnityEngine;
using System;
using Unity.XR.CoreUtils;
using Rhythem.Core;

namespace Rhythem
{
    /// <summary>
    /// The main game manager that will be used throughout the project.
    /// All things relating to the game as a whole can be found here.
    /// </summary>
    public sealed class GameManager : Manager<GameManager>
    {
        [SerializeField] private GameObject vrRigPrefab;
        public bool showDebugLogs = true;

        public XROrigin VRRig { get; private set; }
        public bool Paused { get; private set; }
        public Action<bool> onPaused;


        protected override void Awake()
        {
            base.Awake();
            VRRig = Instantiate(vrRigPrefab).GetComponentInChildren<XROrigin>();
            //VRRig.name = VRRig.name.Replace("(Clone)", "");
        }

        public void PauseGame(bool pause)
        {
            if (Paused == pause)
            {
                return;
            }

            Paused = pause;
            onPaused?.Invoke(pause);
        }

        public void PauseGame(bool pause, bool freezeTime)
        {
            PauseGame(pause);
            Time.timeScale = freezeTime ? 0 : 1;
        }
    }
}