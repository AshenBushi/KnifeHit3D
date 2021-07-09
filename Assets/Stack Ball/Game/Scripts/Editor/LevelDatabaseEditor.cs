using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using Watermelon;
using System.Collections;

[CustomEditor(typeof(LevelDatabase))]
public class LevelDatabaseEditor : Editor
{
    public Texture2D circleTexture;
    public Material defaultMaterial;
    public Material obstacleMaterial;
    
    private RoundButton roundButton;

    private int tabIndex = 0;

    private GUIStyle addCenteredStyle;
    private GUIContent addGUIContent;

    private Vector2 scrollView = Vector2.zero;

    private bool isInited = false;

    private LevelDatabase targetRef;

    private SerializedProperty platformTypesPlatformsProperty;
    private SerializedProperty levelColorPresetsProperty;
    private SerializedProperty levelsProperty;

    private LevelPlatformType tempPlatformType;

    private Pagination pagination = new Pagination(10, 5);

    private int selectedPlatformTypeIndex = -1;

    private int selectedLevelIndex = -1;
    private int selectedPlatformIndex = -1;
    private SerializedProperty selectedLevelProperty;

    private SerializedProperty selectedLevelPlatformTypeProperty;
    private SerializedProperty selectedLevelDataProperty;

    private List<Platform> visualisePlatforms = new List<Platform>();

    private static Transform platformsContainer;
    private static Transform PlatformsContainer
    {
        get
        {
            if(platformsContainer == null)
            {
                GameObject tempPlatformsObject = GameObject.Find("[EDITOR]");
                if (tempPlatformsObject == null)
                {
                    tempPlatformsObject = new GameObject("[EDITOR]");
                }

                if(tempPlatformsObject != null)
                    platformsContainer = tempPlatformsObject.transform;
            }

            return platformsContainer;
        }
    }

    private string[] platformTypes;

    private int copiedLevelsCount;

    private const string PLATFORM_TYPES_PLATFORMS_PROPERTY_NAME = "levelPlatformTypes";
    private const string LEVEL_COLOR_PRESETS_PROPERTY_NAME = "levelColorPresets";
    private const string LEVELS_PROPERTY_NAME = "levels";

    private readonly string[] EDITOR_TABS = new string[3] { "Levels", "Platform Types", "Color Presets" };

    [InitializeOnLoadMethod]
    private static void LevelDatabaseStatic()
    {
        EditorApplication.playModeStateChanged += LogPlayModeState;
    }

    private static void LogPlayModeState(PlayModeStateChange state)
    {
        if(state == PlayModeStateChange.ExitingEditMode)
        {
            Transform tempPlatformContainer = PlatformsContainer;
            if (tempPlatformContainer != null)
            {
                DestroyImmediate(tempPlatformContainer.gameObject);
            }
        }
    }

    private void OnEnable()
    {
        roundButton = new RoundButton();

        platformTypesPlatformsProperty = serializedObject.FindProperty(PLATFORM_TYPES_PLATFORMS_PROPERTY_NAME);
        levelColorPresetsProperty = serializedObject.FindProperty(LEVEL_COLOR_PRESETS_PROPERTY_NAME);
        levelsProperty = serializedObject.FindProperty(LEVELS_PROPERTY_NAME);

        targetRef = (LevelDatabase)target;

        tempPlatformType = new LevelPlatformType();

        InitPlatformTypes();
        
        pagination.Init(levelsProperty);

        UnselectLevel();
    }
    
    private void InitStyles()
    {
        if (isInited)
            return;

        addGUIContent = new GUIContent("+");

        addCenteredStyle = GUI.skin.box;
        addCenteredStyle.alignment = TextAnchor.MiddleCenter;
        addCenteredStyle.fontSize = 36;
        addCenteredStyle.normal.textColor = GUI.skin.button.normal.textColor;

        isInited = true;
    }

    private void InitPlatformTypes()
    {
        platformTypes = targetRef.levelPlatformTypes.Select(x => x.name).ToArray();
    }

