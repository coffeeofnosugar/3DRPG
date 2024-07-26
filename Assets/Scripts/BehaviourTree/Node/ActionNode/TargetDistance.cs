using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BehaviourTree
{
    public class TargetDistance : ActionNode
    {
        public float distance = 1;
        protected override void OnStart()
        {

        }

        protected override void OnStop()
        {

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