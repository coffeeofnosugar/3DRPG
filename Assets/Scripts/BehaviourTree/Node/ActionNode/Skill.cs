using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BehaviourTree
{
    public class Skill : ActionNode
    {
        [Title("节点参数")]
        public string animatorParameter;

        private bool _flag;
        protected override void OnStart()
        {
            base.OnStart();
            if (blackboard.target)
            {
                if (!monsterStats.CouldAttack(animatorParameter))
                {
                    _flag = true;
                    return;
                }

                blackboard.lastAttackTime[animatorParameter] = 0;
                _flag = false;
                monsterStats.animator.SetBool("Run", false);
                // 停止移动
                monsterStats.agent.destination = monsterStats.transform.position;
                // 朝向攻击目标
                monsterStats.transform.LookAt(blackboard.target.transform);
                // 播放攻击动画
                monsterStats.animator.SetTrigger(animatorParameter);
            }
        }

        protected override void OnStop()
        {
            base.OnStop();
        }

        protected override State OnUpdate()
        {
            if (_flag)
            {
                return State.Success;
            }
            var info = monsterStats.animator.GetCurrentAnimatorStateInfo(1);
            string animatorNmae = monsterStats.animator.GetCurrentAnimatorClipInfo(1)[0].clip.name;

            monsterStats.agent.destination = monsterStats.transform.position;
            monsterStats.transform.LookAt(blackboard.target.transform);       // 怪物会莫名的旋转，故添加这行代码
            
            Debug.Log($"{animatorNmae}: {info.normalizedTime}");
            if (animatorNmae != animatorParameter)
            {
                return State.Running;
            }
            if (animatorNmae == animatorParameter && info.normalizedTime <= 0.95f)
            {
                return State.Running;
            }
            return State.Success;
        }
    }
}