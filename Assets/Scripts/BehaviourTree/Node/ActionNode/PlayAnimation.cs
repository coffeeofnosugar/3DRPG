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
                // ֹͣ�ƶ�
                characterStats.agent.destination = characterStats.transform.position;
                // ���򹥻�Ŀ��
                characterStats.transform.LookAt(blackboard.target.transform);
                // ���Ź�������
                characterStats.animator.SetTrigger(animatorParameter);
                // ����cd
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