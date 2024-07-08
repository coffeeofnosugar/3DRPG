using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class MoveToPosition : ActionNode
    {
        public float tolerance = 1f;
        protected override void OnStart()
        {
            characterStats.animator.SetBool("Walk", true);
            characterStats.agent.destination = blackboard.moveToPosition;
            characterStats.agent.speed = characterStats.WalkSpeed;
        }

        protected override void OnStop()
        {
            characterStats.animator.SetBool("Walk", false);
        }

        protected override State OnUpdate()
        {
            if (blackboard.target)
            {
                return State.Failure;
            }

            // 正在计算路径
            if (characterStats.agent.pathPending)
            {
                return State.Running;
            }

            // 与终点的距离的距离小于tolerance
            if (characterStats.agent.remainingDistance < tolerance)
            {
                return State.Success;
            }

            // 路径无效
            if (characterStats.agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid)
            {
                return State.Failure;
            }

            return State.Running;
        }
    }
}