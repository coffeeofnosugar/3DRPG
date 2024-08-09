using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace UI
{
    
    /// <summary>
    /// �̳�unity�Դ���Button��֧�ּ��̺��ֱ�����
    /// </summary>
    [AddComponentMenu("UOP1/UI/MultiInputButton")]
    public class MultiInputButton : Button
    {
        public LocalizeStringEvent _buttonText;
        private MenuSelectionHandler _menuSelectionHandler;

        /// <summary>
        /// ����ѡ�л�ȡ��ѡ��ʱ�ᴥ����ѡ��
        /// </summary>
        public Action<bool> SelectThisButton = delegate(bool b) { };

        private new void Awake()
        {
            _menuSelectionHandler = transform.root.gameObject.GetComponentInChildren<MenuSelectionHandler>();
            _buttonText = transform.GetChild(transform.childCount - 1).GetComponent<LocalizeStringEvent>();
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            _menuSelectionHandler.HandleMouseEnter(gameObject);
            base.OnPointerEnter(eventData);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            _menuSelectionHandler.HandleMouseExit(gameObject);
            base.OnPointerExit(eventData);
        }

        public override void OnSelect(BaseEventData eventData)
        {
            _menuSelectionHandler.UpdateSelection(this);
            
            SelectThisButton.Invoke(true);
            base.OnSelect(eventData);
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            SelectThisButton.Invoke(false);
            base.OnDeselect(eventData);
        }

        /// <summary>
        /// ������һ�´����¼�������
        /// </summary>
        /// <param name="eventData"></param>
        public override void OnSubmit(BaseEventData eventData)
        {
            if (_menuSelectionHandler.AllowsSubmit())
                base.OnSubmit(eventData);
        }

        /// <summary>
        /// ѡ��ǰ��ť��
        /// <para>��<see cref="MenuSelectionHandler"/>��_currentSelection����Ϊ��ǰ��ť</para>
        /// <para>���ҽ�EventSystem.currentѡ���Ŀ��Ҳ����Ϊ��ǰ��ť</para>
        /// </summary>
        public void UpdateSelected()
        {
            if (_menuSelectionHandler == null)
                _menuSelectionHandler = transform.root.gameObject.GetComponentInChildren<MenuSelectionHandler>();
            
            _menuSelectionHandler.UpdateSelection(this);
            
            EventSystem.current.SetSelectedGameObject(gameObject);
        }
        
        /// <summary>
        /// ѡ��ǰ��ť�����Ҹ�������ʾ������
        /// </summary>
        /// <param name="tableEntryReference"></param>
        /// <param name="isSelected"></param>
        public void UpdateSelected(string tableEntryReference, bool isSelected = false)
        {
            if (_buttonText == null)
                _buttonText = transform.GetChild(transform.childCount - 1).GetComponent<LocalizeStringEvent>();
            
            _buttonText.StringReference.TableEntryReference = tableEntryReference;
            
            if (isSelected)
                UpdateSelected();
        }
    }
}