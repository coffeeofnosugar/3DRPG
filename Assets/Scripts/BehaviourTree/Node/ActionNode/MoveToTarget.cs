using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class MoveToTarget : ActionNode
    {
        public float tolerance = 1;
        protected override void OnStart()
        {
            if (blackboard.target)
            {
                characterStats.animator.SetBool("Run", true);
                characterStats.agent.speed = characterStats.RunSpeed;
            }
        }

        protected override void OnStop()
        {
            characterStats.animator.SetBool("Run", false);
        }

        protected override State OnUpdate()
        {
            if (blackboard.target)
            {
                characterStats.agent.destination = blackboard.target.transform.position;
                float distance = characterStats.agent.remainingDistance;
                if (distance < characterStats.AttackRange)
                {
                    return State.Success;
                }
            }
            else
            {
                characterStats.agent.destination = characterStats.transform.position;
                return State.Failure;
            }

            // ���ڼ���·��
            if (characterStats.agent.pathPending)
            {
                return State.Running;
            }

            // ���յ�ľ���
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