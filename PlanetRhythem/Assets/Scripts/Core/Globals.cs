using System;
using UnityEngine;

namespace Rhythem
{
    public class Globals : MonoBehaviour
    {
        public const string USE_HEADSET_DIRECTION_NAME = "USE_HEADSET_DIRECTION";
        public const string USE_WORLD_DIRECTION_NAME = "USE_ WORLD_DIRECTION";
        public const string USE_CONTROLLER_DIRECTION_NAME = "USE_CONTROLLER_DIRECTION";

        public const string BEATMAP_ROOT_PATH = "Assets/Resources/Beatmaps";
        public const string JSON_DATA_PATH = "Assets/Resources/Beatmaps/BeatmapsData";

        public const int STARTING_ENERGY = 100;

        public bool HeadsetDirection { get; private set; } = true;

        public void UseHeadsetDirection()
        {
            HeadsetDirection = true;
            WorldDirection = false;
            ControllerDirection = false;
            PlayerPrefs.SetInt(USE_HEADSET_DIRECTION_NAME, 1);
            PlayerPrefs.SetInt(USE_WORLD_DIRECTION_NAME, 0);
            PlayerPrefs.SetInt(USE_CONTROLLER_DIRECTION_NAME, 0);
        }

        public bool WorldDirection { get; private set; }

        public void UseWorldDirection()
        {
            HeadsetDirection = false;
            WorldDirection = true;
            ControllerDirection = false;
            PlayerPrefs.SetInt(USE_HEADSET_DIRECTION_NAME, 0);
            PlayerPrefs.SetInt(USE_WORLD_DIRECTION_NAME, 1);
            PlayerPrefs.SetInt(USE_CONTROLLER_DIRECTION_NAME, 0);
        }

        public bool ControllerDirection { get; private set; }

        public void UseControllerDirection()
        {
            HeadsetDirection = false;
            WorldDirection = false;
            ControllerDirection = true;
            PlayerPrefs.SetInt(USE_HEADSET_DIRECTION_NAME, 0);
            PlayerPrefs.SetInt(USE_WORLD_DIRECTION_NAME, 0);
            PlayerPrefs.SetInt(USE_CONTROLLER_DIRECTION_NAME, 1);
        }


        public static Globals Instance { get; private set; }

        protected void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(transform.root);
                LoadGlobals();
            }
        }

        protected void LoadGlobals()
        {
            HeadsetDirection = Convert.ToBoolean(PlayerPrefs.GetInt(USE_HEADSET_DIRECTION_NAME, 1));
            WorldDirection = Convert.ToBoolean(PlayerPrefs.GetInt(USE_WORLD_DIRECTION_NAME, 0));
            ControllerDirection = Convert.ToBoolean(PlayerPrefs.GetInt(USE_CONTROLLER_DIRECTION_NAME, 0));
        }
    }
}