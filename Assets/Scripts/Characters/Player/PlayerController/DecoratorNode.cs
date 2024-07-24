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
                node.child = node.Clone();
            return node;
        }

        protected override State FixeUpdateState()
        {
            if (isTrigger)
                return child != null ? child.FixedUpdate() : State.Success;
            else
                return child != null ? child.FixedUpdate() : State.Running;
        }
    }
}