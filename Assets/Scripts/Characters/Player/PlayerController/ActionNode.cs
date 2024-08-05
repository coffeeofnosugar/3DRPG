using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;


namespace Player.PlayerController
{
    public abstract class ActionNode : Node
    {
        [ReadOnly, BoxGroup]
        public Node child;
        
        public override Node Clone()
        {
            ActionNode node = Instantiate(this);
            if (child)
                node.child = child.Clone();
            return node;
        }
    }
}