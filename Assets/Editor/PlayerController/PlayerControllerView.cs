using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;


namespace Player.PlayerController
{
    public class PlayerControllerView : GraphView
    {
        public Action<NodeView> OnNodeSelected;
        
        public new class UxmlFactory : UxmlFactory<PlayerControllerView, GraphView.UxmlTraits> { }

        private PlayerController controller;
        public PlayerControllerView()
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
            
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/GridBackground.uss");
            styleSheets.Add(styleSheet);

            Undo.undoRedoPerformed += OnUndoRedo;
        }

        private void OnUndoRedo()
        {
            PopulateView(controller);
            AssetDatabase.SaveAssets();
        }
        public void PopulateView(PlayerController playerController)
        {
            this.controller = playerController;
            graphViewChanged -= OnGraphViewChanged;
            // ɾ����ͼ�����е�����
            DeleteElements(graphElements);
            graphViewChanged += OnGraphViewChanged;
            if (playerController.rootNode == null)
            {
                playerController.rootNode = playerController.CreateNode(typeof(Root)) as Root;
                EditorUtility.SetDirty(playerController);
                AssetDatabase.SaveAssets();
            }
            
            // ��һ����ÿ���ڵ�
            playerController.nodes.ForEach(n => CreateNodeView(n));
            
            // ��һ����ÿ���ڵ�
            playerController.nodes.ForEach(n =>
            {
                var children = playerController.GetChildren(n);
                NodeView parentView = FindNodeView(n);
                for (int i = 0; i < children.Count; i++)
                {
                    if (children[i] != null)
                    {
                        NodeView childView = FindNodeView(children[i]);
                        Edge edge = parentView.outputs[i].ConnectTo(childView.inputs[0]);
                        AddElement(edge);
                    }
                }
            });
        }

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
        
        
        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            if (graphViewChange.elementsToRemove != null)
            {
                graphViewChange.elementsToRemove.ForEach(elem =>
                {
                    NodeView nodeView = elem as NodeView;
                    if (nodeView != null)
                    {
                        controller.DeleteNode(nodeView.node);
                    }
                    
                    Edge edge = elem as Edge;
                    if (edge != null)
                    {
                        NodeView parentView = edge.output.node as NodeView;
                        NodeView childView = edge.input.node as NodeView;

                        controller.RemoveChild(parentView.node, childView.node, edge.output.tabIndex);
                    }
                });
            }

            if (graphViewChange.edgesToCreate != null)
            {
                graphViewChange.edgesToCreate.ForEach(edge =>
                {
                    NodeView parentView = edge.output.node as NodeView;
                    NodeView childView = edge.input.node as NodeView;
                    controller.AddChild(parentView.node, childView.node, edge.output.tabIndex);
                });
            }

            return graphViewChange;
        }
        
        
        /// <summary>
        /// �Ҽ��˵�
        /// </summary>
        /// <param name="evt"></param>
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            Vector2 mousePosition = this.ChangeCoordinatesTo(contentContainer, evt.localMousePosition);
            {
                var types = TypeCache.GetTypesDerivedFrom<CompositeNode>();
                foreach (var type in types)
                {
                    evt.menu.AppendAction($"[CompositeNode]/{type.Name}", (a) => CreateNode(type, mousePosition));
                }
            }
            {
                var types = TypeCache.GetTypesDerivedFrom<DecoratorNode>();
                foreach (var type in types)
                {
                    evt.menu.AppendAction($"[DecoratorNode]/{type.Name}", (a) => CreateNode(type, mousePosition));
                }
            }
            {
                var types = TypeCache.GetTypesDerivedFrom<WhetherNode>();
                foreach (var type in types)
                {
                    evt.menu.AppendAction($"[WhetherNode]/{type.Name}", (a) => CreateNode(type, mousePosition));
                }
            }
        }

        private void CreateNode(Type type, Vector2 position)
        {
            Node node = controller.CreateNode(type);
            node.position = position;
            CreateNodeView(node);
        }

        private void CreateNodeView(Node node)
        {
            NodeView nodeView = new NodeView(node);
            nodeView.OnNodeSelected = OnNodeSelected;
            AddElement(nodeView);
        }
    }
}