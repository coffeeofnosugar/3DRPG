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
                characterStats.animator.SetBool("Run", true);
                characterStats.agent.speed = characterStats.RunSpeed;
            }
        }

        protected override void OnStop()
        {
            if (_flag)
            {
                characterStats.animator.SetBool("Run", false);
            }
        }

        protected override State OnUpdate()
        {
            _flag = true;
            if (blackboard.target)
            {
                characterStats.agent.destination = blackboard.target.transform.position;
                // 与终点的距离
                if (characterStats.agent.remainingDistance < tolerance)
                {
                    return State.Success;
                }
                if (characterStats.CouldAttack())
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

            // 正在计算路径
            if (characterStats.agent.pathPending)
            {
                return State.Running;
            }

            // 与终点的距离
            if (characterStats.agent.remainingDistance < tolerance)
            {
                _flag = true;
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