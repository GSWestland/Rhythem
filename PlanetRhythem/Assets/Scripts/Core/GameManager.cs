using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Unity.XR.CoreUtils;
using Rhythem.TrackEditor;
using Sirenix.OdinInspector;
using UnityEngine.Rendering;

namespace Rhythem
{
    /// <summary>
    /// The main game manager that will be used throughout the project.
    /// All things relating to the game as a whole can be found here.
    /// </summary>
    public sealed class GameManager : Manager<GameManager>
    {
        [Title("Assignables")]
        [SerializeField] public GameObject playerPrefab;
        private GameObject playerObjectInstance;
        public bool showDebugLogs = true;
        public XROrigin VRRig { get; private set; }
        public Play.Player player { get; private set; }
        public bool Paused { get; private set; }
        public Action<bool> onPaused;

        public SongScoringProfile scoreProfile;
        private Beatmap _currentBeatmap;

        //this should only ever be assigned in a Menu Session
        public Beatmap CurrentBeatmap
        {
            get
            {
                if (_currentBeatmap == null)
                {
                    return null;
                }
                return _currentBeatmap;
            }
            set
            {
                _currentBeatmap = value;

                //if (SessionsManager.Instance.GetCurrentSession<MenuSession>() != null)
                //{
                //    _currentBeatmap = value;
                //}
                //else
                //{
                //    Debug.LogError("# Rhythem.Core Error: Cannot assign beatmap outside of menu mode.");
                //}
            }
        }

        protected override void Awake()
        {
            base.Awake();

            if (SessionsManager.Instance.GetCurrentSession() == null)
            {
                //SessionsManager.Instance.LoadSession<MenuSession>();
                SessionsManager.Instance.LoadSession<SongSession>();
            }
            playerObjectInstance = Instantiate(playerPrefab);
            playerObjectInstance.name = playerObjectInstance.name.Replace("(Clone)", "");
            VRRig = playerObjectInstance.GetComponentInChildren<XROrigin>();
            player = playerObjectInstance.GetComponent<Play.Player>();
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

        public void LoadScene(int scene, bool additive = false)
        {
            if (additive)
            {
                SceneManager.LoadScene(scene, LoadSceneMode.Additive);
            }
            else
            {
                SceneManager.LoadScene(scene, LoadSceneMode.Single);
            }
        }
    }
}