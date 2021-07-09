using UnityEngine;
using UnityEditor;
using Watermelon;

public class LevelDatabaseWindow : EditorWindow
{
    private LevelDatabase levelDatabase;
    private LevelDatabaseEditor levelDatabaseEditor;

    private Vector2 scrollView;

    [MenuItem("Tools/Editor/Level Editor")]
    static void ShowWindow()
    {
        LevelDatabaseWindow window = (LevelDatabaseWindow)EditorWindow.GetWindow(typeof(LevelDatabaseWindow));
        window.titleContent = new GUIContent("Level Editor");
        window.Show();
    }

    private void OnEnable()
    {
        if (!levelDatabase)
        {
            levelDatabase = EditorUtils.GetAsset<LevelDatabase>();

            levelDatabaseEditor = Editor.CreateEditor(levelDatabase, typeof(LevelDatabaseEditor)) as LevelDatabaseEditor;
        }
    }

    private void OnDisable()
    {
        GameObject editorGameObject = GameObject.Find("[EDITOR]");
        if (editorGameObject != null)
            DestroyImmediate(editorGameObject);
    }

    private void OnGUI()
    {
        if (levelDatabase != null && levelDatabaseEditor != null)
        {
            scrollView = EditorGUILayout.BeginScrollView(scrollView);

            levelDatabaseEditor.serializedObject.Update();
            levelDatabaseEditor.OnInspectorGUI();
            levelDatabaseEditor.serializedObject.ApplyModifiedProperties();

            GUILayout.Space(5);

            EditorGUILayout.EndScrollView();
        }
    }
}