    #region Platforms
    private void SpawnPlatforms()
    {
        if (selectedLevelIndex != -1 && selectedLevelProperty != null)
        {
            visualisePlatforms = new List<Platform>();

            for (int i = 0; i < selectedLevelDataProperty.arraySize; i++)
            {
                SpawnPlatform(i);
            }
        }
    }

    private void ResetPlatforms()
    {
        if (selectedLevelIndex != -1 && selectedLevelProperty != null)
        {
            int platformsCount = visualisePlatforms.Count;
            for (int i = 0; i < platformsCount; i++)
            {
                visualisePlatforms[i].transform.position = new Vector3(0, i * LevelController.PLATFORM_POSITION_VALUE, 0);
                visualisePlatforms[i].transform.localEulerAngles = new Vector3(0, i * LevelController.PLATFORM_ROTATE_VALUE, 0);

                visualisePlatforms[i].Init(i, targetRef.levels[selectedLevelIndex].levelPlatformsData[i].data, Color.white, 1, 1, defaultMaterial, obstacleMaterial);
            }
        }
    }

    private void SpawnPlatform(int index)
    {
        GameObject gameObject = Instantiate(targetRef.levelPlatformTypes[targetRef.levels[selectedLevelIndex].levelPlatformTypeID].prefab);
        gameObject.transform.SetParent(PlatformsContainer);
        gameObject.transform.position = new Vector3(0, index * LevelController.PLATFORM_POSITION_VALUE, 0);
        gameObject.transform.localEulerAngles = new Vector3(0, index * LevelController.PLATFORM_ROTATE_VALUE, 0);

        Platform platform = gameObject.GetComponent<Platform>();
        platform.InitializeComponent();

        platform.Init(index, targetRef.levels[selectedLevelIndex].levelPlatformsData[index].data, Color.white, 1, 1, defaultMaterial, obstacleMaterial);

        visualisePlatforms.Add(platform);
    }

    private void SetPlatformData(int index)
    {
        visualisePlatforms[index].Init(index, targetRef.levels[selectedLevelIndex].levelPlatformsData[index].data, Color.white, 1, 1, defaultMaterial, obstacleMaterial);
    }

    private void RemovePlatform(int index)
    {
        if (visualisePlatforms.IsInRange(index))
        {
            DestroyImmediate(visualisePlatforms[index].gameObject);

            visualisePlatforms.RemoveAt(index);

            ResetPlatforms();
        }
    }

    private void DestroyPlatforms()
    {
        if (visualisePlatforms != null)
        {
            int platformsCount = visualisePlatforms.Count;
            for (int i = 0; i < platformsCount; i++)
            {
                if(visualisePlatforms[i].gameObject != null)
                {
                    DestroyImmediate(visualisePlatforms[i].gameObject);
                }
            }

            visualisePlatforms = null;
        }
    }
    #endregion

    public void OnDestroy()
    {
        Transform tempPlatformContainer = PlatformsContainer;
        if (tempPlatformContainer != null)
        {
            DestroyImmediate(tempPlatformContainer.gameObject);
        }
    }

    public void OnDisable()
    {
        visualisePlatforms = null;

        Transform tempPlatformContainer = PlatformsContainer;
        if (tempPlatformContainer != null)
        {
            DestroyImmediate(tempPlatformContainer.gameObject);
        }
    }

    private void UnselectLevel()
    {
        // Unselect level
        selectedLevelIndex = -1;
        selectedLevelProperty = null;

        visualisePlatforms = null;

        Transform tempPlatformContainer = PlatformsContainer;
        if (tempPlatformContainer != null)
        {
            DestroyImmediate(tempPlatformContainer.gameObject);
        }
    }

