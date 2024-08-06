using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// ע�����˵��ϰ�ť�ĵ���¼�
    /// </summary>
    public class UIMainMenu : MonoBehaviour
    {
        [SerializeField] private Button _continueButton = default;
        [SerializeField] private Button _NewGameButton = default;

        public UnityAction NewGameButtonAction;
        public UnityAction ContinueButtonAction;
        public UnityAction SettingsButtonAction;
        public UnityAction CreditsButtonAction;
        public UnityAction ExitButtonAction;

        /// <summary>
        /// �����Ƿ��д浵�����Ƽ�����Ϸ���صĿɲ����Ժ�Ĭ��ѡ��
        /// </summary>
        /// <param name="hasSaveData"></param>
        public void SetMenuScreen(bool hasSaveData)
        {
            _continueButton.interactable = hasSaveData;
            Debug.Log($"hasSaveData: {hasSaveData}");
            if (hasSaveData)
                _continueButton.Select();
            else
                _NewGameButton.Select();
        }

        public void NewGameButton()
        {
            Debug.Log("NewGame");
            NewGameButtonAction.Invoke();
        }

        public void ContinueButton()
        {
            Debug.Log("Continue");
            ContinueButtonAction.Invoke();
        }

        public void SettingsButton()
        {
            Debug.Log("Settings");
            SettingsButtonAction.Invoke();
        }

        public void CreditsButton()
        {
            Debug.Log("Credits");
            CreditsButtonAction.Invoke();
        }

        public void ExitButton()
        {
            Debug.Log("Exit");
            ExitButtonAction.Invoke();
        }
    }
}