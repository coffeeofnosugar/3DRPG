using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.PlayerController
{
    public class Move : ActionNode
    {
        [Header("�ڵ����")]
        [SerializeField] private float currentSpeed;
        [SerializeField] private float coefficient = 20f;
        protected override void EnterState()
        {
            base.EnterState();
        }

        protected override void ExitState()
        {
            base.ExitState();
        }

        protected override State FixeUpdateState()
        {
            MovePlayer();
            if (child)
            {
                child.state = state;
                child.FixedUpdate();
            }
            return state;
        }

        private void MovePlayer()
        {
            // Debug.Log(_characterStats.PlayerInputController.isRun);
            currentSpeed = _characterStats.PlayerInputController.isRun ? _characterStats.RunSpeed : _characterStats.WalkSpeed;
            
            // ��ȡ�ƶ������������������ˮƽ�ϵ�ͶӰ
            Vector3 moveDirection =
                _characterStats.cameraTransform.forward * _characterStats.PlayerInputController.currentMovementInput.y + _characterStats.cameraTransform.right * _characterStats.PlayerInputController.currentMovementInput.x;
            moveDirection.y = 0;
            
            if (_characterStats.PlayerInputController.currentMovementInput.y != 0 || _characterStats.PlayerInputController.currentMovementInput.x != 0)
                _characterStats.transform.eulerAngles = Quaternion.LookRotation(moveDirection).eulerAngles;
            
            _characterStats.rigidbody.AddForce(moveDirection.normalized * (currentSpeed * coefficient), ForceMode.Force);
        }
    }
}