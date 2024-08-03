using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

public class GroupChildren : OdinEditorWindow
{
    [MenuItem("Tools/Scripts/Group Children")]
    public static void ShowWindow()
    {
        GetWindow<GroupChildren>("Group Children");
    }

    [ShowInInspector] public Transform[] children;
    [ShowInInspector] public Transform[] parent;

    [Button(ButtonSizes.Gigantic), GUIColor(0, 1, 0)]
    public void GroupChildrenByName()
    {
        if (children != null && parent != null)
        {
            parent.Where(t => t != null).ForEach(t =>
            {
                foreach (var child in children)
                {
                    if (child && child.name == t.name)
                    {
                        child.SetParent(t);
                    }
                }
            });
        }
    }
}

#endif