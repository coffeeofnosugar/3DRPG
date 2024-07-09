using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEditor;
using UnityEngine;


namespace BehaviourTree
{
    /// <summary>
    /// һ���������Ϊ��
    /// </summary>
    [CreateAssetMenu()]
    public class BehaviourTree : ScriptableObject
    {
        public Node rootNode;
        public Node.State treeState = Node.State.Running;
        public List<Node> nodes = new List<Node>();
        public Blackboard blackboard = new Blackboard();
        public CharacterStats characterStats;

        public Node.State Update()
        {
            if (rootNode.state == Node.State.Running)
            {
                treeState = rootNode.Update();
            }
            return treeState;
        }

        /// <summary>
        /// �ص����� + �ݹ麯�� �����
        /// ��ȫ�������нڵ�
        /// </summary>
        /// <param name="node"></param>
        /// <param name="visiter"></param>
        private void Traverse(Node node, System.Action<Node> visiter)
        {
            if (node)
            {
                visiter.Invoke(node);
                var children = GetChildren(node);
                children.ForEach((n) => { Traverse(n, visiter); });
            }
        }

        public BehaviourTree Clone()
        {
            // ��¡��Ϊ��
            BehaviourTree tree = Instantiate(this);
            // ��¡���ڵ�
            tree.rootNode = tree.rootNode.Clone();
            // ��ʼ����¡��Ľڵ��б�
            tree.nodes = new List<Node>();      // �б�Ŀ�¡�е��鷳

            Traverse(tree.rootNode, (n) =>
            {
                tree.nodes.Add(n);
            });
            return tree;
        }

        /// <summary>
        /// �󶨺ڰ�
        /// </summary>
        public void Bind(CharacterStats character)
        {
            this.characterStats = character;
            Traverse(rootNode, (n) =>
            {
                n.characterStats = character;
                n.blackboard = blackboard;
            });
        }

        /// <summary>
        /// Ѱ�ҹ���Ŀ��
        /// </summary>
        public void FoundTarget()
        {
            var colliders = Physics.OverlapSphere(characterStats.transform.position, characterStats.SightRadius, 1 << 6);
            if (colliders.Length > 0)
            {
                blackboard.target = colliders[0].gameObject;
                blackboard.distanceTarget = characterStats.agent.remainingDistance;
                characterStats.animator.SetLayerWeight(1, 1);
            }
            else
            {
                blackboard.target = null;
                characterStats.animator.SetLayerWeight(1, 0);
            }
        }

        /// <summary>
        /// cooldown��ʱ��
        /// </summary>
        public void RunCooldown()
        {
            //characterStats.LastAttackTime += Time.deltaTime;
            //characterStats.LastSkillTime += Time.deltaTime;
        }

        public void DebugShow()
        {
            //Debugs.Instance["cooldonw"] = characterStats.LastAttackTime.ToString();
            Debugs.Instance["movePosition"] = characterStats.agent.destination.ToString();
            if (blackboard.target)
            {
                Debugs.Instance["distance"] = blackboard.distanceTarget.ToString();
            }
        }


#if UNITY_EDITOR
        #region Ϊ��ͼ����ķ���

        /// <summary>
        /// �����ڵ�ScriptableObject��
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Node CreateNode(System.Type type)
        {
            Node node = ScriptableObject.CreateInstance(type) as Node;
            node.name = type.Name;
            node.guid = GUID.Generate().ToString();

            Undo.RecordObject(this, "Behaviour Tree (CreateNode)");     // ��ӽڵ�Ԫ��ʱ��������ֻ�ı�����ͼ�еĻ��漰����������Ϊ����������Ҫ�������ScriptableObject��
            nodes.Add(node);

            if (!Application.isPlaying)     // ��PlayModeģʽ���޷���ӽڵ�
            {
                AssetDatabase.AddObjectToAsset(node, this);
            }
            Undo.RegisterCreatedObjectUndo(this, "Behaviour Tree (CreateNode)");        // �漰��Project�еĴ�����Ҫʹ�����
            AssetDatabase.SaveAssets();
            return node;
        }

        /// <summary>
        /// ɾ���ڵ�ScriptableObject��
        /// </summary>
        /// <param name="node"></param>
        public void DeleteNode(Node node)
        {
            Undo.RecordObject(this, "Behaviour Tree (DeleteNode)");
            nodes.Remove(node);

            //AssetDatabase.RemoveObjectFromAsset(node);
            Undo.DestroyObjectImmediate(node);      // ������һ�����
            AssetDatabase.SaveAssets();
        }

        /// <summary>
        /// ��ScriptableObject������ӽڵ�
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="child"></param>
        public void AddChild(Node parent, Node child)
        {
            Root rootNode = parent as Root;
            if (rootNode)
            {
                Undo.RecordObject(rootNode, "Behaviour Tree (AddChild)");
                rootNode.child = child;
                EditorUtility.SetDirty(rootNode);
            }

            CompositeNode composite = parent as CompositeNode;
            if (composite)
            {
                Undo.RecordObject(composite, "Behaviour Tree (AddChild)");
                composite.children.Add(child);
                EditorUtility.SetDirty(composite);
                Weight weight = parent as Weight;
                if (weight)
                    weight.weights.Add(1);
            }

            DecoratorNode decorator = parent as DecoratorNode;
            if (decorator)
            {
                Undo.RecordObject(decorator, "Behaviour Tree (AddChild)");
                decorator.child = child;
                EditorUtility.SetDirty(decorator);
            }
        }

        public void RemoveChild(Node parent, Node child)
        {
            Root rootNode = parent as Root;
            if (rootNode)
            {
                Undo.RecordObject(rootNode, "Behaviour Tree (AddChild)");
                rootNode.child = null;
                EditorUtility.SetDirty(rootNode);
            }

            CompositeNode composite = parent as CompositeNode;
            if (composite)
            {
                Undo.RecordObject(composite, "Behaviour Tree (AddChild)");
                composite.children.Remove(child);
                EditorUtility.SetDirty(composite);
                Weight weight = parent as Weight;
                if (weight)
                    weight.weights.Remove(weight.weights[0]);
            }

            DecoratorNode decorator = parent as DecoratorNode;
            if (decorator)
            {
                Undo.RecordObject(decorator, "Behaviour Tree (AddChild)");
                decorator.child = null;
                EditorUtility.SetDirty(decorator);
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
#endif
    }
}