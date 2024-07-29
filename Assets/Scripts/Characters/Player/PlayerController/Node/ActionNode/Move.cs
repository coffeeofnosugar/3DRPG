using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.PlayerController
{
    public class Move : ActionNode
    {
        [Header("�ڵ����")]
        [SerializeField] private float currentSpeed;
        // [SerializeField] private float coefficient = 20f;
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
            currentSpeed = playerStats.playerInputController.isRun ? playerStats.RunSpeed : playerStats.WalkSpeed;
            
            // ��ȡ�ƶ������������������ˮƽ�ϵ�ͶӰ
            Vector3 moveDirection =
                playerStats.cameraTransform.forward * playerStats.playerInputController.inputMovement.y + playerStats.cameraTransform.right * playerStats.playerInputController.inputMovement.x;
            moveDirection.y = 0;
            
            if (playerStats.playerInputController.inputMovement.y != 0 || playerStats.playerInputController.inputMovement.x != 0)
                playerStats.transform.eulerAngles = Quaternion.LookRotation(moveDirection).eulerAngles;
            
            // playerStats.rigidbody.AddForce(moveDirection.normalized * (currentSpeed * coefficient), ForceMode.Force);
        }
    }
}