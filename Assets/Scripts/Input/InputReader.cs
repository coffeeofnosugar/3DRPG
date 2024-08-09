using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [CreateAssetMenu(fileName = "InputReader", menuName = "Character/Player/InputReader")]
    public class InputReader : DescriptionBaseSO, PlayerInput.IInGameActions, PlayerInput.IUIActions
    {
        private PlayerInput _playerInput;

        // 将delegate分配给事件，可以使用空委托初始化他们
        // 当我们调用事件时，可以跳过null检查

        /// <summary>
        /// 左键是否点击下去了
        /// </summary>
        /// <returns></returns>
        public bool LeftMouseDown() => Mouse.current.leftButton.isPressed;
        
        #region InGame
        
        public event Action<Vector2> MoveEvent = delegate { };
        public event Action<Vector2> LookEvent = delegate { };
        public event Action StartRunEvent = delegate { };
        public event Action StopRunEvent = delegate { };
        public event Action JumpEvent = delegate { };
        public event Action JumpCanceledEvent = delegate { };
        public event Action StartCrouchEvent = delegate { };
        public event Action StopCrouchEvent = delegate { };
        public event Action AttackEvent = delegate { };
        public event Action AttackCanceledEvent = delegate { };
        public event Action PauseEvent = delegate { };
        
        #endregion

        #region UI
        
        /// <summary>
        /// UI控制: 键盘ESC、手柄B键和菜单键
        /// </summary>
        public event Action UICancelEvent = delegate { };
        /// <summary>
        /// UI控制: 鼠标移动时的位置坐标
        /// </summary>
        public event Action<Vector2> UIMousePointEvent = delegate { };
        /// <summary>
        /// UI控制: 键盘的 W/A/S/D/上/下/左/右、手柄的左摇杆/十字键
        /// </summary>
        public event Action<Vector2> UINavigateEvent = delegate { };
        
        
        #endregion

        private void OnEnable()
        {
            if (_playerInput == null)
            {
                _playerInput = new PlayerInput();

                _playerInput.InGame.SetCallbacks(this);
                _playerInput.UI.SetCallbacks(this);
            }
        }
        

        private void OnDisable()
        {
            DisableAllInput();
        }

        /// <summary>
        /// 启用游戏键位，关闭UI键位
        /// </summary>
        public void EnableInGameInput()
        {
            Debug.Log("Enable InGameInput");
            _playerInput.InGame.Enable();
            _playerInput.UI.Disable();
        }

        /// <summary>
        /// 启用UI键位，关闭游戏键位
        /// </summary>
        public void EnableUIInput()
        {
            Debug.Log("Enable UIInput");
            _playerInput.InGame.Disable();
            _playerInput.UI.Enable();
        }

        /// <summary>
        /// 关闭所有键位
        /// </summary>
        public void DisableAllInput()
        {
            _playerInput.InGame.Disable();
            _playerInput.UI.Disable();
        }

        #region InGame

        public void OnMove(InputAction.CallbackContext context)
        {
            MoveEvent.Invoke(context.ReadValue<Vector2>());
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            LookEvent.Invoke(context.ReadValue<Vector2>());
        }

        public void OnRun(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                StartRunEvent.Invoke();
            else if (context.phase == InputActionPhase.Canceled)
                StopRunEvent.Invoke();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                JumpEvent.Invoke();
            else if (context.phase == InputActionPhase.Canceled)
                JumpCanceledEvent.Invoke();
        }

        public void OnCrouch(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                StartCrouchEvent.Invoke();
            else if (context.phase == InputActionPhase.Canceled)
                StopCrouchEvent.Invoke();
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                AttackEvent.Invoke();
            else if (context.phase == InputActionPhase.Canceled)
                AttackCanceledEvent.Invoke();
        }

        public void OnPause(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                PauseEvent.Invoke();
            }
        }

        #endregion

        #region UI

        public void OnCancel(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                UICancelEvent.Invoke();
        }

        public void OnPoint(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                UIMousePointEvent.Invoke(context.ReadValue<Vector2>());
        }

        public void OnClick(InputAction.CallbackContext context)
        {
            
        }

        public void OnRightClick(InputAction.CallbackContext context)
        {
            
        }

        public void OnNavigate(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                UINavigateEvent.Invoke(context.ReadValue<Vector2>());
        }

        public void OnScrollWheel(InputAction.CallbackContext context)
        {
            
        }

        public void OnSubmit(InputAction.CallbackContext context)
        {
            
        }
        #endregion
    }
}
