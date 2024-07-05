using UnityEngine;
using UnityEditor.Experimental.GraphView;
using System;

public class NodeView : UnityEditor.Experimental.GraphView.Node
{
    public Action<NodeView> OnNodeSelected;
    public Node node;
    public Port input;
    public Port output;

    public NodeView(Node node)
    {
        this.node = node;
        this.title = node.name;
        this.viewDataKey = node.guid;

        style.left = node.position.x;
        style.top = node.position.y;

        CreateInputPorts();
        CreateOutputPorts();
    }

    /// <summary>
    /// �����ڵ�Ľӿںͳ���
    /// </summary>
    private void CreateInputPorts()
    {
        // ע������
        if (node is ActionNode)
        {
            //                      ˮƽ                    ����            ����                    ����
            input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
        }
        else if(node is CompositeNode)
        {
            input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
        }
        else if (node is DecoratorNode)
        {
            input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
        }
        else if (node is RootNode)
        {

        }

        // ��������
        if (input != null)
        {
            input.portName = "";
            inputContainer.Add(input);
        }
    }

    private void CreateOutputPorts()
    {
        if (node is ActionNode)
        {

        }
        else if (node is CompositeNode)
        {
            output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));
        }
        else if (node is DecoratorNode)
        {
            output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
        }
        else if (node is RootNode)
        {
            output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
        }

        if (output != null)
        {
            output.portName = "";
            outputContainer.Add(output);
        }
    }

    /// <summary>
    /// ��д���෽��
    /// ���ƶ��ýڵ�Ԫ�غ󣬽����µ�λ����Ϣ�洢��SCriptableObject�ڵ���
    /// </summary>
    /// <param name="newPos"></param>
    public override void SetPosition(Rect newPos)
    {
        base.SetPosition(newPos);
        // UnityEngine.Rect �޷�ֱ��ת����Vector2����Ҫʹ��Rect�ķ���
        // ����λ����Ϣ��ScriptableObject
        node.position.x = newPos.xMin;
        node.position.y = newPos.yMin;
    }

    /// <summary>
    /// ��д���෽��
    /// ����һ��ί�У��ڴ�����ѡ��ýڵ�Ԫ�غ�ִ�����ί�У�ÿ���ڵ�Ԫ���ڴ�����ʱ��ͻ�ע�����ί���ˡ�
    /// ����ע�����ί�е���������һ��ί�У����ǳ�ǰһ��ί��Ϊί��A����һ��ί��Ϊί��B����
    /// ί��B���ڴ�����Ϊ������ʱ��ע���ˣ�ע��ʵ�ֵ������ǣ����ýڵ�Ԫ�صĽڵ�Inspector��Ϣӳ�䵽����InspeView��ͼ�С�
    /// �������Ƚ��ƣ�����ʵ����Ϊ�˱��ִ���Ŀ�ά���ԣ�
    /// û����InspectorView��ʵ������Щ����Ϊ����NodeView�еķ�����BehaviourTreeEditor�е�InspectorViewʵ����ϵ��������������ί��
    /// </summary>
    public override void OnSelected()
    {
        base.OnSelected();
        if (OnNodeSelected != null)
        {
            OnNodeSelected.Invoke(this);
        }
    }
}