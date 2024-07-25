using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.PlayerController
{
    public class Root : Node
    {
        public List<Node> children;
        private int current;
        protected override void EnterState()
        {
            current = 0;
            Debug.Log("root 进入");
            base.EnterState();
        }

        protected override void ExitState()
        {
            Debug.Log("root 出来");
            base.ExitState();
        }

        protected override State FixeUpdateState()
        {
            for (int i = current; i < children.Count; i++)
            {
                current = i;
                var child = children[current];

                switch (child.FixedUpdate())
                {
                    case State.Running:
                        return State.Running;
                    case State.Failure:
                        continue;
                    case State.Success:
                        continue;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            return State.Running;
        }

        public override Node Clone()
        {
            Root node = Instantiate(this);
            for (int i = 0; i < children.Count; i++)
            {
                if (node.children[i])
                    node.children[i] = children[i]?.Clone();
            }
            return node;
        }
    }
}