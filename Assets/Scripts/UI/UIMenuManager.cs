using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
	public class UIMenuManager : MonoBehaviour
	{
		[SerializeField] private UIPopup _popupPanel = default;
		[SerializeField] private UISettingsController _settingsPanel = default;
		[SerializeField] private UICredits _creditsPanel = default;
		[SerializeField] private UIMainMenu _mainMenuPanel = default;

		// [SerializeField] private SaveSystem _saveSystem = default;

		[SerializeField] private Player.InputReader _inputReader = default;


		// [Header("Broadcasting on")]
		// [SerializeField]
		// private VoidEventChannelSO _startNewGameEvent = default;
		// [SerializeField]
		// private VoidEventChannelSO _continueGameEvent = default;



		private bool _hasSaveData = false;

		private void Start()
		{
			_inputReader.EnableUIInput();
			SetMenuScreen();
		}

		void SetMenuScreen()
		{
			// _hasSaveData = _saveSystem.LoadSaveDataFromDisk();
			_mainMenuPanel.SetMenuScreen(_hasSaveData);
			// _mainMenuPanel.ContinueButtonAction += _continueGameEvent.RaiseEvent;
			// _mainMenuPanel.NewGameButtonAction += ButtonStartNewGameClicked;
			_mainMenuPanel.SettingsButtonAction += OpenSettingsScreen;
			_mainMenuPanel.CreditsButtonAction += OpenCreditsScreen;
			_mainMenuPanel.ExitButtonAction += ShowExitConfirmationPopup;
		}

		// void ButtonStartNewGameClicked()
		// {
		// 	if (!_hasSaveData)
		// 	{
		// 		ConfirmStartNewGame();
		// 	}
		// 	else
		// 	{
		// 		ShowStartNewGameConfirmationPopup();
		// 	}
		// }

		// void ConfirmStartNewGame()
		// {
		// 	_startNewGameEvent.RaiseEvent();
		// }

		// void ShowStartNewGameConfirmationPopup()
		// {
		// 	_popupPanel.ConfirmationResponseAction += StartNewGamePopupResponse;
		// 	_popupPanel.ClosePopupAction += HidePopup;
		//
		// 	_popupPanel.gameObject.SetActive(true);
		// 	_popupPanel.SetPopup(PopupType.NewGame);
		// }

		// void StartNewGamePopupResponse(bool startNewGameConfirmed)
		// {
		//
		// 	_popupPanel.ConfirmationResponseAction -= StartNewGamePopupResponse;
		// 	_popupPanel.ClosePopupAction -= HidePopup;
		//
		// 	_popupPanel.gameObject.SetActive(false);
		//
		// 	if (startNewGameConfirmed)
		// 	{
		// 		ConfirmStartNewGame();
		// 	}
		// 	else
		// 	{
		// 		// _continueGameEvent.RaiseEvent();
		// 	}
		//
		// 	_mainMenuPanel.SetMenuScreen(_hasSaveData);
		// }

		void HidePopup()
		{
			_popupPanel.ClosePopupAction -= HidePopup;
			_popupPanel.gameObject.SetActive(false);
			_mainMenuPanel.SetMenuScreen(_hasSaveData);
		}

		public void OpenSettingsScreen()
		{
			_settingsPanel.gameObject.SetActive(true);
			_settingsPanel.Closed += CloseSettingsScreen;
		}
		
		public void CloseSettingsScreen()
		{
			_settingsPanel.Closed -= CloseSettingsScreen;
			_settingsPanel.gameObject.SetActive(false);
			_mainMenuPanel.SetMenuScreen(_hasSaveData);
		}
		
		public void OpenCreditsScreen()
		{
			_creditsPanel.gameObject.SetActive(true);
			_creditsPanel.Closed += CloseCreditsScreen;
		}
		public void CloseCreditsScreen()
		{
			_creditsPanel.Closed -= CloseCreditsScreen;
			_creditsPanel.gameObject.SetActive(false);
			_mainMenuPanel.SetMenuScreen(_hasSaveData);
		}

		/// <summary>
		/// 展示退出弹窗提示界面
		/// </summary>
		private void ShowExitConfirmationPopup()
		{
			_popupPanel.ConfirmationResponseAction += HideExitConfirmationPopup;
			_popupPanel.gameObject.SetActive(true);
			_popupPanel.SetPopup(PopupType.Quit);
		}
		
		/// <summary>
		/// 做出选择后调用的事件
		/// </summary>
		/// <param name="quitConfirmed"></param>
		private void HideExitConfirmationPopup(bool quitConfirmed)
		{
			_popupPanel.ConfirmationResponseAction -= HideExitConfirmationPopup;
			_popupPanel.gameObject.SetActive(false);
			if (quitConfirmed)
			{
#if UNITY_EDITOR
				UnityEditor.EditorApplication.isPlaying = false;
#else
				Application.Quit();
#endif
			}
			_mainMenuPanel.SetMenuScreen(_hasSaveData);		// 重置箭头指针
		}
		private void OnDestroy()
		{
			_popupPanel.ConfirmationResponseAction -= HideExitConfirmationPopup;
			// _popupPanel.ConfirmationResponseAction -= StartNewGamePopupResponse;
		}
	}
}
