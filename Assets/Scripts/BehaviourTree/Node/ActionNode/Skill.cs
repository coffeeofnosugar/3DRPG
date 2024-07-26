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
                if (monsterStats.CouldAttack(animatorParameter))
                {
                    _flag = true;
                    return;
                }

                blackboard.lastAttackTime[animatorParameter] = 0;
                _flag = false;
                monsterStats.animator.SetBool("Run", false);
                // ֹͣ�ƶ�
                monsterStats.agent.destination = monsterStats.transform.position;
                // ���򹥻�Ŀ��
                monsterStats.transform.LookAt(blackboard.target.transform);
                // ���Ź�������
                monsterStats.animator.SetTrigger(animatorParameter);
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
            var info = monsterStats.animator.GetCurrentAnimatorStateInfo(1);
            string animatorNmae = monsterStats.animator.GetCurrentAnimatorClipInfo(1)[0].clip.name;

            monsterStats.agent.destination = monsterStats.transform.position;
            monsterStats.transform.LookAt(blackboard.target.transform);       // �����Ī������ת����������д���
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