using UnityEditor;
using UnityEngine;
using System.IO;

public static class S_ProjectInitializer
{
    private static readonly string[] Folders =
    {
        "App",

        "App/Animations",

        "App/Arts",
        "App/Arts/Sprites",

        "App/Audio",
        "App/Audio/Musics",
        "App/Audio/SFX",
        "App/Audio/UI",

        "App/Inputs",

        "App/Prefabs",
        "App/Prefabs/Managers",
        "App/Prefabs/UI",

        "App/Scenes",
        "App/Scenes/Tests",

        "App/Scripts",
        "App/Scripts/Editor",
        "App/Scripts/Runtime/Containers",
        "App/Scripts/Runtime/Inputs",
        "App/Scripts/Runtime/Managers",
        "App/Scripts/Runtime/UI",
        "App/Scripts/Runtime/Utils",
        "App/Scripts/Runtime/Wrapper",
        "App/Scripts/Runtime/Wrapper/RSE",
        "App/Scripts/Runtime/Wrapper/RSO",
        "App/Scripts/Runtime/Wrapper/SSO",

        "App/SOD",
        "App/SOD/RSE",
        "App/SOD/RSO",
        "App/SOD/SSO",

        "Plugins",
        "Resources",
        "ScriptTemplates",
        "Settings"
    };

    [MenuItem("Tools/Initialize Project Folders")]
    public static void CreateProjectFolders()
    {
        foreach (var folder in Folders) CreateFolder(folder);

        AssetDatabase.Refresh();
        Debug.Log("✅ Project folder structure initialized.");
    }

    private static void CreateFolder(string relativePath)
    {
        var fullPath = Path.Combine(Application.dataPath, relativePath);

        if (Directory.Exists(fullPath)) return;

        Directory.CreateDirectory(fullPath);
    }
}