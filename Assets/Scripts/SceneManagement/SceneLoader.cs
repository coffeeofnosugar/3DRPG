using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

/// <summary>
/// �����������ļ��غ�ж�ء�
/// </summary>
public class SceneLoader : MonoBehaviour
{
	[SerializeField, InlineEditor] private GameSceneSO _gameplayScene = default;			// GamePlay�������ó�������������ʱ�ĸ���Manager����Ѫ����չʾ��GameManager��Ϸ���̿��Ƶ�
	[SerializeField, InlineEditor] private Player.InputReader _inputReader = default;		// �û�����

	[Title("Listening to")]		// ע���¼�
	// [SerializeField] private LoadEventChannelSO _loadLocation = default;			// ע������淨����
	[SerializeField, InlineEditor] private LoadEventChannelSO _loadMenu = default;				// ע����ز˵����������ز˵�����ʱ��Ҫɾ��Gameplay��Ϸ������������Ҫ�ֿ��������淨�������ֿ���
	[SerializeField, InlineEditor] private LoadEventChannelSO _coldStartupLocation = default;		// ע����صص㳡�����÷�����ͨ�����UnityEditor��Playֱ�ӿ�ʼ��Ϸ��������ͨ��Initialization Scene��ʼ��
																					// ������Ҫ�ǻ�ȡGameplay���������������������뿪���淨����ʱGameplayû��ж��
	[Title("Broadcasting on")]	// �����㲥
	// [SerializeField] private BoolEventChannelSO _toggleLoadingScreen = default;		// ������ʾ�����ؼ�����Ļ
	[SerializeField, InlineEditor] private VoidEventChannelSO _onSceneReadyTrigger = default;			// �����ڳ���������ɺ󴥷��¼�
	// [SerializeField] private FadeChannelSO _fadeRequestChannel = default;			// ���ڹ�����Ļ�ĵ���͵���Ч��

	private AsyncOperationHandle<SceneInstance> _loadingOperationHandle;			// ���ڴ洢�����첽���ز����ľ����ͨ�������Ը��ټ��ز�����״̬�ͽ����
	// private AsyncOperationHandle<SceneInstance> _gameplayManagerLoadingOpHandle;	// ���ڴ洢��Ϸ�������첽���ز����ľ����

	// �洢������������Ĳ���
	private GameSceneSO _sceneToLoad;				// Ҫ���ص�Ŀ�곡��
	private GameSceneSO _currentlyLoadedScene;		// ��ǰ�ĳ���
	// private bool _showLoadingScreen;				// ��־�Ƿ��ڼ����³���ʱ��ʾ������Ļ

	// private SceneInstance _gameplaySceneInstance = new SceneInstance();				// ���ڴ洢��Ϸ��������ʵ����������
	// private float _fadeDuration = .5f;												// ���뵭������ʱ��
	private bool _isLoading = false;                                    			// ��־�Ƿ����ڼ����У���ֹ�ڼ��ع����з����µļ�������

	/// <summary>
	/// ����ʱע���¼�
	/// </summary>
	private void OnEnable()
	{
		// _loadLocation.OnLoadingRequested += LoadLocation;
		_loadMenu.OnLoadingRequested += LoadMenu;
#if UNITY_EDITOR	// ��Ϊ_coldStartupLocation����UnityEditor��Playֱ�ӿ�ʼ��Ϸ������ֻ����UnityEditor������
		_coldStartupLocation.OnLoadingRequested += LocationColdStartup;
#endif
	}

	/// <summary>
	/// ����ʱע���¼�����ֹ���ص���
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
	/// ���� Unity �༭����ʹ�õ������������ڱ༭����ֱ������������������ Initialisation Scene ��ʼ����
	/// </summary>
	private void LocationColdStartup(GameSceneSO currentlyOpenedLocation, bool showLoadingScreen, bool fadeScreen)
	{
		_currentlyLoadedScene = currentlyOpenedLocation;

		// ����ó����ǵص㳡��
		if (_currentlyLoadedScene.sceneType == GameSceneSO.GameSceneType.Location)
		{
			// ����Gameplay��Ϸ��������ͬ�����أ�
			// _gameplayManagerLoadingOpHandle = _gameplayScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
			// _gameplayManagerLoadingOpHandle.WaitForCompletion();
			// _gameplaySceneInstance = _gameplayManagerLoadingOpHandle.Result;

			// Gameplay��Ϸ������������Ϻ�������Ϸ�淨
			StartGameplay();
		}
	}
#endif

	// /// <summary>
	// /// ���� Location �ؿ�������������
	// /// </summary>
	// private void LoadLocation(GameSceneSO locationToLoad, bool showLoadingScreen, bool fadeScreen)
	// {
	// 	// ����Ƿ����ڼ��س�������ֹ�����ͬһ֡��ͬʱ������������
	// 	if (_isLoading)
	// 		return;
	//
	// 	_sceneToLoad = locationToLoad;
	// 	// _showLoadingScreen = showLoadingScreen;
	// 	_isLoading = true;
	//
	// 	// ������Ǵ����˵����룬������Ҫ���ȼ���Gameplay��Ϸ������
	// 	if (_gameplaySceneInstance.Scene == null
	// 		|| !_gameplaySceneInstance.Scene.isLoaded)
	// 	{
	// 		// �첽����Gameplay����
	// 		_gameplayManagerLoadingOpHandle = _gameplayScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
	// 		_gameplayManagerLoadingOpHandle.Completed += OnGameplayManagersLoaded;		// ������Ϻ�Gameplay��Ϸ����ֵ��_gameplayManagerSceneInstance����ж��֮ǰ�ĳ���
	// 	}
	// 	else
	// 	{
	// 		// ���Gameplay��Ϸ�������Ѿ�������ֱ�ӿ�ʼж��֮ǰ�ĳ���
	// 		UnloadPreviousScene();
	// 	}
	// }

