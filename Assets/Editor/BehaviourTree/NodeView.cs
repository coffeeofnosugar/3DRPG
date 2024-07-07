using UnityEngine;
using UnityEditor.Experimental.GraphView;
using System;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;

namespace BehaviourTree
{
    public class NodeView : UnityEditor.Experimental.GraphView.Node
    {
        public Action<NodeView> OnNodeSelected;
        public Node node;
        public Port input;
        public Port output;

        public NodeView(Node node) : base("Assets/Editor/BehaviourTree/NodeView.uxml")
        {
            this.node = node;
            this.title = node.name;
            this.viewDataKey = node.guid;

            style.left = node.position.x;
            style.top = node.position.y;

            CreateInputPorts();
            CreateOutputPorts();
            SetupClasses();

            // 抓取description元素
            Label descriptionLabel = this.Q<Label>("description");
            // 这里的"description"表示的就是node.description字段
            descriptionLabel.bindingPath = "description";
            // 创建并绑定需要检测对象的副本
            descriptionLabel.Bind(new SerializedObject(node));
        }

        /// <summary>
        /// 给节点添加uss标签，以便更改uxml中的显示颜色等
        /// </summary>
        private void SetupClasses()
        {
            if (node is ActionNode)
            {
                AddToClassList("action");
            }
            else if (node is CompositeNode)
            {
                AddToClassList("composite");
            }
            else if (node is DecoratorNode)
            {
                AddToClassList("decorator");
            }
            else if (node is Root)
            {
                AddToClassList("root");
            }
        }

        /// <summary>
        /// 创建节点的接口和出口
        /// </summary>
        private void CreateInputPorts()
        {
            // 注册输入
            if (node is ActionNode)
            {
                input = new NodePort(Direction.Input, Port.Capacity.Single);
            }
            else if (node is CompositeNode)
            {
                input = new NodePort(Direction.Input, Port.Capacity.Single);
            }
            else if (node is DecoratorNode)
            {
                input = new NodePort(Direction.Input, Port.Capacity.Single);
            }
            else if (node is Root)
            {

            }

            // 设置属性
            if (input != null)
            {
                input.portName = "";
                // 设置input的布局为从上到下
                input.style.flexDirection = FlexDirection.Column;
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
                output = new NodePort(Direction.Output, Port.Capacity.Multi);
            }
            else if (node is DecoratorNode)
            {
                output = new NodePort(Direction.Output, Port.Capacity.Single);
            }
            else if (node is Root)
            {
                output = new NodePort(Direction.Output, Port.Capacity.Single);
            }

            if (output != null)
            {
                output.portName = "";
                // 设置input的布局为从下到上
                output.style.flexDirection = FlexDirection.ColumnReverse;
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
            // 时移动节点元素可以使用Ctrl+Z撤回
            Undo.RecordObject(node, "Behaviour Tree (Set Position)");
            // UnityEngine.Rect 无法直接转换成Vector2，需要使用Rect的方法
            // 保存位置信息到ScriptableObject
            node.position.x = newPos.xMin;
            node.position.y = newPos.yMin;
            EditorUtility.SetDirty(node);       // 这个方法有助于程序集保存
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

        /// <summary>
        /// 给子节点排序
        /// </summary>
        public void SortChildren()
        {
            CompositeNode composite = node as CompositeNode;
            if (composite != null)
            {
                composite.children.Sort(SortByHorizontalPosition);
            }
        }

        /// <summary>
        /// return 1 : 位置不变
        /// return 0 : 位置不变
        /// return -1 : right与left换位置
        /// 这里虽然写的是left和right，但是在实际的调试中可以看到，前面的参数其实在右边的参数后面
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private int SortByHorizontalPosition(Node left, Node right)
        {
            return left.position.x < right.position.x ? -1 : 1;
        }

        /// <summary>
        /// 实时给每个节点添加标签running\failure\success
        /// </summary>
        public void UpdateState()
        {
            // 先清除掉标签
            RemoveFromClassList("running");
            RemoveFromClassList("failure");
            RemoveFromClassList("success");

            // 在playmode模式下根据其节点的状态添加标签
            if (Application.isPlaying)
            {
                switch (node.state)
                {
                    case Node.State.Running:
                        // 节点的默认state是Running，有些状态从来没有运行过其状态也是Running
                        // 这里判断started的作用就是为了不给这个从来没有运行过的节点添加Running状态
                        if (node.started)
                        {
                            AddToClassList("running");
                        }
                        break;
                    case Node.State.Failure:
                        AddToClassList("failure");
                        break;
                    case Node.State.Success:
                        AddToClassList("success");
                        break;
                    default:
                        break;
                }
            }
        }
    }
}