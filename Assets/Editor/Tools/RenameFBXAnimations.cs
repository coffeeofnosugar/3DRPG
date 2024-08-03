using UnityEngine;
using UnityEditor;
using System.IO;

#if UNITY_EDITOR


/// <summary>
/// 选择文件夹，将文件夹下的子物体
/// </summary>
public class RenameFBXAnimations : EditorWindow
{
    private string folderPath = "Assets"; // 默认路径

    [MenuItem("Tools/Tools/Rename FBX Animations")]
    public static void ShowWindow()
    {
        GetWindow<RenameFBXAnimations>("Rename FBX Animations");
    }

    private void OnGUI()
    {
        GUILayout.Label("Select Folder Containing FBX Files", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        folderPath = EditorGUILayout.TextField("Folder Path", folderPath);
        if (GUILayout.Button("Browse", GUILayout.Width(70)))
        {
            string selectedPath = EditorUtility.OpenFolderPanel("Select Folder", "Assets", "");
            if (!string.IsNullOrEmpty(selectedPath))
            {
                folderPath = "Assets" + selectedPath.Substring(Application.dataPath.Length);
            }
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Label("Or Drag and Drop Folder Here", EditorStyles.boldLabel);
        Event evt = Event.current;
        Rect dropArea = GUILayoutUtility.GetRect(0.0f, 50.0f, GUILayout.ExpandWidth(true));
        GUI.Box(dropArea, "Drag Folder Here");

        switch (evt.type)
        {
            case EventType.DragUpdated:
            case EventType.DragPerform:
                if (!dropArea.Contains(evt.mousePosition))
                    break;

                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                if (evt.type == EventType.DragPerform)
                {
                    DragAndDrop.AcceptDrag();
                    foreach (string path in DragAndDrop.paths)
                    {
                        if (Directory.Exists(path))
                        {
                            folderPath = path;
                            break;
                        }
                    }
                }
                Event.current.Use();
                break;
        }

        if (GUILayout.Button("Rename Animations"))
        {
            RenameAnimations();
        }
    }

    private void RenameAnimations()
    {
        if (!Directory.Exists(folderPath))
        {
            Debug.LogError("The specified folder path does not exist.");
            return;
        }

        string[] fbxPaths = Directory.GetFiles(folderPath, "*.fbx", SearchOption.AllDirectories);

        foreach (string fbxPath in fbxPaths)
        {
            string fbxName = Path.GetFileNameWithoutExtension(fbxPath);
            ModelImporter modelImporter = AssetImporter.GetAtPath(fbxPath) as ModelImporter;
            fbxName = fbxName.Replace("sword and shield ", "").Trim();

            if (modelImporter != null)
            {
                ModelImporterClipAnimation[] clipAnimations = modelImporter.defaultClipAnimations;

                foreach (var clip in clipAnimations)
                {
                    clip.name = fbxName;
                }

                modelImporter.clipAnimations = clipAnimations;
                AssetDatabase.ImportAsset(fbxPath);
                Debug.Log($"Renamed animations in {fbxPath} to {fbxName}");
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Renaming completed.");
    }
}

#endif