using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

public class BehaviourTreeView : GraphView
{
    public new class UxmlFactory : UxmlFactory<BehaviourTreeView, GraphView.UxmlTraits> { }
    public BehaviourTreeView()
    {
        Insert(0, new GridBackground());

        // 可缩放
        this.AddManipulator(new ContentZoomer());
        // 可拖动
        this.AddManipulator(new ContentDragger());
        // 可选择
        this.AddManipulator(new SelectionDragger());
        // 可框中选择
        this.AddManipulator(new RectangleSelector());

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/BehaviourTreeEditor.uss");
        styleSheets.Add(styleSheet);
    }
}