	// /// <summary>
	// /// ��Gameplay��Ϸ����ֵ��_gameplayManagerSceneInstance����ж��֮ǰ�ĳ���
	// /// </summary>
	// /// <param name="obj"></param>
	// private void OnGameplayManagersLoaded(AsyncOperationHandle<SceneInstance> obj)
	// {
	// 	_gameplaySceneInstance = _gameplayManagerLoadingOpHandle.Result;
	//
	// 	UnloadPreviousScene();
	// }

	/// <summary>
	/// ����˵���������������Ҫ�Ƚ�Gameplay��Ϸ������ɾ������ֹ����������Ϸ�淨�߼�
	/// </summary>
	private void LoadMenu(GameSceneSO menuToLoad, bool showLoadingScreen, bool fadeScreen)
	{
		// ��LoadLocation���Ʒ�ֹ�ظ�����
		if (_isLoading)
			return;

		_sceneToLoad = menuToLoad;
		// _showLoadingScreen = showLoadingScreen;
		_isLoading = true;

		// // ������Ǵ��淨�ؿ��������˵���������Ҫж��Gameplay��Ϸ������
		// if (_gameplaySceneInstance.Scene != null
		// 	&& _gameplaySceneInstance.Scene.isLoaded)
		// 	Addressables.UnloadSceneAsync(_gameplayManagerLoadingOpHandle, true);		// �ͷź�_gameplayManagerSceneInstanceͬ��Ҳ�����

		UnloadPreviousScene();
	}

	/// <summary>
	/// ��Location��Menu�����У����������ж����ǰ�ĳ���
	/// </summary>
	private void UnloadPreviousScene()
	{
		_inputReader.DisableAllInput();					// ������������
		// _fadeRequestChannel.FadeOut(_fadeDuration);		// ��������Ч��

		// yield return new WaitForSeconds(_fadeDuration); // �ȴ��������

		if (_currentlyLoadedScene != null)															// ������������ͦ���
		{																							// 1.����������Ϸ����һ�ν������˵���_currentlyLoadedScene��δ����ֵ����ֱ������
			if (_currentlyLoadedScene.sceneReference.OperationHandle.IsValid())						// 2.����������Ϸ�������˵�����ؿ�������_currentlyLoadedScene�ǲ˵�����Ҫж��
			{																						// 3.����������Ϸ���ӹؿ������������˵���_currentlyLoadedScene�ǹؿ�����Ҫж��
				// ͨ��_currentlyLoadedScene��AssetReferenceж�س�������ͨ��Addressable system��		// 4.���������ӹؿ��������˵���������ʱ_currentlyLoadedScene�и�ֵ�����Բ�Ϊ�գ�
				_currentlyLoadedScene.sceneReference.UnLoadScene();									//							����_currentlyLoadedScene��ͨ������ֱ�������ģ�������LoadSceneAsync��������OperationHandle�����Ч
			}																						//							ִ��else����
#if UNITY_EDITOR
			else
			{
				// ֻ���ڡ���������������ƶ���һ���³���ʱ��ʹ��
				// ��Ϊ_currentlyLoadedScene��AsyncOperationHandleû�б�ʹ��(�����Ѿ��ڱ༭���д���)��
				// ������Ҫʹ�ó����SceneManager������addresable��ж��
				SceneManager.UnloadSceneAsync(_currentlyLoadedScene.sceneReference.editorAsset.name);
			}
#endif
		}

		// ж����ɺ󣬼����³���
		LoadNewScene();
	}

	/// <summary>
	/// �첽�����µĳ���
	/// </summary>
	private void LoadNewScene()
	{
		// // �Ƿ���ʾ������Ļ
		// if (_showLoadingScreen)
		// 	_toggleLoadingScreen.RaiseEvent(true);

		// �첽���س���
		_loadingOperationHandle = _sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true, 0);
		_loadingOperationHandle.Completed += OnNewSceneLoaded;
	}

	private void OnNewSceneLoaded(AsyncOperationHandle<SceneInstance> obj)
	{
		// ������صĳ���������һ�μ�����������ʱж���������
		_currentlyLoadedScene = _sceneToLoad;

		Scene s = obj.Result.Scene;
		SceneManager.SetActiveScene(s);			// ���ó���Ϊ�������Ҫ��Ȼ���ա���պеȲ��Ṥ��
		LightProbes.TetrahedralizeAsync();		// ��������̽�����

		_isLoading = false;						// ��ԭ��־����ʾ��һ�γ������ؽ���

		// if (_showLoadingScreen)
		// 	_toggleLoadingScreen.RaiseEvent(false);		// �رռ�����Ļ
		//
		// _fadeRequestChannel.FadeIn(_fadeDuration);		// ��������Ч��
	
		StartGameplay();								// ������Ϸ�淨
	}

	private void StartGameplay()
	{
		_onSceneReadyTrigger.RaiseEvent();				// SpawnSystem�����ˢ�µ���Ϸ������
	}

	private void ExitGame()
	{
		Debug.Log("Exit!");
		Application.Quit();
	}
}
