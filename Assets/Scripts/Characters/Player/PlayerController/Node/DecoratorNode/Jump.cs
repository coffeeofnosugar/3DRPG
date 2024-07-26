using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.PlayerController
{
    public class Jump : DecoratorNode
    {
        [Header("节点参数")]
        public float jumpForce = 14;
        public float landHigh = .3f;
        protected override void EnterState()
        {
            base.EnterState();
            _characterStats.rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        protected override State FixeUpdateState()
        {
            Ray _ray = new Ray(_characterStats.transform.position, Vector3.down);
            if (Physics.Raycast(_ray, out RaycastHit _hit, landHigh))
            {
                AnimationClip clip = _characterStats.animator.GetCurrentAnimatorClipInfo(0)[0].clip;
                if (clip.name.Contains("loop"))
                {
                    _characterStats.animator.SetTrigger("isLand");
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