    private void LevelsGUI()
    {
        EditorGUILayout.BeginVertical(GUI.skin.box, GUILayout.MinHeight(276));

        if(GUILayout.Button("Add Level"))
        {
            levelsProperty.arraySize++;

            SerializedProperty tempLevel = levelsProperty.GetArrayElementAtIndex(levelsProperty.arraySize - 1);
            tempLevel.FindPropertyRelative("levelPlatformTypeID").intValue = 0;
            tempLevel.FindPropertyRelative("levelPlatformsData").arraySize = 0;

            pagination.Init(levelsProperty);
        }

        int arraySize = levelsProperty.arraySize;
        // Draw level list
        int max = pagination.GetMaxElementNumber();
        for (int i = pagination.GetMinElementNumber(); i < max; i++)
        {
            bool isLevelSelected = selectedLevelIndex == i;

            if (isLevelSelected)
                GUI.color = EditorColor.green05;

            Rect levelRect = EditorGUILayout.BeginHorizontal(GUI.skin.box);
            EditorGUILayout.LabelField("Level " + i.ToString());

            if (GUILayout.Button("▶", GUILayout.Width(16), GUILayout.Height(16)))
            {
                PlayerPrefs.SetInt(GameController.LAST_LEVEL_PREFS_NAME, i);
            }

            if (GUILayout.Button("↑", GUILayout.Width(16), GUILayout.Height(16)))
            {
                if (i > 0)
                {
                    levelsProperty.MoveArrayElement(i, i - 1);

                    UnselectLevel();
                }
            }
            if (GUILayout.Button("↓", GUILayout.Width(16), GUILayout.Height(16)))
            {
                if (i + 1 < arraySize)
                {
                    levelsProperty.MoveArrayElement(i, i + 1);

                    UnselectLevel();
                }
            }
            GUI.color = Color.red;
            if (GUILayout.Button("x", GUILayout.Width(16), GUILayout.Height(16)))
            {
                int tempIndex = i;

                if (EditorUtility.DisplayDialog("Are you sure?", "This level will be removed!", "Remove", "Cancel"))
                {
                    levelsProperty.RemoveFromVariableArrayAt(tempIndex);

                    UnselectLevel();

                    pagination.Init(levelsProperty);
                }
            }
            GUI.color = Color.white;
            
            if (GUI.Button(levelRect, GUIContent.none, GUIStyle.none))
            {
                // Unselect level
                if(isLevelSelected)
                {
                    UnselectLevel();
                }
                // Select level
                else
                {
                    if (selectedLevelIndex != -1)
                        UnselectLevel();

                    selectedLevelIndex = i;
                    selectedLevelProperty = levelsProperty.GetArrayElementAtIndex(i);

                    selectedLevelPlatformTypeProperty = selectedLevelProperty.FindPropertyRelative("levelPlatformTypeID");
                    selectedLevelDataProperty = selectedLevelProperty.FindPropertyRelative("levelPlatformsData");

                    // Initialize round button
                    roundButton.Init(targetRef.levelPlatformTypes[targetRef.levels[i].levelPlatformTypeID].elementsAmount, circleTexture);

                    // Spawn platforms
                    SpawnPlatforms();
                }
            }

            EditorGUILayout.EndHorizontal();

            if (isLevelSelected)
                GUI.color = Color.white;
        }

        //Draw pagination
        pagination.DrawPagination();

        EditorGUILayout.EndVertical();

        GUILayout.Space(12);

        if (selectedLevelIndex != -1 && selectedLevelProperty != null)
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);

            GUILayout.Space(3);

