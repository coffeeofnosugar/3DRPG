using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Player.PlayerController;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerInputController : MonoBehaviour
    {
        public enum InputParameter { Movement, IsRun, IsJump }
        [Header("input parameter")]
        public Vector2 currentMovementInput;
        public bool isRun;
        public bool isJump;
        
        private Rigidbody _rigidbody;
        private CharacterStats _characterStats;
        private Transform _cameraTransform;
        
        public PlayerInput PlayerInput;
        

        private void Awake()
        {
            PlayerInput = new PlayerInput();
            _rigidbody = GetComponent<Rigidbody>();
            _characterStats = GetComponent<CharacterStats>();
            _cameraTransform = Camera.main.transform;

            PlayerInput.CharacterControls.Move.started += onMoveInput;
            PlayerInput.CharacterControls.Move.canceled += onMoveInput;
            PlayerInput.CharacterControls.Move.performed += onMoveInput;

            PlayerInput.CharacterControls.Run.started += onRunInput;
            PlayerInput.CharacterControls.Run.canceled += onRunInput;

            PlayerInput.CharacterControls.Jump.started += onJumpInput;
            PlayerInput.CharacterControls.Jump.canceled += onJumpInput;
        }
        
        private void Update()
        {
            Debugs.Instance["Speed"] = _rigidbody.velocity.magnitude.ToString("f2");
        }

        #region input event
        private void onRunInput(InputAction.CallbackContext obj)
        {
            isRun = obj.ReadValueAsButton();
        }

        private void onMoveInput(InputAction.CallbackContext obj)
        {
            currentMovementInput = obj.ReadValue<Vector2>();
        }

        private void onJumpInput(InputAction.CallbackContext obj)
        {
            isJump = obj.ReadValueAsButton();
        }

        private void OnEnable()
        {
            PlayerInput.CharacterControls.Enable();
        }

        private void OnDisable()
        {
            PlayerInput.CharacterControls.Disable();
        }
        #endregion
    }
}
