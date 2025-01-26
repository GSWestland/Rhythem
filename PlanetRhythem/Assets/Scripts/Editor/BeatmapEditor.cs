using UnityEditor;
using UnityEngine;

namespace Rhythem.TrackEditor {
    /// <summary>
    /// Custom editor for Beatmap.cs SerializedObject that adds a button to generate and link JSON object to the SerializedObject
    /// </summary>
    [CustomEditor(typeof(Beatmap))]
    public class BeatmapEditor : Editor
    {
        SerializedProperty trackDataPath;
        int dummyFillNoteRatio = 2;
        private void OnEnable()
        {
            trackDataPath = serializedObject.FindProperty("trackDataPath");
        }

        public override void OnInspectorGUI()
        {
            Beatmap beatmap = (Beatmap)target;
            base.OnInspectorGUI();
            if (trackDataPath.stringValue == string.Empty)
            {
                if (GUILayout.Button("Generate Track Data"))
                {
                    //generate json file with same name as beatmap
                    beatmap.DoJsonTrackDataSetup();
                    serializedObject.ApplyModifiedProperties();
                    AssetDatabase.Refresh();
                }
            }
            else
            {
                GUILayout.Label($"Track Data Path:\n    {trackDataPath.stringValue}");
                dummyFillNoteRatio = EditorGUILayout.IntField("Note : Obstacle Ratio", dummyFillNoteRatio);
                if (GUILayout.Button("Fill Song with Dummy Data"))
                {
                    beatmap.FillSongWithDummyData(dummyFillNoteRatio);
                    AssetDatabase.Refresh();
                }
            }

            serializedObject.Update();
        }
    }
}