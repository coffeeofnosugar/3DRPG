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
        [Header("input parameter")]
        public Vector2 currentMovementInput;
        public bool isRun;
        public bool isJump;
        
        private Rigidbody _rigidbody;
        private CharacterStats _characterStats;
        private Transform _cameraTransform;

        [Header("control parameter")]
        [SerializeField] private float walkSpeed;
        [SerializeField] private float runSpeed;
        [SerializeField] private float currentSpeed;
        
        private PlayerInput _playerInput;
        

        private void Awake()
        {
            _playerInput = new PlayerInput();
            _rigidbody = GetComponent<Rigidbody>();
            _characterStats = GetComponent<CharacterStats>();
            _cameraTransform = Camera.main.transform;

            _playerInput.CharacterControls.Move.started += onMoveInput;
            _playerInput.CharacterControls.Move.canceled += onMoveInput;
            _playerInput.CharacterControls.Move.performed += onMoveInput;

            _playerInput.CharacterControls.Run.started += onRunInput;
            _playerInput.CharacterControls.Run.canceled += onRunInput;

            _playerInput.CharacterControls.Jump.started += onJumpInput;
        }

        private void Start()
        {
            walkSpeed = _characterStats.WalkSpeed;
            runSpeed = _characterStats.RunSpeed;
            currentSpeed = walkSpeed;
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
            _playerInput.CharacterControls.Enable();
        }

        private void OnDisable()
        {
            _playerInput.CharacterControls.Disable();
        }
        #endregion

        private void FixedUpdate()
        {
            MovePlayer();
        }

        private void Update()
        {
            Debugs.Instance["Speed"] = _rigidbody.velocity.magnitude.ToString("f2");
        }

        private void MovePlayer()
        {
            currentSpeed = isRun ? runSpeed : walkSpeed;
            
            // 获取移动方向向量——相机在水平上的投影
            Vector3 moveDirection =
                _cameraTransform.forward * currentMovementInput.y + _cameraTransform.right * currentMovementInput.x;
            moveDirection.y = 0;

            if (currentMovementInput.y != 0 || currentMovementInput.x != 0)
                transform.eulerAngles = Quaternion.LookRotation(moveDirection).eulerAngles;
            
            _rigidbody.AddForce(moveDirection.normalized * (currentSpeed * 10f), ForceMode.Force);
        }
    }
}
