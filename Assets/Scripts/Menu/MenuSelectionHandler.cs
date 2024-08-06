using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class MenuSelectionHandler : MonoBehaviour
    {
        [SerializeField] private InputReader _inputReader;
        [SerializeField] private GameObject _defaultSelection;
        [SerializeField, ReadOnly] private GameObject _currentSelection;
        [SerializeField, ReadOnly] private GameObject _mouseSelection;

        private void OnEnable()
        {
            _inputReader.UIMousePointEvent += HandleMousePoint;
            _inputReader.UINavigateEvent += HandleNavigate;
            
            UpdateSelection(_defaultSelection);     // 设置默认选择
        }
    
        private void OnDisable()
        {
            _inputReader.UIMousePointEvent -= HandleMousePoint;
            _inputReader.UINavigateEvent -= HandleNavigate;
        }

        private void Update()
        {
            if ((EventSystem.current != null) && (EventSystem.current.currentSelectedGameObject == null) && (_currentSelection != null))
                EventSystem.current.SetSelectedGameObject(_currentSelection);
        }

        public void Unselect()
        {
            _currentSelection = null;
            if (EventSystem.current != null)
                EventSystem.current.SetSelectedGameObject(null);
        }

        public void HandleMouseEnter(GameObject UIElement)
        {
            _mouseSelection = UIElement;
            EventSystem.current.SetSelectedGameObject(UIElement);
        }

        public void HandleMouseExit(GameObject UIElement)
        {
            _mouseSelection = null;
            EventSystem.current.SetSelectedGameObject(_currentSelection);
        }

        private void HandleMousePoint(Vector2 point)
        {
            Cursor.visible = true;      // 显示鼠标
            
            if (_mouseSelection != null)
                EventSystem.current.SetSelectedGameObject(_mouseSelection);
        }

        /// <summary>
        /// 使用键盘的 W/A/S/D/上/下/左/右 或 手柄的左摇杆/十字键 时触发事件
        /// </summary>
        /// <param name="navigate"></param>
        private void HandleNavigate(Vector2 navigate)
        {
            Cursor.visible = false;     // 隐藏鼠标

            if (EventSystem.current.currentSelectedGameObject == null)
                EventSystem.current.SetSelectedGameObject(_currentSelection);
        }

        /// <summary>
        /// 由手柄或键盘导航输入触发
        /// </summary>
        /// <param name="UIElement"></param>
        public void UpdateSelection(GameObject UIElement)
        {
            if (UIElement.TryGetComponent<MultiInputSelectableElement>(out var e))
            {
                _mouseSelection = UIElement;
                _currentSelection = UIElement;
            }
        }
    }
}
