using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerInputController : MonoBehaviour
    {
        public enum InputParameter { Movement, IsRun, IsJump }
        /// <summary>
        /// 移动输入
        /// </summary>
        [Header("input parameter")]
        public Vector2 currentMovementInput;
        public Vector2 mouseDelta;
        public bool isRun;
        public bool isJump;
        public bool isCrouch;
        /// <summary>
        /// 玩家移动方向
        /// </summary>
        public Vector3 playerMovement;
        private PlayerInput _playerInput;

        private void Awake()
        {
            _playerInput = new PlayerInput();

            _playerInput.CharacterControls.Move.started += OnMoveInput;
            _playerInput.CharacterControls.Move.canceled += OnMoveInput;
            _playerInput.CharacterControls.Move.performed += OnMoveInput;
            
            _playerInput.CharacterControls.Look.started += OnLookInput;
            _playerInput.CharacterControls.Look.canceled += OnLookInput;
            _playerInput.CharacterControls.Look.performed += OnLookInput;

            _playerInput.CharacterControls.Run.started += OnRunInput;
            _playerInput.CharacterControls.Run.canceled += OnRunInput;

            _playerInput.CharacterControls.Jump.started += OnJumpInput;
            _playerInput.CharacterControls.Jump.canceled += OnJumpInput;

            _playerInput.CharacterControls.Crouch.started += OnCrouchInput;
            _playerInput.CharacterControls.Crouch.canceled += OnCrouchInput;
        }

        private void OnEnable()
        {
            _playerInput.CharacterControls.Enable();
        }

        private void OnDisable()
        {
            _playerInput.CharacterControls.Disable();
        }
        
        #region input event
        
        private void OnMoveInput(InputAction.CallbackContext obj) { currentMovementInput = obj.ReadValue<Vector2>(); }

        private void OnLookInput(InputAction.CallbackContext obj) { mouseDelta = obj.ReadValue<Vector2>(); }
        private void OnRunInput(InputAction.CallbackContext obj) { isRun = obj.ReadValueAsButton(); }
        private void OnJumpInput(InputAction.CallbackContext obj) { isJump = obj.ReadValueAsButton(); }
        private void OnCrouchInput(InputAction.CallbackContext obj) { isCrouch = obj.ReadValueAsButton(); }
        #endregion
    }
}
