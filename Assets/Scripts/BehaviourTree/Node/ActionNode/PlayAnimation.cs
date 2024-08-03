using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BehaviourTree
{
    public class PlayAnimation : ActionNode
    {
        [Title("节点参数")]
        public string animatorParameter;
        
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
            
            return State.Success;
        }
    }
}