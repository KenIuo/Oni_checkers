using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ObjectSpacing))]
public class ObjectSpacingEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ObjectSpacing script = (ObjectSpacing)target;
        if (GUILayout.Button("Перестроить сцену"))
        {
            script.RebuildScene();
        }
    }
}
