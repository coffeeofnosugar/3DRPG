using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Player.PlayerController
{
    public abstract class CompositeNode : Node
    {
        [ReadOnly, BoxGroup]
        public Node[] children = new Node[3];

        public override Node Clone()
        {
            CompositeNode node = Instantiate(this);
            for (int i = 0; i < children.Length; i++)
            {
                if (node.children[i])
                    node.children[i] = children[i]?.Clone();
            }
            return node;
        }
    }
}