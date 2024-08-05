using System;
using Sirenix.OdinInspector;
using Tools.CoffeeTools;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Player
{
    public class PlayerInputController : MonoBehaviour
    {
        public enum InputParameter { Movement, IsRun, IsJump }
        [Header("input parameter")]
        private PlayerInput _playerInput;
        private PlayerStats _playerStats;
        /// <summary>
        /// 移动input输入
        /// </summary>
        [ReadOnly, BoxGroup("移动"), LabelText("玩家操作输入")] public Vector2 inputMovement;
        
        /// <summary>
        /// 角色的移动方向，以世界为坐标系
        /// </summary>
        [ReadOnly, BoxGroup("移动"), LabelText("角色移动方向（世界坐标系）")] public Vector3 inputMovementWorld;
        
        /// <summary>
        /// 角色的移动方向，以模型为坐标系
        /// </summary>
        [ReadOnly, BoxGroup("移动"), LabelText("角色移动方向（角色坐标系）")] public Vector3 inputMovementPlayer;

        [ReadOnly] public Vector2 mouseDelta;
        [ReadOnly] public bool isRun;
        [ReadOnly] public bool isJump;
        [ReadOnly] public bool isCrouch;
        [ReadOnly] public bool isAttack;

        private void Awake()
        {
            _playerInput = new PlayerInput();
            _playerStats = GetComponent<PlayerStats>();

            _playerInput.InGame.Move.started += OnMoveInput;
            _playerInput.InGame.Move.canceled += OnMoveInput;
            _playerInput.InGame.Move.performed += OnMoveInput;
            
            _playerInput.InGame.Look.started += OnLookInput;
            _playerInput.InGame.Look.canceled += OnLookInput;
            _playerInput.InGame.Look.performed += OnLookInput;

            _playerInput.InGame.Run.started += OnRunInput;
            _playerInput.InGame.Run.canceled += OnRunInput;

            _playerInput.InGame.Jump.started += OnJumpInput;
            _playerInput.InGame.Jump.canceled += OnJumpInput;

            _playerInput.InGame.Crouch.started += OnCrouchInput;
            _playerInput.InGame.Crouch.canceled += OnCrouchInput;

            _playerInput.InGame.Attack.started += OnAttackInput;
            _playerInput.InGame.Attack.canceled += OnAttackInput;
        }

        private void Start()
        {
            Debugs.UpdateLogText["移动"] = "W/A/S/D";
            Debugs.UpdateLogText["跳跃/攀爬"] = "Space";
            Debugs.UpdateLogText["下蹲"] = "Ctrl";
        }

        private void Update()
        {
            var forward = _playerStats.cameraTransform.forward;
            Vector3 camForwardProjection = new Vector3(forward.x, 0, forward.z).normalized;
            // 玩家输入（世界向量）
            inputMovementWorld = camForwardProjection * inputMovement.y +
                                 _playerStats.cameraTransform.right * inputMovement.x;
            // 玩家输入（玩家相对向量）
            inputMovementPlayer = _playerStats.transform.InverseTransformVector(inputMovementWorld);
        }

        private void OnEnable() { _playerInput.InGame.Enable(); }

        private void OnDisable() { _playerInput.InGame.Disable(); }
        
        #region input event
        
        private void OnMoveInput(InputAction.CallbackContext obj) { inputMovement = obj.ReadValue<Vector2>(); }
        private void OnLookInput(InputAction.CallbackContext obj) { mouseDelta = obj.ReadValue<Vector2>(); }
        private void OnRunInput(InputAction.CallbackContext obj) { isRun = obj.ReadValueAsButton(); }
        private void OnJumpInput(InputAction.CallbackContext obj) { isJump = obj.ReadValueAsButton(); }
        private void OnCrouchInput(InputAction.CallbackContext obj) { isCrouch = obj.ReadValueAsButton(); }
        private void OnAttackInput(InputAction.CallbackContext obj) { isAttack = obj.ReadValueAsButton(); }

        #endregion
    }
}
