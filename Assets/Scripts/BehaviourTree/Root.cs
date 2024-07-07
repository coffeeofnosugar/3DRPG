using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BehaviourTree
{
    public class Root : Node
    {
        //[HideInInspector] 
        public Node child;
        protected override void OnStart()
        {

        }

        protected override void OnStop()
        {

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