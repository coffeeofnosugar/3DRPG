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
            base.OnSelect(eventData);
        }

        public void UpdateSelected()
        {
            _menuSelectionHandler.UpdateSelection(gameObject);
        }
        
        public void UpdateSelected(string tableEntryReference, bool isSelected = false)
        {
            _buttonText.StringReference.TableEntryReference = tableEntryReference;
            if (isSelected)
                Select();
        }

        // public override void OnSubmit(BaseEventData eventData)
        // {
        //     if (_menuSelectionHandler.AllowsSubmit())
        //         base.OnSubmit(eventData);
        // }
    }
}