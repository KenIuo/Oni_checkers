using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WallConstructor))]
public class WallConstructorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        WallConstructor script = (WallConstructor)target;
        if (GUILayout.Button("Расставить объекты"))
        {
            script.BuildScene();
        }
        if (GUILayout.Button("Убрать объекты"))
        {
            script.ClearScene();
        }
    }
}
