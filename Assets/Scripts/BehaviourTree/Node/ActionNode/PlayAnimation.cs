using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class PlayAnimation : ActionNode
    {
        public string animatorParameter;
        protected override void OnStart()
        {
            if (blackboard.target && characterStats.LastAttackTime >= characterStats.CoolDown)
            {
                characterStats.animator.SetBool("Run", false);
                // ֹͣ�ƶ�
                characterStats.agent.destination = characterStats.transform.position;
                // ���򹥻�Ŀ��
                characterStats.transform.LookAt(blackboard.target.transform);
                // ���Ź�������
                characterStats.animator.SetTrigger(animatorParameter);
                // ����cd
                characterStats.LastAttackTime = 0;
            }
        }

        protected override void OnStop()
        {

        }

        protected override State OnUpdate()
        {
            var info = characterStats.animator.GetCurrentAnimatorStateInfo(1);
            if (info.normalizedTime >= 0.95f)
            {
                return State.Success;
            }
            return State.Running;
        }
    }
}