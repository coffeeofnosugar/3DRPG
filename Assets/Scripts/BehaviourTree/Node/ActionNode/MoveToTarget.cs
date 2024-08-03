using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BehaviourTree
{
    public class MoveToTarget : ActionNode
    {
        [Title("�ڵ����")]
        public float tolerance = 1;
        private bool _flag;
        protected override void OnStart()
        {
            base.OnStart();
            if (blackboard.target)
            {
                monsterStats.animator.SetBool("Run", true);
                monsterStats.agent.speed = monsterStats.RunSpeed;
            }
        }

        protected override void OnStop()
        {
            base.OnStop();
            if (_flag)
            {
                monsterStats.animator.SetBool("Run", false);
            }
        }

        protected override State OnUpdate()
        {
            _flag = true;
            if (blackboard.target)
            {
                monsterStats.agent.destination = blackboard.target.transform.position;
                // ���յ�ľ���
                if (monsterStats.agent.remainingDistance < tolerance)
                {
                    return State.Success;
                }
                if (monsterStats.CouldAttack())
                {
                    _flag = false;
                    return State.Success;
                }
            }
            else
            {
                monsterStats.agent.destination = monsterStats.transform.position;
                return State.Failure;
            }

            // ���ڼ���·��
            if (monsterStats.agent.pathPending)
            {
                return State.Running;
            }

            // ���յ�ľ���
            if (monsterStats.agent.remainingDistance < tolerance)
            {
                _flag = true;
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