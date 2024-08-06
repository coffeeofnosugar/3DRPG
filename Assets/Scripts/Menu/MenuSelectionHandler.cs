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
            
            UpdateSelection(_defaultSelection);     // ����Ĭ��ѡ��
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
            Cursor.visible = true;      // ��ʾ���
            
            if (_mouseSelection != null)
                EventSystem.current.SetSelectedGameObject(_mouseSelection);
        }

        /// <summary>
        /// ʹ�ü��̵� W/A/S/D/��/��/��/�� �� �ֱ�����ҡ��/ʮ�ּ� ʱ�����¼�
        /// </summary>
        /// <param name="navigate"></param>
        private void HandleNavigate(Vector2 navigate)
        {
            Cursor.visible = false;     // �������

            if (EventSystem.current.currentSelectedGameObject == null)
                EventSystem.current.SetSelectedGameObject(_currentSelection);
        }

        /// <summary>
        /// ���ֱ�����̵������봥��
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
