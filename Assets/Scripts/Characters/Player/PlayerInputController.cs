using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerController
{
    public class PlayerInputController
    {
        public Vector2 currentMovementInput;
        public bool isRun;
        public bool isJump;
        
        private PlayerInput _playerInput;

        public PlayerInputController()
        {
            _playerInput = new PlayerInput();

            _playerInput.CharacterControls.Move.started += onMoveInput;
            _playerInput.CharacterControls.Move.canceled += onMoveInput;
            _playerInput.CharacterControls.Move.performed += onMoveInput;

            _playerInput.CharacterControls.Run.started += onRunInput;
            _playerInput.CharacterControls.Run.canceled += onRunInput;

            _playerInput.CharacterControls.Jump.started += (obj) => { isJump = obj.ReadValueAsButton(); Debug.Log("Jump"); };

        }

        private void onRunInput(InputAction.CallbackContext obj)
        {
            isRun = obj.ReadValueAsButton();
        }

        private void onMoveInput(InputAction.CallbackContext obj)
        {
            currentMovementInput = obj.ReadValue<Vector2>();
            Debug.Log(currentMovementInput);
        }

        private void OnEnable()
        {
            _playerInput.CharacterControls.Enable();
        }

        private void OnDisable()
        {
            _playerInput.CharacterControls.Disable();
        }
    }
}
