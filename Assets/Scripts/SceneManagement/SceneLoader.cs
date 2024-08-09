using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

/// <summary>
/// 这个类管理场景的加载和卸载。
/// </summary>
public class SceneLoader : MonoBehaviour
{
	[SerializeField, InlineEditor] private GameSceneSO _gameplayScene = default;			// GamePlay场景，该场景里存放了游玩时的各种Manager，如血量的展示、GameManager游戏进程控制等
	[SerializeField, InlineEditor] private Player.InputReader _inputReader = default;		// 用户输入

	[Title("Listening to")]		// 注册事件
	// [SerializeField] private LoadEventChannelSO _loadLocation = default;			// 注册加载玩法场景
	[SerializeField, InlineEditor] private LoadEventChannelSO _loadMenu = default;				// 注册加载菜单场景，加载菜单场景时需要删除Gameplay游戏管理场景，所以要分开将加载玩法场景区分开来
	[SerializeField, InlineEditor] private LoadEventChannelSO _coldStartupLocation = default;		// 注册加载地点场景，该方法是通过点击UnityEditor的Play直接开始游戏，而无需通过Initialization Scene初始化
																					// 这里主要是获取Gameplay场景并保存下来，以免离开当玩法场景时Gameplay没有卸载
	[Title("Broadcasting on")]	// 触发广播
	// [SerializeField] private BoolEventChannelSO _toggleLoadingScreen = default;		// 用于显示或隐藏加载屏幕
	[SerializeField, InlineEditor] private VoidEventChannelSO _onSceneReadyTrigger = default;			// 用于在场景加载完成后触发事件
	// [SerializeField] private FadeChannelSO _fadeRequestChannel = default;			// 用于管理屏幕的淡入和淡出效果

	private AsyncOperationHandle<SceneInstance> _loadingOperationHandle;			// 用于存储场景异步加载操作的句柄。通过它可以跟踪加载操作的状态和结果。
	// private AsyncOperationHandle<SceneInstance> _gameplayManagerLoadingOpHandle;	// 用于存储游戏管理场景异步加载操作的句柄。

	// 存储场景加载请求的参数
	private GameSceneSO _sceneToLoad;				// 要加载的目标场景
	private GameSceneSO _currentlyLoadedScene;		// 当前的场景
	// private bool _showLoadingScreen;				// 标志是否在加载新场景时显示加载屏幕

	// private SceneInstance _gameplaySceneInstance = new SceneInstance();				// 用于存储游戏管理场景的实例，方便在
	// private float _fadeDuration = .5f;												// 淡入淡出持续时间
	private bool _isLoading = false;                                    			// 标志是否正在加载中，防止在加载过程中发起新的加载请求

	/// <summary>
	/// 激活时注册事件
	/// </summary>
	private void OnEnable()
	{
		// _loadLocation.OnLoadingRequested += LoadLocation;
		_loadMenu.OnLoadingRequested += LoadMenu;
#if UNITY_EDITOR	// 因为_coldStartupLocation是在UnityEditor的Play直接开始游戏，所以只能在UnityEditor中运行
		_coldStartupLocation.OnLoadingRequested += LocationColdStartup;
#endif
	}

	/// <summary>
	/// 销毁时注销事件，防止多重调用
	/// </summary>
	private void OnDisable()
	{
		// _loadLocation.OnLoadingRequested -= LoadLocation;
		_loadMenu.OnLoadingRequested -= LoadMenu;
#if UNITY_EDITOR
		_coldStartupLocation.OnLoadingRequested -= LocationColdStartup;
#endif
	}

#if UNITY_EDITOR
	/// <summary>
	/// 仅在 Unity 编辑器中使用的冷启动，即在编辑器中直接启动场景而不经过 Initialisation Scene 初始化。
	/// </summary>
	private void LocationColdStartup(GameSceneSO currentlyOpenedLocation, bool showLoadingScreen, bool fadeScreen)
	{
		_currentlyLoadedScene = currentlyOpenedLocation;

		// 如果该场景是地点场景
		if (_currentlyLoadedScene.sceneType == GameSceneSO.GameSceneType.Location)
		{
			// 加载Gameplay游戏管理场景（同步加载）
			// _gameplayManagerLoadingOpHandle = _gameplayScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
			// _gameplayManagerLoadingOpHandle.WaitForCompletion();
			// _gameplaySceneInstance = _gameplayManagerLoadingOpHandle.Result;

			// Gameplay游戏管理场景加载完毕后启动游戏玩法
			StartGameplay();
		}
	}
#endif

	// /// <summary>
	// /// 处理 Location 关卡场景加载请求
	// /// </summary>
	// private void LoadLocation(GameSceneSO locationToLoad, bool showLoadingScreen, bool fadeScreen)
	// {
	// 	// 检查是否正在加载场景，防止玩家在同一帧中同时进入两个场景
	// 	if (_isLoading)
	// 		return;
	//
	// 	_sceneToLoad = locationToLoad;
	// 	// _showLoadingScreen = showLoadingScreen;
	// 	_isLoading = true;
	//
	// 	// 如果我们从主菜单进入，我们需要首先加载Gameplay游戏管理场景
	// 	if (_gameplaySceneInstance.Scene == null
	// 		|| !_gameplaySceneInstance.Scene.isLoaded)
	// 	{
	// 		// 异步加载Gameplay场景
	// 		_gameplayManagerLoadingOpHandle = _gameplayScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
	// 		_gameplayManagerLoadingOpHandle.Completed += OnGameplayManagersLoaded;		// 加载完毕后将Gameplay游戏管理赋值给_gameplayManagerSceneInstance，并卸载之前的场景
	// 	}
	// 	else
	// 	{
	// 		// 如果Gameplay游戏管理场景已经加载则直接开始卸载之前的场景
	// 		UnloadPreviousScene();
	// 	}
	// }

