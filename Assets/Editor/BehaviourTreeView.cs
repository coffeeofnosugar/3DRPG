using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

/// <summary>
/// 行为树视图右边区域
/// </summary>
public class BehaviourTreeView : GraphView
{
    public new class UxmlFactory : UxmlFactory<BehaviourTreeView, GraphView.UxmlTraits> { }

    BehaviorTree tree;
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

    // 更新视图
    internal void PopulateView(BehaviorTree tree)
    {
        this.tree = tree;

        graphViewChanged -= OnGraphViewChanged;
        // 删除视图中所有的内容
        DeleteElements(graphElements);
        graphViewChanged += OnGraphViewChanged;

        // 逐一创建每个节点
        tree.nodes.ForEach(n => CreateNodeView(n));
    }

    /// <summary>
    /// 每当视图有变化时就会调用此方法
    /// </summary>
    /// <param name="graphViewChange"></param>
    /// <returns></returns>
    private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
    {
        // 如果正在移除的元素不为空，在移除该元素的同时，移除ScriptableObject中对应的节点
        if (graphViewChange.elementsToRemove != null)
        {
            graphViewChange.elementsToRemove.ForEach(elem =>
            {
                NodeView nodeView = elem as NodeView;
                if (nodeView != null)
                {
                    tree.DeleteNode(nodeView.node);
                }
            });
        }
        return graphViewChange;
    }

    // 添加右键菜单内容
    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        //base.BuildContextualMenu(evt);
        {
            var types = TypeCache.GetTypesDerivedFrom<ActionNode>();        // 返回派生类的无序集合
            foreach (var type in types)
            {
                // 将查找到的类赋给菜单，并绑定创建方法
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
        AddElement(nodeView);
    }
}