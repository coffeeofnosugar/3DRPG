using System;
using BehaviourTree;
using UnityEditor;
using UnityEngine;
using UnityEditor.Experimental.GraphView;

namespace Player.PlayerController
{
    public class NodeView : UnityEditor.Experimental.GraphView.Node
    {
        public Action<NodeView> OnNodeSelected;
        public Node node;
        public Port[] inputs;
        public Port[] outputs;

        public NodeView(Node node) : base("Assets/Editor/PlayerController/NodeView.uxml")
        {
            this.node = node;
            this.title = node.name;
            this.viewDataKey = node.guid;

            style.left = node.position.x;
            style.top = node.position.y;

            CreateInputPorts();
            CreateOutPorts();
            SetupClasses();
        }

        private void SetupClasses()
        {
            if (node is Root)
                AddToClassList("root");
            else if (node is CompositeNode)
                AddToClassList("composite");
            else if (node is DecoratorNode)
                AddToClassList("decorator");
            else if (node is WhetherNode)
                AddToClassList("whether");
        }


        private void CreateInputPorts()
        {
            if (node is Root)
            {
                
            }
            else if (node is CompositeNode)
            {
                inputs = new Port[] { InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool)) };
                inputs[0].portName = "Input";
            }
            else if (node is DecoratorNode)
            {
                inputs = new Port[] { InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool)) };
                inputs[0].portName = "Input";
            }
            else if (node is WhetherNode)
            {
                inputs = new Port[] { InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool)) };
                inputs[0].portName = "Input";
            }

            if (inputs != null)
            {
                for (int i = 0; i < inputs.Length; i++)
                {
                    inputs[i].tabIndex = i;
                    inputContainer.Add(inputs[i]);
                }
            }
        }

        private void CreateOutPorts()
        {
            if (node is Root)
            {
                outputs = new Port[] { InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool)) };
                outputs[0].portName = "Output";
            }
            else if (node is CompositeNode)
            {
                outputs = new Port[]
                {
                    InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool)),
                    InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool)),
                    InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool)),
                    InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool))
                };
                outputs[0].portName = "OnStart";
                outputs[1].portName = "OnUpdate";
                outputs[2].portName = "OnEnd";
                outputs[3].portName = "OnTrigger";
            }
            else if (node is DecoratorNode)
            {
                outputs = new Port[] { InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool)) };
                outputs[0].portName = "Output";
            }
            else if (node is WhetherNode)
            {
                outputs = new Port[]
                {
                    InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool)),
                    InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool))
                };
                outputs[0].portName = "True";
                outputs[1].portName = "False";
            }

            if (outputs != null)
            {
                for (int i = 0; i < outputs.Length; i++)
                {
                    outputs[i].tabIndex = i;
                    outputContainer.Add(outputs[i]);
                }
            }
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            Undo.RecordObject(node, "Player Controller (Set Position)");
            node.position.x = newPos.xMin;
            node.position.y = newPos.yMin;
            EditorUtility.SetDirty(node);
        }

        public override void OnSelected()
        {
            base.OnSelected();
            OnNodeSelected?.Invoke(this);
        }
    }
}