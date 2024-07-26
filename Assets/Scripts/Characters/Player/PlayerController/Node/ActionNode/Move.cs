using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.PlayerController
{
    public class Move : ActionNode
    {
        [Header("节点参数")]
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
            currentSpeed = playerStats.PlayerInputController.isRun ? playerStats.RunSpeed : playerStats.WalkSpeed;
            
            // 获取移动方向向量——相机在水平上的投影
            Vector3 moveDirection =
                playerStats.cameraTransform.forward * playerStats.PlayerInputController.currentMovementInput.y + playerStats.cameraTransform.right * playerStats.PlayerInputController.currentMovementInput.x;
            moveDirection.y = 0;
            
            if (playerStats.PlayerInputController.currentMovementInput.y != 0 || playerStats.PlayerInputController.currentMovementInput.x != 0)
                playerStats.transform.eulerAngles = Quaternion.LookRotation(moveDirection).eulerAngles;
            
            playerStats.rigidbody.AddForce(moveDirection.normalized * (currentSpeed * coefficient), ForceMode.Force);
        }
    }
}