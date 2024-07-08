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

            // ���ڼ���·��
            if (characterStats.agent.pathPending)
            {
                return State.Running;
            }

            // ���յ�ľ���ľ���С��tolerance
            if (characterStats.agent.remainingDistance < tolerance)
            {
                return State.Success;
            }

            // ·����Ч
            if (characterStats.agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid)
            {
                return State.Failure;
            }

            return State.Running;
        }
    }
}