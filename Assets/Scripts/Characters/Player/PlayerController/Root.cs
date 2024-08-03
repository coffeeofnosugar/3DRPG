using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Player.PlayerController
{
    public class Root : Node
    {
        [ReadOnly, FoldoutGroup("Node")]
        public List<Node> children;
        [SerializeField, ReadOnly, FoldoutGroup("Node")]
        private int current;
        protected override void EnterState()
        {
            current = 0;
            base.EnterState();
        }

        protected override void ExitState()
        {
            base.ExitState();
        }

        protected override State FixeUpdateState()
        {
            for (int i = current; i < children.Count; i++)
            {
                // Debug.Log(current);
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
            return State.Success;
        }

        public override Node Clone()
        {
            Root node = Instantiate(this);
            for (int i = 0; i < children.Count; i++)
            {
                if (node.children[i])
                    node.children[i] = children[i].Clone();
            }
            return node;
        }
    }
}