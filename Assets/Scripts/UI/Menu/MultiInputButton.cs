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

        /// <summary>
        /// 当被选中或取消选中时会触发该选项
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
        /// 简单限制一下触发事件的条件
        /// </summary>
        /// <param name="eventData"></param>
        public override void OnSubmit(BaseEventData eventData)
        {
            if (_menuSelectionHandler.AllowsSubmit())
                base.OnSubmit(eventData);
        }

        /// <summary>
        /// 选择当前按钮，
        /// <para>将<see cref="MenuSelectionHandler"/>的_currentSelection设置为当前按钮</para>
        /// <para>并且将EventSystem.current选择的目标也设置为当前按钮</para>
        /// </summary>
        public void UpdateSelected()
        {
            if (_menuSelectionHandler == null)
                _menuSelectionHandler = transform.root.gameObject.GetComponentInChildren<MenuSelectionHandler>();
            
            _menuSelectionHandler.UpdateSelection(this);
            
            EventSystem.current.SetSelectedGameObject(gameObject);
        }
        
        /// <summary>
        /// 选择当前按钮，并且更新其显示的文字
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