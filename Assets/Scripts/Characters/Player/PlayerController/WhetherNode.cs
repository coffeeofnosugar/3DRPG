using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.PlayerController
{
    public abstract class WhetherNode : Node
    {
        public Node[] children = new Node[2];
        
        public override Node Clone()
        {
            WhetherNode node = Instantiate(this);
            for (int i = 0; i < children.Length; i++)
            {
                node.children[i] = children[i].Clone();
            }
            return node;
        }
    }
}