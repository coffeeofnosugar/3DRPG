using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class PlayAnimation : ActionNode
    {
        public string animatorParameter;
        
        protected override void OnStart()
        {
            
        }

        protected override void OnStop()
        {

        }

        protected override State OnUpdate()
        {
            
            return State.Success;
        }
    }
}