using Player;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    /// <summary>
    /// ������Ҫ��Ϊ����ʾ��ǰѡ������ĸ���ť����û��ֱ�Ӳ��뵽ѡ��ť��
    /// </summary>
    public class MenuSelectionHandler : MonoBehaviour
    {
        [SerializeField] private InputReader _inputReader;
        [SerializeField, ReadOnly] private GameObject _currentSelection;
        [SerializeField, ReadOnly] private GameObject _mouseSelection;

        private void OnEnable()
        {
            _inputReader.UIMousePointEvent += HandleMousePoint;
            _inputReader.UINavigateEvent += HandleNavigate;
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
        
        // ��֪����ɶ��
        // public void Unselect()
        // {
        //     _currentSelection = null;
        //     if (EventSystem.current != null)
        //         EventSystem.current.SetSelectedGameObject(null);
        // }

        public void HandleMouseEnter(GameObject UIElement)
        {
            if (UIElement.TryGetComponent<MultiInputButton>(out var button) && button.interactable)
            {
                _mouseSelection = UIElement.gameObject;
                EventSystem.current.SetSelectedGameObject(_mouseSelection);
            }
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
            if (UIElement.TryGetComponent<MultiInputButton>(out var button))
            {
                _currentSelection = UIElement;
                _mouseSelection = UIElement;
            }
        }
    }
}
