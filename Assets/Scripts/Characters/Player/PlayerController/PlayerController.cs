using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Player.PlayerController
{
    [CreateAssetMenu()]
    public class PlayerController : ScriptableObject
    {
        public Node rootNode;
        public Node.State controllerState = Node.State.Running;
        public List<Node> nodes = new List<Node>();

        public Blackboard blackboard = new Blackboard();
        public CharacterStats characterStats;
        public PlayerInputController playerInputController;

        public void FixeUpdate()
        {
            if (rootNode.state == Node.State.Running)
                rootNode.FixeUpdate();
        }
        
        public Node.State Update()
        {
            if (rootNode.state == Node.State.Running)
            {
                controllerState = rootNode.Update();
            }
            return controllerState;
        }

        public void LateUpdate()
        {
            if (rootNode.state == Node.State.Running)
                rootNode.LateUpdate();
        }
        
        public void Bind(CharacterStats characterStats, PlayerInputController playerInputController)
        {
            this.characterStats = characterStats;
            this.playerInputController = playerInputController;
            characterStats.blackboard = blackboard;
            Traverse(rootNode, n =>
            {
                n._characterStats = characterStats;
                n._playerInputController = playerInputController;
                n.blackboard = blackboard;
            });
        }

        public PlayerController Clone()
        {
            PlayerController controller = Instantiate(this);
            controller.rootNode = controller.rootNode.Clone();
            controller.nodes = new List<Node>();
            Traverse(controller.rootNode, node =>
            {
                controller.nodes.Add(node);
            });
            return controller;
        }

        private void Traverse(Node node, Action<Node> visiter)
        {
            if (node)
            {
                visiter.Invoke(node);
                var children = GetChildren(node);
                children.ForEach(n => Traverse(n, visiter));
            }
        }

#if UNITY_EDITOR
        #region Ϊview����
        public Node CreateNode(System.Type type)
        {
            Node node = ScriptableObject.CreateInstance(type) as Node;
            node.name = type.Name;
            node.guid = GUID.Generate().ToString();

            Undo.RecordObject(this, "Player Controller (CreateNode)");
            nodes.Add(node);
            
            if (!Application.isPlaying)
            {
                AssetDatabase.AddObjectToAsset(node, this);
            }
            Undo.RegisterCreatedObjectUndo(this, "Player Controller (CreateNode)");
            AssetDatabase.SaveAssets();
            return node;
        }

        public void DeleteNode(Node node)
        {
            Undo.RecordObject(this, "Player Controller (DeleteNode)");
            nodes.Remove(node);
            Undo.DestroyObjectImmediate(node);
            AssetDatabase.SaveAssets();
        }

        public void AddChild(Node parent, Node child, int childIndex = 0)
        {
            Root rootNode = parent as Root;
            if (rootNode)
            {
                Undo.RecordObject(rootNode,  "Player Controller (AddChild)");
                rootNode.child = child;
                EditorUtility.SetDirty(rootNode);
            }
            
            CompositeNode compositeNode = parent as CompositeNode;
            if (compositeNode)
            {
                Undo.RecordObject(compositeNode, "Player Controller (AddChild)");
                compositeNode.children[childIndex] = child;
                EditorUtility.SetDirty(compositeNode);
            }
            
            DecoratorNode decoratorNode = parent as DecoratorNode;
            if (decoratorNode)
            {
                Undo.RecordObject(decoratorNode, "Player Controller (AddChild)");
                decoratorNode.child = child;
                EditorUtility.SetDirty(decoratorNode);
            }
            
            WhetherNode whetherNode = parent as WhetherNode;
            if (whetherNode)
            {
                Undo.RecordObject(whetherNode, "Player Controller (AddChild)");
                whetherNode.children[childIndex] = child;
                EditorUtility.SetDirty(whetherNode);
            }
        }
        
        public void RemoveChild(Node parent, Node child, int childIndex = 0)
        {
            Root rootNode = parent as Root;
            if (rootNode)
            {
                Undo.RecordObject(rootNode,  "Player Controller (RemoveChild)");
                rootNode.child = null;
                EditorUtility.SetDirty(rootNode);
            }
            
            CompositeNode compositeNode = parent as CompositeNode;
            if (compositeNode)
            {
                Undo.RecordObject(compositeNode, "Player Controller (RemoveChild)");
                compositeNode.children[childIndex] = null;
                EditorUtility.SetDirty(compositeNode);
            }
            
            DecoratorNode decoratorNode = parent as DecoratorNode;
            if (decoratorNode)
            {
                Undo.RecordObject(decoratorNode, "Player Controller (RemoveChild)");
                decoratorNode.child = null;
                EditorUtility.SetDirty(decoratorNode);
            }
            
            WhetherNode whetherNode = parent as WhetherNode;
            if (whetherNode)
            {
                Undo.RecordObject(whetherNode, "Player Controller (AddChild)");
                whetherNode.children[childIndex] = null;
                EditorUtility.SetDirty(whetherNode);
            }
        }

        public List<Node> GetChildren(Node parent)
        {
            List<Node> children = new List<Node>();

            Root rootNode = parent as Root;
            if (rootNode && rootNode.child != null)
            {
                children.Add(rootNode.child);
            }
            CompositeNode composite = parent as CompositeNode;
            if (composite)
            {
                children = new List<Node>(composite.children);
            }
            
            DecoratorNode decorator = parent as DecoratorNode;
            if (decorator && decorator.child != null)
            {
                children.Add(decorator.child);
            }
            
            WhetherNode whetherNode = parent as WhetherNode;
            if (whetherNode)
            {
                children = new List<Node>(whetherNode.children);
            }
            return children;
        }
        #endregion
#endif
    }
}