using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class Skill : ActionNode
    {
        public string animatorParameter;

        private bool _flag;
        protected override void OnStart()
        {
            if (blackboard.target)
            {
                if (blackboard.lastAttackTime[animatorParameter] <= characterStats.skillDict[animatorParameter].coolDown
                    || (characterStats.transform.position - blackboard.target.transform.position).sqrMagnitude >= characterStats.skillDict[animatorParameter].attackRangeSqr)
                {
                    _flag = true;
                    return;
                }

                blackboard.lastAttackTime[animatorParameter] = 0;
                _flag = false;
                // characterStats.animator.SetBool("Run", false);
                // 停止移动
                characterStats.agent.destination = characterStats.transform.position;
                // 朝向攻击目标
                characterStats.transform.LookAt(blackboard.target.transform);
                // 播放攻击动画
                characterStats.animator.SetTrigger(animatorParameter);
            }
        }

        protected override void OnStop()
        {
            
        }

        protected override State OnUpdate()
        {
            if (_flag)
            {
                return State.Success;
            }
            var info = characterStats.animator.GetCurrentAnimatorStateInfo(1);
            string animatorNmae = characterStats.animator.GetCurrentAnimatorClipInfo(1)[0].clip.name;

            characterStats.agent.destination = characterStats.transform.position;
            characterStats.transform.LookAt(blackboard.target.transform);       // 怪物会莫名的旋转，故添加这行代码
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