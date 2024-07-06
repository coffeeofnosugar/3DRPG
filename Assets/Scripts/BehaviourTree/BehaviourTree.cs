using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


namespace BehaviourTree
{
    /// <summary>
    /// 一个怪物的行为树
    /// </summary>
    [CreateAssetMenu()]
    public class BehaviourTree : ScriptableObject
    {
        public Node rootNode;
        public Node.State treeState = Node.State.Running;
        public List<Node> nodes = new List<Node>();

        public Node.State Update()
        {
            if (rootNode.state == Node.State.Running)
            {
                treeState = rootNode.Update();
            }
            return treeState;
        }

        public BehaviourTree Clone()
        {
            BehaviourTree tree = Instantiate(this);
            tree.rootNode = tree.rootNode.Clone();
            return tree;
        }

        #region 为视图服务的方法

        /// <summary>
        /// 创建节点ScriptableObject类
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Node CreateNode(System.Type type)
        {
            Node node = ScriptableObject.CreateInstance(type) as Node;
            node.name = type.Name;
            node.guid = GUID.Generate().ToString();
            nodes.Add(node);

            AssetDatabase.AddObjectToAsset(node, this);
            AssetDatabase.SaveAssets();
            return node;
        }

        /// <summary>
        /// 删除节点ScriptableObject类
        /// </summary>
        /// <param name="node"></param>
        public void DeleteNode(Node node)
        {
            nodes.Remove(node);
            AssetDatabase.RemoveObjectFromAsset(node);
            AssetDatabase.SaveAssets();
        }

        /// <summary>
        /// 在ScriptableObject中添加子节点
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="child"></param>
        public void AddChild(Node parent, Node child)
        {
            RootNode rootNode = parent as RootNode;
            if (rootNode)
            {
                rootNode.child = child;
            }

            CompositeNode composite = parent as CompositeNode;
            if (composite)
            {
                composite.children.Add(child);
            }

            DecoratorNode decorator = parent as DecoratorNode;
            if (decorator)
            {
                decorator.child = child;
            }
        }

        public void RemoveChild(Node parent, Node child)
        {
            RootNode rootNode = parent as RootNode;
            if (rootNode)
            {
                rootNode.child = null;
            }

            CompositeNode composite = parent as CompositeNode;
            if (composite)
            {
                composite.children.Remove(child);
            }

            DecoratorNode decorator = parent as DecoratorNode;
            if (decorator)
            {
                decorator.child = null;
            }
        }

        public List<Node> GetChildren(Node parent)
        {
            List<Node> children = new List<Node>();

            RootNode rootNode = parent as RootNode;
            if (rootNode && rootNode.child != null)
            {
                children.Add(rootNode.child);
            }

            CompositeNode composite = parent as CompositeNode;
            if (composite)
            {
                children = composite.children;
            }

            DecoratorNode decorator = parent as DecoratorNode;
            if (decorator && decorator.child != null)
            {
                children.Add(decorator.child);
            }
            return children;
        }
        #endregion
    }
}