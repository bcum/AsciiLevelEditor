using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class LevelEditor : EditorWindow
{

    string text = "No level files found...";
    TextAsset txtAsset;
    Vector2 scroll;

    // Add menu item named "My Window" to the Window menu
    [MenuItem("Window/Level Editor")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        GetWindow(typeof(LevelEditor));
    }

    string[] levelObjectGUIDs, systemFonts;
    List<Object> levelObjects = new List<Object>();
    List<string> levelObjectNames = new List<string>();

    Object txtSource;

    int indexL = 0;
    int indexF = 0;

    bool fileChanged = false;

    string font;

    GUIStyle customStyle = new GUIStyle();
    GUIStyle centeredStyle = new GUIStyle();



    private void OnEnable()
    {
        GetLevels();
    }


    private void GetLevels()
    {
        levelObjectGUIDs = AssetDatabase.FindAssets("Level", new string[] { "Assets/Levels" });

        for (int x = 0; x < levelObjectGUIDs.Length; x++)
        {

            Object tempO = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(levelObjectGUIDs[x]), typeof(Object)) as Object;
            levelObjects.Add(tempO);
            levelObjectNames.Add(tempO.name);

        }
        txtSource = levelObjects[indexL];
    }

    private void Awake()
    {
        GetFonts();
        
        //customStyle.border = new RectOffset(4, 4, 4, 4);
        centeredStyle.alignment = TextAnchor.MiddleCenter;
        centeredStyle.fontSize = 16;
        customStyle.fontSize = 16;
                
    }


    private void GetFonts()
    {
        systemFonts = Font.GetOSInstalledFontNames();

        for (; indexF < systemFonts.Length; indexF++)
        {
            if (systemFonts[indexF].Contains("Courier New"))
            {
                break;
            }
        }

        font = systemFonts[indexF];
        customStyle.font = Font.CreateDynamicFontFromOSFont(font, 16);
    }


    void OnGUI()
    {
        // Set the border
        customStyle.border = new RectOffset(5,5,5,5);

        // Set the background
        customStyle.normal.background = GUI.skin.textArea.normal.background;

        // Set the padding around the text
        customStyle.padding = new RectOffset(10, 10, 10, 10);

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Ascii Map Editor", centeredStyle);

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginVertical();

        EditorGUILayout.LabelField("Font: ", EditorStyles.boldLabel, GUILayout.Width(120));

        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical();

        EditorGUI.BeginChangeCheck();
        indexF = EditorGUILayout.Popup(indexF, systemFonts);

        if (EditorGUI.EndChangeCheck())
        {
            font = systemFonts[indexF];
            customStyle.font = Font.CreateDynamicFontFromOSFont(font, 12);
        }

        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginVertical();

        EditorGUILayout.LabelField("Select Level File: ", EditorStyles.boldLabel, GUILayout.Width(120));

        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical();

        EditorGUI.BeginChangeCheck();
        indexL = EditorGUILayout.Popup(indexL, levelObjectNames.ToArray());

        if (EditorGUI.EndChangeCheck())
        {
            txtSource = levelObjects[indexL];
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical();

        if (GUILayout.Button("New",GUILayout.Height(14)))
        {
            CreateLevel.Create();
            GetLevels();
        }

        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        //txtSource = EditorGUILayout.ObjectField(txtSource, typeof(Object), true);

        TextAsset newTxtAsset = (TextAsset)txtSource;

        if (newTxtAsset != txtAsset)
            ReadTextAsset(newTxtAsset);

        scroll = EditorGUILayout.BeginScrollView(scroll, customStyle);
        EditorGUI.BeginChangeCheck();
        text = EditorGUILayout.TextArea(text, customStyle);
        if (EditorGUI.EndChangeCheck())
        {
            //Undo.RecordObject(text, "Changed Level Text");
            fileChanged = true;
        }
        EditorGUILayout.EndScrollView();

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();

        if (fileChanged)
        {
            EditorGUILayout.LabelField("Your file is NOT saved", EditorStyles.boldLabel);
            GUI.enabled = true; // Enable following control (Button for saving) 
        }
        else
        {
            EditorGUILayout.LabelField("Your file is saved", EditorStyles.boldLabel);
            GUI.enabled = false; // Disable following control (Button for saving) 
        }

        if (GUILayout.Button("Save"))
        {
            SaveTextAsset(text, txtSource);

        }
        //EditorGUILayout.Space();

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();

    }

    void ReadTextAsset(TextAsset txt)
    {
        text = txt.text;
        txtAsset = txt;
        fileChanged = false;
    }

    void SaveTextAsset(string txtToSave, Object source)
    {
        File.WriteAllText(AssetDatabase.GetAssetPath(source), txtToSave);
        fileChanged = false;
        EditorUtility.SetDirty(source);
    }
}