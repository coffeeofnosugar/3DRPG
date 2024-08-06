using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// 注册主菜单上按钮的点击事件
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
        /// 根据是否有存档，控制继续游戏开关的可操作性和默认选择
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