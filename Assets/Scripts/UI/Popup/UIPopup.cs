using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Serialization;

namespace UI
{
    public enum PopupType { Quit, NewGame, BackToMenu, }

    public class UIPopup : MonoBehaviour
    {
        [SerializeField] private LocalizeStringEvent _titleText = default;
        [SerializeField] private LocalizeStringEvent _descriptionText = default;
        [SerializeField] private MultiInputButton _popupButton1 = default;
        [SerializeField] private MultiInputButton _popupButton2 = default;
        [SerializeField] private MultiInputButton _closeButton = default;
        [SerializeField] private Player.InputReader _inputReader = default;

        private PopupType _actualType;

        public event UnityAction<bool> ConfirmationResponseAction;
        public event UnityAction ClosePopupAction;

        private void OnDisable()
        {
            _popupButton1.onClick.RemoveListener(ConfirmButtonClicked);
            _popupButton2.onClick.RemoveListener(CancelButtonClicked);
            _inputReader.UICancelEvent -= ClosePopupButtonClicked;
        }
        
        public void SetPopup(PopupType popupType)
        {
            _actualType = popupType;
            bool isConfirmation = false;            // 是否需要两个按钮
            bool hasExitButton = false;             // 是否拥有关闭窗口按钮
            _titleText.StringReference.TableEntryReference = _actualType.ToString() + "_Popup_Title";
            _descriptionText.StringReference.TableEntryReference = _actualType.ToString() + "_Popup_Description";
            string tableEntryReferenceConfirm =  _actualType + "_Confirm";
            string tableEntryReferenceCancel = _actualType + "_Cancel" ;
            switch (_actualType)
            {
                case PopupType.NewGame:
                case PopupType.BackToMenu:
                    isConfirmation = true;
                    _popupButton1.UpdateSelected(tableEntryReferenceConfirm, true);
                    _popupButton2.UpdateSelected(tableEntryReferenceCancel, false);
                    hasExitButton = true;
                    break;
                case PopupType.Quit:
                    isConfirmation = true;
                    _popupButton1.UpdateSelected(tableEntryReferenceConfirm, true);
                    _popupButton2.UpdateSelected(tableEntryReferenceCancel, false);
                    hasExitButton = false;
                    break;
                default:
                    isConfirmation = false;
                    hasExitButton = false;
                    break;
            }
        
            if (isConfirmation) // 需要两个按钮：做出决定
            {
                _popupButton1.gameObject.SetActive(true);
                _popupButton2.gameObject.SetActive(true);
        
                _popupButton1.onClick.AddListener(ConfirmButtonClicked);
                _popupButton2.onClick.AddListener(CancelButtonClicked);
            }
            else // 只需要一个按钮：是一个信息窗口 
            {
                _popupButton1.gameObject.SetActive(true);
                _popupButton2.gameObject.SetActive(false);
        
                _popupButton1.onClick.AddListener(ConfirmButtonClicked);
            }
        

            if (hasExitButton) // 是否有关闭窗口按钮
            {
                _closeButton.gameObject.SetActive(true);
                _inputReader.UICancelEvent += ClosePopupButtonClicked;
            }
        }
        
        private void ConfirmButtonClicked() { ConfirmationResponseAction.Invoke(true); }

        private void CancelButtonClicked() { ConfirmationResponseAction.Invoke(false); }

        private void ClosePopupButtonClicked() { ClosePopupAction.Invoke(); }
    }
}