            EditorGUI.BeginChangeCheck();
            int platformTypeID = EditorGUILayout.Popup("Platform Type: ", selectedLevelPlatformTypeProperty.intValue, platformTypes);
            if (EditorGUI.EndChangeCheck())
            {
                if (targetRef.levelPlatformTypes[targetRef.levels[selectedLevelIndex].levelPlatformTypeID].elementsAmount != targetRef.levelPlatformTypes[platformTypeID].elementsAmount)
                {
                    if (selectedLevelDataProperty.arraySize > 0)
                    {
                        if (EditorUtility.DisplayDialog("Are you sure?", "This platform has different elements amount, if it will changed platforms list will be cleared!", "Remove", "Cancel"))
                        {
                            selectedLevelDataProperty.arraySize = 0;
                        }
                        else
                        {
                            return;
                        }
                    }
                }

                DestroyPlatforms();

                selectedLevelPlatformTypeProperty.intValue = platformTypeID;

                // Re-initialize round button
                roundButton.Init(targetRef.levelPlatformTypes[platformTypeID].elementsAmount, circleTexture);

                EditorApplication.delayCall += delegate
                {
                    SpawnPlatforms();
                };
            }

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            
            if (GUILayout.Button("Add Platform"))
            {
                selectedLevelDataProperty.arraySize++;

                selectedPlatformIndex = selectedLevelDataProperty.arraySize - 1;

                EditorApplication.delayCall += delegate
                {
                    targetRef.levels[selectedLevelIndex].levelPlatformsData[selectedPlatformIndex].data = roundButton.buttonValues;

                    SpawnPlatform(selectedPlatformIndex);
                };
            }

            int arrayDataSize = selectedLevelDataProperty.arraySize;
            for (int i = arrayDataSize - 1; i >= 0; i--)
            {
                bool isPlatformSelected = selectedPlatformIndex == i;

                if (isPlatformSelected)
                    GUI.color = EditorColor.green05;

                Rect levelRect = EditorGUILayout.BeginHorizontal(GUI.skin.box);

                // Draw platform title
                EditorGUILayout.LabelField("Platform " + i.ToString());
                
                GUI.color = Color.red;
                if (GUILayout.Button("x", EditorStyles.miniButton, GUILayout.Width(16), GUILayout.Height(16)))
                {
                    int tempIndex = i;

                    if (EditorUtility.DisplayDialog("Are you sure?", "This platform will be removed!", "Remove", "Cancel"))
                    {
                        selectedLevelDataProperty.RemoveFromVariableArrayAt(tempIndex);

                        // Unselect platform
                        selectedPlatformIndex = -1;

                        // Remove platform
                        RemovePlatform(tempIndex);
                    }
                }
                GUI.color = Color.white;

                if (GUI.Button(levelRect, GUIContent.none, GUIStyle.none))
                {
                    // Unselect platform
                    if(isPlatformSelected)
                    {
                        selectedPlatformIndex = -1;
                    }
                    else
                    {
                        selectedPlatformIndex = i;
                        
                        roundButton.buttonValues = targetRef.levels[selectedLevelIndex].levelPlatformsData[selectedPlatformIndex].data;
                    }
                }

                EditorGUILayout.EndHorizontal();

                if (selectedPlatformIndex == i)
                {
                    EditorGUILayout.BeginHorizontal(GUI.skin.box);

                    bool buttonClicked = false;
                    roundButton.DrawCirlceButtons(ref buttonClicked);

                    if(buttonClicked)
                    {
                        targetRef.levels[selectedLevelIndex].levelPlatformsData[selectedPlatformIndex].data = roundButton.buttonValues;

                        SetPlatformData(selectedPlatformIndex);

                        EditorUtility.SetDirty(target);
                    }

                    EditorGUILayout.EndHorizontal();
                }
                
                if (isPlatformSelected)
                    GUI.color = Color.white;
            };

