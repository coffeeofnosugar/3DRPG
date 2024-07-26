using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Player.PlayerController
{
    public abstract class DecoratorNode : Node
    {
        public Node child;
        
        public override Node Clone()
        {
            DecoratorNode node = Instantiate(this);
            if (child)
                node.child = child.Clone();
            return node;
        }
    }
}