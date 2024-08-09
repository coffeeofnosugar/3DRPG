//-----------------------------------------------------------------------
// <summary>
// 文件名: SceneInitializer
// 描述: #DESCRIPTION#
// 作者: #AUTHOR#
// 创建日期: #CREATIONDATE#
// 修改记录: #MODIFICATIONHISTORY#
// </summary>
//-----------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class SceneInitializer : MonoBehaviour
{
	[Title("Scene Ready Event")]	// 触发广播
	[SerializeField, InlineEditor] private VoidEventChannelSO _onSceneReady;

	[Title("Listening to")] // 注册事件
	[SerializeField, InlineEditor] private VoidEventChannelSO _startInitializers = default;		// 场景都加载完毕后开始初始化

	private void Awake()
	{
		_onSceneReady.OnEventRaised += Initialized;
	}
	
	private void Initialized()
	{
		_startInitializers.RaiseEvent();
		_onSceneReady.OnEventRaised -= Initialized;
	}
}
