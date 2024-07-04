using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

/// <summary>
/// ��Ϊ����ͼ�ұ�����
/// </summary>
public class BehaviourTreeView : GraphView
{
    public new class UxmlFactory : UxmlFactory<BehaviourTreeView, GraphView.UxmlTraits> { }

    BehaviorTree tree;
    public BehaviourTreeView()
    {
        Insert(0, new GridBackground());

        // ������
        this.AddManipulator(new ContentZoomer());
        // ���϶�
        this.AddManipulator(new ContentDragger());
        // ��ѡ��
        this.AddManipulator(new SelectionDragger());
        // �ɿ���ѡ��
        this.AddManipulator(new RectangleSelector());

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/BehaviourTreeEditor.uss");
        styleSheets.Add(styleSheet);
    }

    // ������ͼ
    internal void PopulateView(BehaviorTree tree)
    {
        this.tree = tree;

        graphViewChanged -= OnGraphViewChanged;
        // ɾ����ͼ�����е�����
        DeleteElements(graphElements);
        graphViewChanged += OnGraphViewChanged;

        // ��һ����ÿ���ڵ�
        tree.nodes.ForEach(n => CreateNodeView(n));
    }

    /// <summary>
    /// ÿ����ͼ�б仯ʱ�ͻ���ô˷���
    /// </summary>
    /// <param name="graphViewChange"></param>
    /// <returns></returns>
    private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
    {
        // ��������Ƴ���Ԫ�ز�Ϊ�գ����Ƴ���Ԫ�ص�ͬʱ���Ƴ�ScriptableObject�ж�Ӧ�Ľڵ�
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

    // ����Ҽ��˵�����
    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        //base.BuildContextualMenu(evt);
        {
            var types = TypeCache.GetTypesDerivedFrom<ActionNode>();        // ��������������򼯺�
            foreach (var type in types)
            {
                // �����ҵ����ำ���˵������󶨴�������
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
    /// ����Ϊ���д����ڵ㣬������ͼ�д���elements�ڵ�Ԫ��
    /// </summary>
    /// <param name="type"></param>
    private void CreateNode(System.Type type)
    {
        Node node = tree.CreateNode(type);
        CreateNodeView(node);
    }

    /// <summary>
    /// ����ͼ�д���elements�ڵ�Ԫ��
    /// </summary>
    /// <param name="node"></param>
    private void CreateNodeView(Node node)
    {
        NodeView nodeView = new NodeView(node);
        AddElement(nodeView);
    }
}