using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using System.IO;
using System.Text.RegularExpressions;

[CanEditMultipleObjects]
public class SRename: EditorWindow{

    public static List<Object> tempSelectObjects = new List<Object>();
    List<Object> selectedObjectList = new List<Object>();
    Dictionary<Object, string> originalNameDic = new Dictionary<Object, string>();

    private Vector2 windowScrollPos;
	//To do: Make RenameList reorederable;
    //private ReorderableList reorderList;

    string renameStr = "";

    private int startIndex = 0;
    private int step = 1;

    private const string helpText = @"Nothing in the Selected List.
You can selected GameObejcts in Hierarchy Window or assets in Project Window,
then right click to select "" Add to rename List""";

    [MenuItem("SRename/ShowMeWindow")]
    static void Init()
    {
		var window = GetWindow<SRename>();
		window.minSize = new Vector2(400, 500);
        window.Show();
    }

    void OnInspectorUpdate()
    {
        Repaint();
    }

    void OnGUI()
    {
        windowScrollPos = EditorGUILayout.BeginScrollView(windowScrollPos);

        if(selectedObjectList.Count <= 0 && tempSelectObjects.Count <= 0)
        {
            EditorGUILayout.HelpBox(helpText, MessageType.Info);
        }
        else
        {
            if(tempSelectObjects.Count > 0)
            {
                int length = tempSelectObjects.Count;
                for(int i= 0; i< length; i++)
                {
                    Object obj = tempSelectObjects[i];
                    if (!selectedObjectList.Contains(obj))
                    {
                        selectedObjectList.Add(obj);
                    }

                    string objOriginalName = GetObjectName(obj);
                    if (!originalNameDic.ContainsKey(obj))
                    {
                        originalNameDic.Add(obj, objOriginalName);
                    }
                }
            }
            tempSelectObjects.Clear();

            EditorGUILayout.BeginHorizontal();
            using (var before = new EditorGUILayout.VerticalScope("Box"))
            {
                EditorGUILayout.LabelField("Selected List");

                if (selectedObjectList.Count > 0)
                {
                    var displayList = selectedObjectList.ToArray();
                    int length = displayList.Length;
                    for (int i = 0; i < length; i++)
                    {
                        EditorGUILayout.ObjectField(displayList[i], typeof(Object), true);
                    }
                }
            }

            using (var before = new EditorGUILayout.VerticalScope("Box"))
            {
                EditorGUILayout.LabelField("Original Name List");
                if (originalNameDic.Count > 0)
                {
                    int length = selectedObjectList.Count;
                    for (int i = 0; i < length; i++)
                    {
                        EditorGUILayout.LabelField(originalNameDic[selectedObjectList[i]]);
                    }
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginVertical();
            if (GUILayout.Button("Clear List"))
            {
                selectedObjectList.Clear();
                originalNameDic.Clear();
            }

            using(new EditorGUILayout.VerticalScope("Box"))
            {
                renameStr = EditorGUILayout.TextField("TargetStr", renameStr);
                startIndex = EditorGUILayout.IntField("StartIndex", startIndex);
                step = EditorGUILayout.IntField("Step", step);
            }

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Revert"))
            {
                int length = selectedObjectList.Count;
                for(int i= 0; i< length; i++)
                {
                    string originalName = originalNameDic[selectedObjectList[i]];
                    RevertName(selectedObjectList[i], originalName);
                }
            }

            if (GUILayout.Button("Apply"))
            {
                int length = selectedObjectList.Count;
                for(int i= 0; i< length; i++)
                {
                    int index = startIndex + i * step;
                    string newName = string.Format(renameStr, index);
                    ApplyNewName(selectedObjectList[i], newName);
                }
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndScrollView();
    }


    [MenuItem("Assets/Add to Rename List")]
    private static void ProjectWindw_AddSelectionsToStack()
    {
        //Debug.Log("Debug");
		var window = GetWindow<SRename>();
        window.Show();
        foreach(Object obj in Selection.objects)
        {
            if (!tempSelectObjects.Contains(obj))
            {
                tempSelectObjects.Add(obj);
                tempSelectObjects.Sort(new CustomSort());
            }
        }
    }

    [MenuItem("GameObject/Add to Rename List", false, 0)]
    private static void Hierarchy_AddSelectionsToStack(MenuCommand command)
    {
		var window = GetWindow<SRename>();
        window.Show();
        //Debug.Log("Debug = " + command.context.name);
        if (!tempSelectObjects.Contains(command.context))
        {
            tempSelectObjects.Add(command.context);
            tempSelectObjects.Sort(new CustomSort());
        }
    }

    private string GetObjectName(Object obj)
    {
        if (obj.GetType().Equals(typeof(GameObject)))
        {
            return obj.name;
        }
        else
        {
            string assetPath = AssetDatabase.GetAssetPath(obj);
            string fileName = Path.GetFileNameWithoutExtension(assetPath);
            return fileName;
        }
    }

    private void ApplyNewName(Object obj, string newName)
    {
        if (obj.GetType().Equals(typeof(GameObject)))
        {
            GameObject go = (GameObject)obj;
            go.name = newName;
        }
        else
        {
            Regex regex = new Regex(@"^[^\/\:\*\?\""\<\>\|\,]+$");
            Match match = regex.Match(newName);
            if(!match.Success)
            {
                EditorUtility.DisplayDialog("Failed to rename asset", "Asset file has invalid char, please fix it", "OK");
                return;
            }
            string assetPath = AssetDatabase.GetAssetPath(obj);
            AssetDatabase.RenameAsset(assetPath, newName);
        }
    }

    private void RevertName(Object obj, string originalName)
    {
        if (obj.GetType().Equals(typeof(GameObject)))
        {
            GameObject go = (GameObject)obj;
            go.name = originalName;
        }
        else
        {
            string assetPath = AssetDatabase.GetAssetPath(obj);
            AssetDatabase.RenameAsset(assetPath, originalName);
        }
    }
}

public class CustomSort: IComparer<Object>
{
    public int Compare(Object lhs, Object rhs)
    {
        if (lhs == rhs) return 0;
        if (lhs == null) return -1;
        if (rhs == null) return 1;
        if(lhs is GameObject && rhs is GameObject)
        {
            GameObject L = (GameObject)lhs;
            GameObject R = (GameObject)rhs;
            return (L.transform.GetSiblingIndex() > R.transform.GetSiblingIndex()) ? 1 : -1;
        }
        else
            return EditorUtility.NaturalCompare(lhs.name, rhs.name);
    }
}
