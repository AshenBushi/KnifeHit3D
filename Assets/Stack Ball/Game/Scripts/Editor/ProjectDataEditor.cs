using UnityEditor;
using Watermelon;

[CustomEditor(typeof(ProjectData))]
public class ProjectDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.BeginVertical(EditorStylesExtended.editorSkin.box);

        EditorGUILayoutCustom.Header("SETTINGS");

        EditorGUILayoutCustom.DrawAllProperties(serializedObject);
        
        EditorGUILayout.EndVertical();
        
        serializedObject.ApplyModifiedProperties();
    }
}
