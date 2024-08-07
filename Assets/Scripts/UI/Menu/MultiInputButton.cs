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
            _menuSelectionHandler.UpdateSelection(gameObject);
            SelectThisButton.Invoke(true);
            base.OnSelect(eventData);
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            SelectThisButton.Invoke(false);
            base.OnDeselect(eventData);
        }

        public void UpdateSelected()
        {
            Select();       // ������д��OnSelect��������ʹ���������֮���ִ��<see cref="MenuSelectionHandler.UpdateSelection"/>�������µ�ǰѡ���Button
        }
        
        public void UpdateSelected(string tableEntryReference, bool isSelected = false)
        {
            _buttonText.StringReference.TableEntryReference = tableEntryReference;
            if (isSelected)
                Select();       // ������д��OnSelect��������ʹ���������֮���ִ��<see cref="MenuSelectionHandler.UpdateSelection"/>�������µ�ǰѡ���Button
        }

        // public override void OnSubmit(BaseEventData eventData)
        // {
        //     if (_menuSelectionHandler.AllowsSubmit())
        //         base.OnSubmit(eventData);
        // }
    }
}