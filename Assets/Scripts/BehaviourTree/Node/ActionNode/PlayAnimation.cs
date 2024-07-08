using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class PlayAnimation : ActionNode
    {
        public string animatorParameter;
        private bool flag = false;
        protected override void OnStart()
        {
            if (blackboard.target) //  && characterStats.LastAttackTime >= characterStats.CoolDown)
            {
                characterStats.animator.SetBool("Run", false);
                // 停止移动
                characterStats.agent.destination = characterStats.transform.position;
                // 朝向攻击目标
                characterStats.transform.LookAt(blackboard.target.transform);
                // 播放攻击动画
                characterStats.animator.SetTrigger(animatorParameter);
                // 重置cd
                //characterStats.LastAttackTime = 0;
                flag = true;
            }
        }

        protected override void OnStop()
        {
            flag = false;
        }

        protected override State OnUpdate()
        {
            var info = characterStats.animator.GetCurrentAnimatorStateInfo(1);
            string animatorNmae = characterStats.animator.GetCurrentAnimatorClipInfo(1)[0].clip.name;

            characterStats.agent.destination = characterStats.transform.position;
            if (flag)
            {
                flag = false;
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