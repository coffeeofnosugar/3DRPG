//-----------------------------------------------------------------------
// <summary>
// 文件名: StartGameController
// 描述: 控制开始游戏
// 作者: #AUTHOR#
// 创建日期: #CREATIONDATE#
// 修改记录: #MODIFICATIONHISTORY#
// </summary>
//-----------------------------------------------------------------------

using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class StartGameController : MonoBehaviour
{
	[Title("Start Initialized")] [SerializeField, InlineEditor]
	private VoidEventChannelSO _startInitializer = default;		// 初始化
	
	[Title("Listening to")] [SerializeField, InlineEditor]
	private VoidEventChannelSO _startNewGameEvent = default;

	[Title("Broadcasting on")] [SerializeField, InlineEditor]
	private LoadEventChannelSO _loadLocationTrigger;

	[Title("Load Local Scene")] [SerializeField, InlineEditor]
	private GameSceneSO _localtionToLoad;

	private void Awake()
	{
		_startInitializer.OnEventRaised += InitializedStartGameController;
	}

	private void InitializedStartGameController()
	{
		_startNewGameEvent.OnEventRaised += StartNewGame;
		_startInitializer.OnEventRaised -= InitializedStartGameController;
	}

	private void OnDestroy()
	{
		_startNewGameEvent.OnEventRaised -= StartNewGame;
	}

	private void StartNewGame()
	{
		Debug.Log("开始新的游戏");
	}
}