	// /// <summary>
	// /// 将Gameplay游戏管理赋值给_gameplayManagerSceneInstance，并卸载之前的场景
	// /// </summary>
	// /// <param name="obj"></param>
	// private void OnGameplayManagersLoaded(AsyncOperationHandle<SceneInstance> obj)
	// {
	// 	_gameplaySceneInstance = _gameplayManagerLoadingOpHandle.Result;
	//
	// 	UnloadPreviousScene();
	// }

	/// <summary>
	/// 处理菜单场景加载请求，需要先将Gameplay游戏管理场景删除，防止持续运行游戏玩法逻辑
	/// </summary>
	private void LoadMenu(GameSceneSO menuToLoad, bool showLoadingScreen, bool fadeScreen)
	{
		// 与LoadLocation类似防止重复加载
		if (_isLoading)
			return;

		_sceneToLoad = menuToLoad;
		// _showLoadingScreen = showLoadingScreen;
		_isLoading = true;

		// // 如果我们从玩法关卡返回主菜单，我们需要卸载Gameplay游戏管理场景
		// if (_gameplaySceneInstance.Scene != null
		// 	&& _gameplaySceneInstance.Scene.isLoaded)
		// 	Addressables.UnloadSceneAsync(_gameplayManagerLoadingOpHandle, true);		// 释放后_gameplayManagerSceneInstance同样也变空了

		UnloadPreviousScene();
	}

	/// <summary>
	/// 在Location和Menu加载中，这个函数是卸载先前的场景
	/// </summary>
	private void UnloadPreviousScene()
	{
		_inputReader.DisableAllInput();					// 禁用所有输入
		// _fadeRequestChannel.FadeOut(_fadeDuration);		// 触发淡出效果

		// yield return new WaitForSeconds(_fadeDuration); // 等待淡出完成

		if (_currentlyLoadedScene != null)															// 这里的情况还是挺多的
		{																							// 1.正常启动游戏，第一次进入主菜单，_currentlyLoadedScene从未被赋值过，直接跳过
			if (_currentlyLoadedScene.sceneReference.OperationHandle.IsValid())						// 2.正常启动游戏，从主菜单进入关卡场景，_currentlyLoadedScene是菜单，需要卸载
			{																						// 3.正常启动游戏，从关卡场景进入主菜单，_currentlyLoadedScene是关卡，需要卸载
				// 通过_currentlyLoadedScene的AssetReference卸载场景（即通过Addressable system）		// 4.冷启动，从关卡进入主菜单，冷启动时_currentlyLoadedScene有赋值，所以不为空，
				_currentlyLoadedScene.sceneReference.UnLoadScene();									//							并且_currentlyLoadedScene是通过引擎直接启动的，而不是LoadSceneAsync，所以其OperationHandle句柄无效
			}																						//							执行else部分
#if UNITY_EDITOR
			else
			{
				// 只有在“冷启动”后，玩家移动到一个新场景时才使用
				// 因为_currentlyLoadedScene的AsyncOperationHandle没有被使用(场景已经在编辑器中打开了)，
				// 场景需要使用常规的SceneManager而不是addresable来卸载
				SceneManager.UnloadSceneAsync(_currentlyLoadedScene.sceneReference.editorAsset.name);
			}
#endif
		}

		// 卸载完成后，加载新场景
		LoadNewScene();
	}

	/// <summary>
	/// 异步加载新的场景
	/// </summary>
	private void LoadNewScene()
	{
		// // 是否显示加载屏幕
		// if (_showLoadingScreen)
		// 	_toggleLoadingScreen.RaiseEvent(true);

		// 异步加载场景
		_loadingOperationHandle = _sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true, 0);
		_loadingOperationHandle.Completed += OnNewSceneLoaded;
	}

	private void OnNewSceneLoaded(AsyncOperationHandle<SceneInstance> obj)
	{
		// 保存加载的场景，在下一次加载其他场景时卸载这个场景
		_currentlyLoadedScene = _sceneToLoad;

		Scene s = obj.Result.Scene;
		SceneManager.SetActiveScene(s);			// 设置场景为活动场景，要不然光照、天空盒等不会工作
		LightProbes.TetrahedralizeAsync();		// 触发光照探针计算

		_isLoading = false;						// 还原标志，表示这一次场景加载结束

		// if (_showLoadingScreen)
		// 	_toggleLoadingScreen.RaiseEvent(false);		// 关闭加载屏幕
		//
		// _fadeRequestChannel.FadeIn(_fadeDuration);		// 触发淡入效果
	
		StartGameplay();								// 启动游戏玩法
	}

	private void StartGameplay()
	{
		_onSceneReadyTrigger.RaiseEvent();				// SpawnSystem将玩家刷新到游戏场景中
	}

	private void ExitGame()
	{
		Debug.Log("Exit!");
		Application.Quit();
	}
}
