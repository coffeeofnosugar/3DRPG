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

        // ��delegate������¼�������ʹ�ÿ�ί�г�ʼ������
        // �����ǵ����¼�ʱ����������null���

        /// <summary>
        /// ����Ƿ�����ȥ��
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
        /// UI����: ����ESC���ֱ�B���Ͳ˵���
        /// </summary>
        public event Action UICancelEvent = delegate { };
        /// <summary>
        /// UI����: ����ƶ�ʱ��λ������
        /// </summary>
        public event Action<Vector2> UIMousePointEvent = delegate { };
        /// <summary>
        /// UI����: ���̵� W/A/S/D/��/��/��/�ҡ��ֱ�����ҡ��/ʮ�ּ�
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
        /// ������Ϸ��λ���ر�UI��λ
        /// </summary>
        public void EnableInGameInput()
        {
            Debug.Log("Enable InGameInput");
            _playerInput.InGame.Enable();
            _playerInput.UI.Disable();
        }

        /// <summary>
        /// ����UI��λ���ر���Ϸ��λ
        /// </summary>
        public void EnableUIInput()
        {
            Debug.Log("Enable UIInput");
            _playerInput.InGame.Disable();
            _playerInput.UI.Enable();
        }

        /// <summary>
        /// �ر����м�λ
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
