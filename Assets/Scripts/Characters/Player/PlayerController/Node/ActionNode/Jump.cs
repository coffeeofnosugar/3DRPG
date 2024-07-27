using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.PlayerController
{
    public class Jump : ActionNode
    {
        [Header("�ڵ����")]
        public float jumpForce = 14;
        public float landHigh = .3f;
        protected override void EnterState()
        {
            base.EnterState();
            // playerStats.rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        protected override State FixeUpdateState()
        {
            Ray _ray = new Ray(playerStats.transform.position, Vector3.down);
            if (Physics.Raycast(_ray, out RaycastHit _hit, landHigh))
            {
                AnimationClip clip = playerStats.animator.GetCurrentAnimatorClipInfo(0)[0].clip;
                if (clip.name.Contains("loop"))
                {
                    playerStats.animator.SetTrigger("isLand");
                    state = State.Success;
                }
            }
            else
            {
                state = State.Running;
            }
            return state;
        }
    }
}