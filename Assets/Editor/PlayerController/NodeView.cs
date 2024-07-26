using System;
using UnityEditor;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System.Timers;

namespace Player.PlayerController
{
    public class NodeView : UnityEditor.Experimental.GraphView.Node
    {
        public Action<NodeView> OnNodeSelected;
        public Node node;
        public Port[] inputs;
        public Port[] outputs;

        private Timer runningTimer;
        private bool _bool;
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

            SetupDescription();

            node.AddRunningClass += () =>
            {
                AddToClassList("running");
                _bool = true;
            };
            node.RemoveRunningClass += () =>
            {
                if (_bool)
                {
                    runningTimer = new Timer(300);
                    runningTimer.Elapsed += (sender, e) =>
                    {
                        lock (this)
                        {
                            RemoveFromClassList("running");
                            _bool = false;
                        }
                    };
                    runningTimer.AutoReset = false;
                    runningTimer.Start();
                }
            };
        }

        private void SetupDescription()
        {
            Label descriptionLabel = this.Q<Label>("description");
            
            if (node is Root)
            {
                descriptionLabel.text = "";
            }
            else if (node is CompositeNode)
            {
                if (node is LogicKeyListener logicKeyListenerNode)
                {
                    descriptionLabel.text = $"Key:{logicKeyListenerNode.checkKeyAction}";
                }
            }
            else if (node is DecoratorNode)
            {
                if (node is ChangeCharacterState changeCharacterStateNode)
                {
                    descriptionLabel.text = $"{changeCharacterStateNode.animatorParameter}";
                }
                else if (node is Log logNode)
                {
                    descriptionLabel.text = $"{logNode.message}";
                }
            }
            else if (node is WhetherNode)
            {
                if (node is CheckValue checkValueNode)
                {
                    descriptionLabel.text = $"{checkValueNode.key}=={checkValueNode.vector2Value}";
                }
            }
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
                outputs = new Port[] { InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool)) };
                outputs[0].portName = "Output";
            }
            else if (node is CompositeNode)
            {
                outputs = new Port[]
                {
                    InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool)),
                    InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool)),
                    InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool))
                };
                outputs[0].portName = "OnStart";
                outputs[1].portName = "OnUpdate";
                outputs[2].portName = "OnEnd";
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
        
        public void SortChildren()
        {
            Root composite = node as Root;
            if (composite != null)
            {
                composite.children.Sort(SortByHorizontalPosition);
            }
        }

        private int SortByHorizontalPosition(Node left, Node right)
        {
            return left.position.y < right.position.y ? -1 : 1;
        }
        
        // 舍弃该方法，直接在进入离开节点使改变标签
        // public void UpdateState()
        // {
        //     RemoveFromClassList("running");
        //     if (Application.isPlaying)
        //     {
        //         switch (node.state)
        //         {
        //             case Node.State.Running:
        //                 if (node.started)
        //                     AddToClassList("running");
        //                 break;
        //             case Node.State.Failure:
        //                 break;
        //             case Node.State.Success:
        //                 break;
        //             default:
        //                 throw new ArgumentOutOfRangeException();
        //         }
        //     }
        // }
    }
}