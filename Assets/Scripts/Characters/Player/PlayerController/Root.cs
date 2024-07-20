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

        protected override void FixeUpdateState()
        {
            child.FixeUpdate();
        }

        protected override State UpdateState()
        {
            child.Update();
            return State.Running;
        }

        protected override void LateUpdateState()
        {
            child.LateUpdate();
        }

        public override Node Clone()
        {
            Root node = Instantiate(this);
            node.child = child.Clone();
            return node;
        }
    }
}