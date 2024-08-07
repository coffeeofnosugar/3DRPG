using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace UI
{
    
    /// <summary>
    /// 继承unity自带的Button，支持键盘和手柄输入
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
            Select();       // 由于重写了OnSelect，所以在使用这个方法之后会执行<see cref="MenuSelectionHandler.UpdateSelection"/>方法更新当前选择的Button
        }
        
        public void UpdateSelected(string tableEntryReference, bool isSelected = false)
        {
            _buttonText.StringReference.TableEntryReference = tableEntryReference;
            if (isSelected)
                Select();       // 由于重写了OnSelect，所以在使用这个方法之后会执行<see cref="MenuSelectionHandler.UpdateSelection"/>方法更新当前选择的Button
        }

        // public override void OnSubmit(BaseEventData eventData)
        // {
        //     if (_menuSelectionHandler.AllowsSubmit())
        //         base.OnSubmit(eventData);
        // }
    }
}