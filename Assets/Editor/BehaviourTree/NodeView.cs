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

            // ץȡdescriptionԪ��
            Label descriptionLabel = this.Q<Label>("description");
            // �����"description"��ʾ�ľ���node.description�ֶ�
            descriptionLabel.bindingPath = "description";
            // ����������Ҫ������ĸ���
            descriptionLabel.Bind(new SerializedObject(node));
        }

        /// <summary>
        /// ���ڵ����uss��ǩ���Ա����uxml�е���ʾ��ɫ��
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
        /// �����ڵ�Ľӿںͳ���
        /// </summary>
        private void CreateInputPorts()
        {
            // ע������
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

            // ��������
            if (input != null)
            {
                input.portName = "";
                // ����input�Ĳ���Ϊ���ϵ���
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
                // ����input�Ĳ���Ϊ���µ���
                output.style.flexDirection = FlexDirection.ColumnReverse;
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
            // ʱ�ƶ��ڵ�Ԫ�ؿ���ʹ��Ctrl+Z����
            Undo.RecordObject(node, "Behaviour Tree (Set Position)");
            // UnityEngine.Rect �޷�ֱ��ת����Vector2����Ҫʹ��Rect�ķ���
            // ����λ����Ϣ��ScriptableObject
            node.position.x = newPos.xMin;
            node.position.y = newPos.yMin;
            EditorUtility.SetDirty(node);       // ������������ڳ��򼯱���
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

        /// <summary>
        /// ���ӽڵ�����
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
        /// return 1 : λ�ò���
        /// return 0 : λ�ò���
        /// return -1 : right��left��λ��
        /// ������Ȼд����left��right��������ʵ�ʵĵ����п��Կ�����ǰ��Ĳ�����ʵ���ұߵĲ�������
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private int SortByHorizontalPosition(Node left, Node right)
        {
            return left.position.x < right.position.x ? -1 : 1;
        }

        /// <summary>
        /// ʵʱ��ÿ���ڵ���ӱ�ǩrunning\failure\success
        /// </summary>
        public void UpdateState()
        {
            // ���������ǩ
            RemoveFromClassList("running");
            RemoveFromClassList("failure");
            RemoveFromClassList("success");

            // ��playmodeģʽ�¸�����ڵ��״̬��ӱ�ǩ
            if (Application.isPlaying)
            {
                switch (node.state)
                {
                    case Node.State.Running:
                        // �ڵ��Ĭ��state��Running����Щ״̬����û�����й���״̬Ҳ��Running
                        // �����ж�started�����þ���Ϊ�˲����������û�����й��Ľڵ����Running״̬
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