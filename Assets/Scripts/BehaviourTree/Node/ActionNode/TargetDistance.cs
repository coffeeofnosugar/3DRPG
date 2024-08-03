using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;


namespace BehaviourTree
{
    public class TargetDistance : ActionNode
    {
        [Title("节点参数")]
        public float distance = 1;
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
            if (blackboard.target && monsterStats.agent.remainingDistance <= distance)
            {
                return State.Failure;
            }
            return State.Success;
        }
    }
}