            EditorGUILayout.EndVertical();
        }
    }

    private void PlatformTypesGUI()
    {
        bool uniqueNameError = false;
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUI.BeginChangeCheck();
        tempPlatformType.name = EditorGUILayout.TextField("Name: ", tempPlatformType.name);
        if(EditorGUI.EndChangeCheck())
        {
            uniqueNameError = System.Array.FindIndex(targetRef.levelPlatformTypes, x => x.name == tempPlatformType.name) == -1;
        }
        tempPlatformType.prefab = EditorGUILayout.ObjectField(new GUIContent("Platform: "), tempPlatformType.prefab, typeof(GameObject), false) as GameObject;

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Platform Elements: ");
        tempPlatformType.elementsAmount = EditorGUILayout.IntSlider(tempPlatformType.elementsAmount, 2, 15);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Add Platform"))
        {
            platformTypesPlatformsProperty.arraySize++;

            SerializedProperty serializedProperty = platformTypesPlatformsProperty.GetArrayElementAtIndex(platformTypesPlatformsProperty.arraySize - 1);
            serializedProperty.FindPropertyRelative("name").stringValue = tempPlatformType.name;
            serializedProperty.FindPropertyRelative("elementsAmount").intValue = tempPlatformType.elementsAmount;
            serializedProperty.FindPropertyRelative("prefab").objectReferenceValue = tempPlatformType.prefab;

            tempPlatformType = new LevelPlatformType();

            GUI.FocusControl(null);

            EditorApplication.delayCall += delegate
            {
                InitPlatformTypes();
            };
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();

        int arraySize = platformTypesPlatformsProperty.arraySize;
        for (int i = 0; i < arraySize; i++)
        {
            bool isSelected = selectedPlatformTypeIndex == i;

            SerializedProperty levelColorPreset = platformTypesPlatformsProperty.GetArrayElementAtIndex(i);

            SerializedProperty levelColorPresetsNameProperty = levelColorPreset.FindPropertyRelative("name");
            SerializedProperty levelColorPresetsPrefabProperty = levelColorPreset.FindPropertyRelative("prefab");
            SerializedProperty levelColorPresetsElementsAmountProperty = levelColorPreset.FindPropertyRelative("elementsAmount");

            Rect rect = EditorGUILayout.BeginHorizontal(GUI.skin.box);

            EditorGUILayout.LabelField(levelColorPresetsNameProperty.stringValue);
            
            GUI.color = Color.red;
            if (GUILayout.Button("x", EditorStyles.miniButton, GUILayout.Width(16), GUILayout.Height(16)))
            {
                platformTypesPlatformsProperty.RemoveFromVariableArrayAt(i);

                EditorApplication.delayCall += delegate
                {
                    InitPlatformTypes();
                };
            }
            GUI.color = Color.white;

            if (GUI.Button(rect, "", GUIStyle.none))
            {
                if (isSelected)
                {
                    selectedPlatformTypeIndex = -1;
                }
                else
                {
                    selectedPlatformTypeIndex = i;
                }

                return;
            }

            EditorGUILayout.EndHorizontal();

            if(isSelected)
            {
                EditorGUILayout.BeginVertical(GUI.skin.box);
                EditorGUILayout.PropertyField(levelColorPresetsNameProperty);
                EditorGUILayout.PropertyField(levelColorPresetsPrefabProperty);
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Platform Elements: ");
                levelColorPresetsElementsAmountProperty.intValue = EditorGUILayout.IntSlider(levelColorPresetsElementsAmountProperty.intValue, 2, 15);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
            }
        }
    }

    private void ColorPresetsGUI()
    {
        Event e = Event.current;

        Rect boxRect = GUILayoutUtility.GetRect(addGUIContent, addCenteredStyle, GUILayout.Height(48), GUILayout.ExpandWidth(true));
        if(GUI.Button(boxRect, addGUIContent, addCenteredStyle))
        {
            EditorGUIUtility.ShowObjectPicker<LevelColorPreset>(null, false, "", EditorGUIUtility.GetControlID(FocusType.Passive) + 100);
        }
        
        for (int i = 0; i < levelColorPresetsProperty.arraySize; i++)
        {
            SerializedProperty arrayProperty = levelColorPresetsProperty.GetArrayElementAtIndex(i);
            
            EditorGUILayout.BeginVertical(GUI.skin.box);
            if(arrayProperty.objectReferenceValue == null)
            {
                EditorGUILayout.HelpBox("Color preset can't be empty. Please, remove it or link preset!", MessageType.Error, true);
            }
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(arrayProperty);

            GUI.color = Color.red;
            if(GUILayout.Button("x", EditorStyles.miniButton, GUILayout.Width(16), GUILayout.Height(16)))
            {
                levelColorPresetsProperty.RemoveFromObjectArrayAt(i);
            }
            GUI.color = Color.white;

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }

        if (e.type == EventType.ExecuteCommand && e.commandName == "ObjectSelectorClosed")
        {
            if(EditorGUIUtility.GetObjectPickerObject() != null)
            {
                LevelColorPreset levelColorPreset = EditorGUIUtility.GetObjectPickerObject() as LevelColorPreset;

                if (System.Array.FindIndex(targetRef.levelColorPresets, x => x == levelColorPreset) == -1)
                {
                    levelColorPresetsProperty.arraySize++;
                    levelColorPresetsProperty.GetArrayElementAtIndex(levelColorPresetsProperty.arraySize - 1).objectReferenceValue = levelColorPreset;
                }
                else
                {
                    Debug.LogWarning("[LevelDatabase]: Color Preset already in list!");
                }

                Repaint();
            }
        }

        DraggingAndDropping(boxRect);
    }

    public override void OnInspectorGUI()
    {
        if(Application.isPlaying)
        {
            EditorGUILayout.HelpBox("Editor doesn't work in play mode. Please, stop the game to edit levels!", MessageType.Warning, true);

            GUI.enabled = false;
        }

        scrollView = EditorGUILayout.BeginScrollView(scrollView, false, false);
        serializedObject.Update();

        InitStyles();

        EditorGUI.BeginChangeCheck();
        tabIndex = GUILayout.Toolbar(tabIndex, EDITOR_TABS);
        if(EditorGUI.EndChangeCheck())
        {
            scrollView = Vector2.zero;
        }

        switch (tabIndex)
        {
            case 0:
                LevelsGUI();
                break;
            case 1:
                PlatformTypesGUI();
                break;
            case 2:
                ColorPresetsGUI();
                break;
        }

        serializedObject.ApplyModifiedProperties();
        EditorGUILayout.EndScrollView();
    }
    
    private void DraggingAndDropping(Rect dropArea)
    {
        // Cache the current event.
        Event currentEvent = Event.current;
        
        // If the drop area doesn't contain the mouse then return.
        if (!dropArea.Contains(currentEvent.mousePosition))
            return;


        switch (currentEvent.type)
        {
            // If the mouse is dragging something...
            case EventType.DragUpdated:

                // ... change whether or not the drag *can* be performed by changing the visual mode of the cursor based on the IsDragValid function.
                DragAndDrop.visualMode = IsDragValid() ? DragAndDropVisualMode.Link : DragAndDropVisualMode.Rejected;

                // Make sure the event isn't used by anything else.
                currentEvent.Use();

                break;

            // If the mouse was dragging something and has released...
            case EventType.DragPerform:

                // ... accept the drag event.
                DragAndDrop.AcceptDrag();

                serializedObject.Update();
                for (int i = DragAndDrop.objectReferences.Length - 1; i >= 0; i--)
                {
                    LevelColorPreset levelColorPreset = DragAndDrop.objectReferences[i] as LevelColorPreset;
                    if (levelColorPreset != null)
                    {
                        if (System.Array.FindIndex(targetRef.levelColorPresets, x => x == levelColorPreset) == -1)
                        {
                            levelColorPresetsProperty.arraySize++;
                            levelColorPresetsProperty.GetArrayElementAtIndex(levelColorPresetsProperty.arraySize - 1).objectReferenceValue = levelColorPreset;
                        }
                        else
                        {
                            Debug.LogWarning("[LevelDatabase]: Color Preset already in list!");
                        }
                    }
                }
                serializedObject.ApplyModifiedProperties();
                
                // Make sure the event isn't used by anything else.
                currentEvent.Use();

                break;
        }

    }

    private bool IsDragValid()
    {
        // Go through all the objects being dragged...
        for (int i = 0; i < DragAndDrop.objectReferences.Length; i++)
        {
            if (DragAndDrop.objectReferences[i].GetType() != typeof(LevelColorPreset))
            {
                return false;
            }
        }

        // If none of the dragging objects returned that the drag was invalid, return that it is valid.
        return true;
    }
}
