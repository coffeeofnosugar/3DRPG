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
            monsterStats.animator.SetBool("Walk", true);
            monsterStats.agent.destination = blackboard.moveToPosition;
            monsterStats.agent.speed = monsterStats.WalkSpeed;
        }

        protected override void OnStop()
        {
            monsterStats.animator.SetBool("Walk", false);
        }

        protected override State OnUpdate()
        {
            if (blackboard.target)
            {
                return State.Failure;
            }

            // ���ڼ���·��
            if (monsterStats.agent.pathPending)
            {
                return State.Running;
            }

            // ���յ�ľ���ľ���С��tolerance
            if (monsterStats.agent.remainingDistance < tolerance)
            {
                return State.Success;
            }

            // ·����Ч
            if (monsterStats.agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid)
            {
                return State.Failure;
            }

            return State.Running;
        }
    }
}