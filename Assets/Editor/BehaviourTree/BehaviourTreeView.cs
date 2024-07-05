using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// ��Ϊ����ͼ�ұ�����
/// </summary>
public class BehaviourTreeView : GraphView
{
    public Action<NodeView> OnNodeSelected;
    public new class UxmlFactory : UxmlFactory<BehaviourTreeView, GraphView.UxmlTraits> { }

    BehaviourTree tree;
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

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/BehaviourTree/BehaviourTreeEditor.uss");
        styleSheets.Add(styleSheet);
    }

    /// <summary>
    /// ��ScriptableObject�е�������ʾ����ͼ�У���ע��ί�У�����ͼ�з�������ʱִ��`OnGraphViewChanged`����
    /// </summary>
    /// <param name="tree"></param>
    internal void PopulateView(BehaviourTree tree)
    {
        this.tree = tree;

        graphViewChanged -= OnGraphViewChanged;
        // ɾ����ͼ�����е�����
        DeleteElements(graphElements);
        graphViewChanged += OnGraphViewChanged;

        // �ڴ���ͼʱ��û�и��ڵ㣨����մ���������Ϊ�������ʹ���һ�����ڵ㲢��ֵ����Ϊ��
        if (tree.rootNode == null)
        {
            tree.rootNode = tree.CreateNode(typeof(RootNode)) as RootNode;
            // �޸�һ��prefab��MonoBehaviour��ScriptableObject�������������Unity��ֵ�Ѿ��ı䡣
            // ÿ��һ�����Է����仯��Unity����������ڲ�����setDirty��
            // MonoBehaviour��ScriptableObject���Զ������������������ֵ�����棬�������SetDirty��
            EditorUtility.SetDirty(tree);
            AssetDatabase.SaveAssets();
        }

        // ��һ����ÿ���ڵ�
        tree.nodes.ForEach(n => CreateNodeView(n));

        // ��һ����ÿ���ڵ�
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
    /// ͨ���ڵ��guid���������Ӧ�Ľڵ�Ԫ��
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    private NodeView FindNodeView(Node node)
    {
        return GetNodeByGuid(node.guid) as NodeView;
    }

    /// <summary>
    /// ��д���෽��
    /// �÷���������ͼ�еĽڵ��໥���ӣ������д�÷������޷�����
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
    /// ÿ����ͼ�б仯ʱ�ͻ���ô˷���
    /// </summary>
    /// <param name="graphViewChange"></param>
    /// <returns></returns>
    private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
    {
        // ��������Ƴ���Ԫ�ز�Ϊ��
        if (graphViewChange.elementsToRemove != null)
        {
            graphViewChange.elementsToRemove.ForEach(elem =>
            {
                // ���Ƴ���Ԫ�ص�ͬʱ���Ƴ�ScriptableObject�ж�Ӧ�Ľڵ�
                NodeView nodeView = elem as NodeView;
                if (nodeView != null)
                {
                    tree.DeleteNode(nodeView.node);
                }

                // �Ƴ���ͬʱ���Ƴ����ĸ�����child
                Edge edge = elem as Edge;
                if (edge != null)
                {
                    NodeView parentView = edge.output.node as NodeView;
                    NodeView childView = edge.input.node as NodeView;

                    tree.RemoveChild(parentView.node, childView.node);
                }
            });
        }

        // �����ߵ�ͬʱ����ScriptableObject������ӽڵ�
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
    /// ����Ҽ��˵�����
    /// </summary>
    /// <param name="evt"></param>
    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        //base.BuildContextualMenu(evt);
        {
            var types = TypeCache.GetTypesDerivedFrom<ActionNode>();        // ��������������򼯺�
            foreach (var type in types)
            {
                // �����ҵ����ำ���˵������󶨴����¼�
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
        nodeView.OnNodeSelected = OnNodeSelected;
        AddElement(nodeView);
    }
}