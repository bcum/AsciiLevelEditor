using UnityEngine;
using UnityEditor;
using System.IO;

public class CreateLevel
{

    [MenuItem("Assets/Create/Level File")]
        
    public static void Create()
    {

        string[] results;
        int fileCount;

        results = AssetDatabase.FindAssets("Level", new string[] { "Assets/Levels" });

        fileCount = results.Length + 1;

        new FileStream(Application.dataPath + "\\Levels\\Level" + fileCount + ".txt", FileMode.Create);
        AssetDatabase.Refresh();

    }

}
