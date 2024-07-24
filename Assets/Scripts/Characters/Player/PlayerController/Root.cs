using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.PlayerController
{
    public class Root : Node
    {
        public Node child;
        protected override void EnterState()
        {
            
        }

        protected override void ExitState()
        {
            
        }

        protected override State FixeUpdateState()
        {
            child.FixedUpdate();
            return State.Running;
        }

        public override Node Clone()
        {
            Root node = Instantiate(this);
            node.child = child.Clone();
            return node;
        }
    }
}