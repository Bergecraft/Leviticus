using UnityEngine;
using System.Collections;
using UnityEditor;
using Assets.SpaceCraft;

namespace Assets
{
    [CustomEditor(typeof(SpacecraftController), true)]
    class SpacecraftControllerEditor : Editor
    {
        string fileName = "filename.json";
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            GUILayout.Label("Persistence", EditorStyles.boldLabel);
            GUILayout.BeginHorizontal();
                GUILayout.Label("File Name");
                fileName = GUILayout.TextField(fileName);
                SpacecraftController myScript = (SpacecraftController)target;
                if(GUILayout.Button("Export"))
                {
                    myScript.ExportToFile(fileName);
                }
            GUILayout.EndHorizontal();
        }
    }
}
