using UnityEngine;
using Sirenix.OdinInspector;
using SimpleJSON;
using System.IO;

namespace Rhythem.TrackEditor
{
    /// <summary>
    /// Contains logic for converting user input into serializable JSON data and storing it in a JSON object
    /// </summary>
    public class InputRecorder : MonoBehaviour
    {
        private static string BEATMAP_ROOT_PATH = "Assets/Resources/Beatmaps";
        private static string JSON_DATA_PATH = "Assets/Resources/Beatmaps/BeatmapsData";

        private Beatmap currentBeatmap;

        void Start()
        {

        }

        void Update()
        {

        }
    }
}