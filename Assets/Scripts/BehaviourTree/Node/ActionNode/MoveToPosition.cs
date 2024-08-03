using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BehaviourTree
{
    public class MoveToPosition : ActionNode
    {
        [Title("节点参数")]
        public float tolerance = 1f;
        protected override void OnStart()
        {
            base.OnStart();
            monsterStats.animator.SetBool("Walk", true);
            monsterStats.agent.destination = blackboard.moveToPosition;
            monsterStats.agent.speed = monsterStats.WalkSpeed;
        }

        protected override void OnStop()
        {
            monsterStats.animator.SetBool("Walk", false);
            base.OnStop();
        }

        protected override State OnUpdate()
        {
            if (blackboard.target)
            {
                return State.Failure;
            }

            // 正在计算路径
            if (monsterStats.agent.pathPending)
            {
                return State.Running;
            }

            // 与终点的距离的距离小于tolerance
            if (monsterStats.agent.remainingDistance < tolerance)
            {
                return State.Success;
            }

            // 路径无效
            if (monsterStats.agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid)
            {
                return State.Failure;
            }

            return State.Running;
        }
    }
}