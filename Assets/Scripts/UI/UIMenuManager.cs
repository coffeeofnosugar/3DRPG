using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
	public class UIMenuManager : MonoBehaviour
	{
		[Title("Start Initialized")]	// ��ʼ��
		[SerializeField, InlineEditor] private VoidEventChannelSO _startInitializer;
		
		[Title("UI Controller")]
		[SerializeField, BoxGroup("UI Controller")] private UIMainMenu _mainMenuPanel = default;				// ������ѡ��
		[SerializeField, BoxGroup("UI Controller")] private UISettingsController _settingsPanel = default;		// ���ý���
		[SerializeField, BoxGroup("UI Controller")] private UICredits _creditsPanel = default;					// ������Ա����
		[SerializeField, BoxGroup("UI Controller")] private UIPopup _popupPanel = default;						// �˳���������

		// [SerializeField] private SaveSystem _saveSystem = default;

		[Title("Input ScriptableObject")]
		[SerializeField, InlineEditor] private Player.InputReader _inputReader = default;			// �������
		
		[Title("Broadcasting on")]
		// [SerializeField] private VoidEventChannelSO _continueGameEvent = default;
		[SerializeField, InlineEditor] private VoidEventChannelSO _startNewGameTrigger = default;



		private bool _hasSaveData = false;

		private void Awake()
		{
			_startInitializer.OnEventRaised += InitializedMainMenu;
		}

		private void InitializedMainMenu()
		{
			_inputReader.EnableUIInput();
			SetMenuScreen();
			_startInitializer.OnEventRaised -= InitializedMainMenu;
		}


		void SetMenuScreen()
		{
			// _hasSaveData = _saveSystem.LoadSaveDataFromDisk();
			_mainMenuPanel.SetMenuScreen(_hasSaveData);
			_mainMenuPanel.SettingsButtonAction += OpenSettingsScreen;
			_mainMenuPanel.CreditsButtonAction += OpenCreditsScreen;
			_mainMenuPanel.ExitButtonAction += ShowExitConfirmationPopup;
			// _mainMenuPanel.ContinueButtonAction += _continueGameEvent.RaiseEvent;
			_mainMenuPanel.NewGameButtonAction += ButtonStartNewGameClicked;
		}

		void ButtonStartNewGameClicked()
		{
			if (!_hasSaveData)
			{
				ConfirmStartNewGame();
			}
			else
			{
				// ShowStartNewGameConfirmationPopup();
			}
		}

		void ConfirmStartNewGame()
		{
			_startNewGameTrigger.RaiseEvent();
		}

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
		/// չʾ�˳�������ʾ����
		/// </summary>
		private void ShowExitConfirmationPopup()
		{
			_popupPanel.ConfirmationResponseAction += HideExitConfirmationPopup;
			_popupPanel.gameObject.SetActive(true);
			_popupPanel.SetPopup(PopupType.Quit);
		}
		
		/// <summary>
		/// ����ѡ�����õ��¼�
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
			_mainMenuPanel.SetMenuScreen(_hasSaveData);		// ���ü�ͷָ��
		}
		private void OnDestroy()
		{
			_popupPanel.ConfirmationResponseAction -= HideExitConfirmationPopup;
			// _popupPanel.ConfirmationResponseAction -= StartNewGamePopupResponse;
		}
	}
}
