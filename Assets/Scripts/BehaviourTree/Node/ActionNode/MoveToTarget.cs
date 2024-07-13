using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class MoveToTarget : ActionNode
    {
        public float tolerance = 1;
        private bool _flag;
        protected override void OnStart()
        {
            if (blackboard.target)
            {
                Debug.Log("start");
                characterStats.animator.SetBool("Run", true);
                characterStats.agent.speed = characterStats.RunSpeed;
            }
        }

        protected override void OnStop()
        {
            if (_flag)
            {
                Debug.Log("stop");
                characterStats.animator.SetBool("Run", false);
            }
        }

        protected override State OnUpdate()
        {
            _flag = true;
            if (blackboard.target)
            {
                characterStats.agent.destination = blackboard.target.transform.position;
                // ���յ�ľ���
                if (characterStats.agent.remainingDistance < tolerance)
                {
                    return State.Success;
                }
                // ��һ�д���������Ŀ�ĵأ�����agent��û�з�Ӧ����������ʹ��agent�ľ���������ж�
                if ((blackboard.target.transform.position - characterStats.transform.position).sqrMagnitude < characterStats.responseDistanceSqr)
                {
                    _flag = false;
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
                _flag = true;
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