using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;


namespace BehaviourTree
{
    public class Root : Node
    {
        [ReadOnly, BoxGroup]
        public Node child;
        protected override void OnStart()
        {
            base.OnStart();
        }

        protected override void OnStop()
        {
            base.OnStop();
        }

        protected override State OnUpdate()
        {
            return child.Update();
        }
        public override Node Clone()
        {
            Root node = Instantiate(this);
            node.child = child.Clone();
            return node;
        }
    }
}