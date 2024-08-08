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
            bool isConfirmation = false;            // �Ƿ���Ҫ������ť
            bool hasExitButton = false;             // �Ƿ�ӵ�йرմ��ڰ�ť
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
        
            if (isConfirmation) // ��Ҫ������ť����������
            {
                _popupButton1.gameObject.SetActive(true);
                _popupButton2.gameObject.SetActive(true);
        
                _popupButton1.onClick.AddListener(ConfirmButtonClicked);
                _popupButton2.onClick.AddListener(CancelButtonClicked);
            }
            else // ֻ��Ҫһ����ť����һ����Ϣ���� 
            {
                _popupButton1.gameObject.SetActive(true);
                _popupButton2.gameObject.SetActive(false);
        
                _popupButton1.onClick.AddListener(ConfirmButtonClicked);
            }
        

            if (hasExitButton) // �Ƿ��йرմ��ڰ�ť
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
