//-----------------------------------------------------------------------
// <summary>
// 文件名: StartGameController
// 描述: 控制开始游戏
// 作者: #AUTHOR#
// 创建日期: #CREATIONDATE#
// 修改记录: #MODIFICATIONHISTORY#
// </summary>
//-----------------------------------------------------------------------

using Sirenix.OdinInspector;
using UnityEngine;

public class StartGameController : MonoBehaviour
{
	[Title("Start Initialized")]	// 触发广播
	[SerializeField, InlineEditor] private VoidEventChannelSO _startInitializer = default;
	
	[Title("Listening to")]
	[SerializeField, InlineEditor] private VoidEventChannelSO _startNewGameEvent = default;

	private void Awake()
	{
		_startInitializer.OnEventRaised += InitializedStartGameController;
	}

	private void InitializedStartGameController()
	{
		_startNewGameEvent.OnEventRaised += StartNewGame;
		_startInitializer.OnEventRaised -= InitializedStartGameController;
	}

	private void StartNewGame()
	{
		Debug.Log("开始新的游戏");
	}
}
