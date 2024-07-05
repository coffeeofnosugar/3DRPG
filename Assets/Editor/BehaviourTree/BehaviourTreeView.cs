using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 行为树视图右边区域
/// </summary>
public class BehaviourTreeView : GraphView
{
    public Action<NodeView> OnNodeSelected;
    public new class UxmlFactory : UxmlFactory<BehaviourTreeView, GraphView.UxmlTraits> { }

    BehaviourTree tree;
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

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/BehaviourTree/BehaviourTreeEditor.uss");
        styleSheets.Add(styleSheet);
    }

    /// <summary>
    /// 将ScriptableObject中的内容显示到视图中，并注册委托：在视图中发生更改时执行`OnGraphViewChanged`方法
    /// </summary>
    /// <param name="tree"></param>
    internal void PopulateView(BehaviourTree tree)
    {
        this.tree = tree;

        graphViewChanged -= OnGraphViewChanged;
        // 删除视图中所有的内容
        DeleteElements(graphElements);
        graphViewChanged += OnGraphViewChanged;

        // 在打开视图时，没有根节点（比如刚创建的新行为树），就创建一个根节点并赋值给行为树
        if (tree.rootNode == null)
        {
            tree.rootNode = tree.CreateNode(typeof(RootNode)) as RootNode;
            // 修改一个prefab的MonoBehaviour或ScriptableObject变量，必须告诉Unity该值已经改变。
            // 每当一个属性发生变化，Unity内置组件在内部调用setDirty。
            // MonoBehaviour或ScriptableObject不自动做这个，因此如果你想值被保存，必须调用SetDirty。
            EditorUtility.SetDirty(tree);
            AssetDatabase.SaveAssets();
        }

        // 逐一创建每个节点
        tree.nodes.ForEach(n => CreateNodeView(n));

        // 逐一连接每个节点
        tree.nodes.ForEach(n =>
        {
            var children = tree.GetChildren(n);
            NodeView parentView = FindNodeView(n);
            children.ForEach(c =>
            {
                NodeView childView = FindNodeView(c);

                Edge edge = parentView.output.ConnectTo(childView.input);
                AddElement(edge);
            });
        });
    }

    /// <summary>
    /// 通过节点的guid查找与其对应的节点元素
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    private NodeView FindNodeView(Node node)
    {
        return GetNodeByGuid(node.guid) as NodeView;
    }

    /// <summary>
    /// 重写父类方法
    /// 该方法能让视图中的节点相互连接，如果不写该方法将无法连接
    /// </summary>
    /// <param name="startPort"></param>
    /// <param name="nodeAdapter"></param>
    /// <returns></returns>
    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        //return base.GetCompatiblePorts(startPort, nodeAdapter);
        return ports.ToList().Where(endPort =>
        endPort.direction != startPort.direction &&
        endPort.node != startPort.node).ToList();
    }

    /// <summary>
    /// 每当视图有变化时就会调用此方法
    /// </summary>
    /// <param name="graphViewChange"></param>
    /// <returns></returns>
    private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
    {
        // 如果正在移除的元素不为空
        if (graphViewChange.elementsToRemove != null)
        {
            graphViewChange.elementsToRemove.ForEach(elem =>
            {
                // 在移除该元素的同时，移除ScriptableObject中对应的节点
                NodeView nodeView = elem as NodeView;
                if (nodeView != null)
                {
                    tree.DeleteNode(nodeView.node);
                }

                // 移除的同时，移除他的父级的child
                Edge edge = elem as Edge;
                if (edge != null)
                {
                    NodeView parentView = edge.output.node as NodeView;
                    NodeView childView = edge.input.node as NodeView;

                    tree.RemoveChild(parentView.node, childView.node);
                }
            });
        }

        // 当连线的同时，在ScriptableObject中添加子节点
        if (graphViewChange.edgesToCreate != null)
        {
            graphViewChange.edgesToCreate.ForEach(edge =>
            {
                NodeView parentView = edge.output.node as NodeView;
                NodeView childView = edge.input.node as NodeView;
                tree.AddChild(parentView.node, childView.node);
            });
        }

        return graphViewChange;
    }

    /// <summary>
    /// 添加右键菜单内容
    /// </summary>
    /// <param name="evt"></param>
    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        //base.BuildContextualMenu(evt);
        {
            var types = TypeCache.GetTypesDerivedFrom<ActionNode>();        // 返回派生类的无序集合
            foreach (var type in types)
            {
                // 将查找到的类赋给菜单，并绑定创建事件
                evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", (a) => CreateNode(type));
            }
        }
        {
            var types = TypeCache.GetTypesDerivedFrom<CompositeNode>();
            foreach (var type in types)
            {
                evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", (a) => CreateNode(type));
            }
        }
        {
            var types = TypeCache.GetTypesDerivedFrom<DecoratorNode>();
            foreach (var type in types)
            {
                evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", (a) => CreateNode(type));
            }
        }
    }

    /// <summary>
    /// 在行为树中创建节点，并在视图中创建elements节点元素
    /// </summary>
    /// <param name="type"></param>
    private void CreateNode(System.Type type)
    {
        Node node = tree.CreateNode(type);
        
        CreateNodeView(node);
    }

    /// <summary>
    /// 在视图中创建elements节点元素
    /// </summary>
    /// <param name="node"></param>
    private void CreateNodeView(Node node)
    {
        NodeView nodeView = new NodeView(node);
        nodeView.OnNodeSelected = OnNodeSelected;
        AddElement(nodeView);
    }
}