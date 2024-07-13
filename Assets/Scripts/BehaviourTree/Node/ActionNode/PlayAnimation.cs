using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class PlayAnimation : ActionNode
    {
        public string animatorParameter;

        // public bool isCustomizeParameter;
        // public float cusCoolDown;
        private float _lastPlayTime;
        private bool _flag;
        protected override void OnStart()
        {
            if (blackboard.target)
            {
                if (Time.time - _lastPlayTime <= characterStats.skillDict[animatorParameter].coolDown)
                {
                    _flag = true;
                    return;
                }
                
                _flag = false;
                _lastPlayTime = Time.time;
                characterStats.animator.SetBool("Run", false);
                // ֹͣ�ƶ�
                characterStats.agent.destination = characterStats.transform.position;
                // ���򹥻�Ŀ��
                characterStats.transform.LookAt(blackboard.target.transform);
                // ���Ź�������
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