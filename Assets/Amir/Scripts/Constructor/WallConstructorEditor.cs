using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WallConstructor))]
public class WallConstructorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        WallConstructor script = (WallConstructor)target;
        if (GUILayout.Button("���������� �������"))
        {
            script.BuildScene();
        }
        if (GUILayout.Button("������ �������"))
        {
            script.ClearScene();
        }
    }
}
