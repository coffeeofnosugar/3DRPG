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
        public PlayerInput PlayerInput;

        private void Awake()
        {
            PlayerInput = new PlayerInput();

            PlayerInput.CharacterControls.Move.started += onMoveInput;
            PlayerInput.CharacterControls.Move.canceled += onMoveInput;
            PlayerInput.CharacterControls.Move.performed += onMoveInput;

            PlayerInput.CharacterControls.Run.started += onRunInput;
            PlayerInput.CharacterControls.Run.canceled += onRunInput;

            PlayerInput.CharacterControls.Jump.started += onJumpInput;
            PlayerInput.CharacterControls.Jump.canceled += onJumpInput;
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
