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
            if (hasSaveData)
                _continueButton.Select();
            else
                _NewGameButton.Select();
        }

        public void ContinueButton()
        {
            ContinueButtonAction.Invoke();
        }

        public void NewGameButton()
        {
            NewGameButtonAction.Invoke();
        }
        
        public void SettingsButton()
        {
            Debug.Log("Open Settings UI");
            SettingsButtonAction.Invoke();
        }

        public void CreditsButton()
        {
            Debug.Log("Open Credits UI");
            CreditsButtonAction.Invoke();
        }

        public void ExitButton()
        {
            Debug.Log("Open Exit UI");
            ExitButtonAction.Invoke();
        }
    }
}