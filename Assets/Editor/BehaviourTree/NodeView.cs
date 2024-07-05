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
    /// 创建节点的接口和出口
    /// </summary>
    private void CreateInputPorts()
    {
        // 注册输入
        if (node is ActionNode)
        {
            //                      水平                    方向            单个                    类型
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

        // 设置属性
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
    /// 重写父类方法
    /// 在移动该节点元素后，将其新的位置信息存储到SCriptableObject节点中
    /// </summary>
    /// <param name="newPos"></param>
    public override void SetPosition(Rect newPos)
    {
        base.SetPosition(newPos);
        // UnityEngine.Rect 无法直接转换成Vector2，需要使用Rect的方法
        // 保存位置信息到ScriptableObject
        node.position.x = newPos.xMin;
        node.position.y = newPos.yMin;
    }

    /// <summary>
    /// 重写父类方法
    /// 定义一个委托，在窗口中选择该节点元素后，执行这个委托，每个节点元素在创建的时候就会注册这个委托了。
    /// 但是注册这个委托的又是另外一个委托（我们称前一个委托为委托A，后一个委托为委托B），
    /// 委托B是在创建行为树窗口时就注册了，注册实现的内容是：将该节点元素的节点Inspector信息映射到左侧的InspeView视图中。
    /// 看起来比较绕，但其实就是为了保持代码的可维护性，
    /// 没有让InspectorView的实例到出些，而为了让NodeView中的方法与BehaviourTreeEditor中的InspectorView实例联系起来而用了两个委托
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