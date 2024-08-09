using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

/// <summary>
/// 这个类负责加载持久化管理器场景来启动游戏
/// 并引发事件来加载主菜单
/// </summary>

public class InitializationLoader : MonoBehaviour
{
    [SerializeField, InlineEditor] private GameSceneSO _persistentManagersSO = default;      // 持久化管理器场景
    [SerializeField, InlineEditor] private GameSceneSO _menuToLoad = default;         // 菜单场景

    [Title("Broadcasting on")]  // 触发广播
    [SerializeField, InlineEditor] private AssetReference _menuLoadChannel = default; // 用于存放 LoadEventChannelSO 资源

    private void Start()
    {
        // 异步加载场景
        // LoadSceneMode.Additive表示添加当前场景，而不是替换
        // true表示是否立刻激活该场景
        // 当场景加载完成时，会触发LoadEventChannel方法
        _persistentManagersSO.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true).Completed += LoadEventChannel;
    }

    /// <summary>
    /// 其实就是考虑到了热更新
    /// </summary>
    /// <param name="obj"></param>
    private void LoadEventChannel(AsyncOperationHandle<SceneInstance> obj)
    {
        // 异步加载 LoadEventChannelSO 资源
        // 资源加载完毕后执行 LoadMainMenu 方法
        _menuLoadChannel.LoadAssetAsync<LoadEventChannelSO>().Completed += LoadMainMenu;
    }

    private void LoadMainMenu(AsyncOperationHandle<LoadEventChannelSO> obj)
    {
        // 将 _menuLoadChannel 传递过来并触发广播————加载主菜单场景的事件
        // true表示显示加载画面
        obj.Result.RaiseEvent(_menuToLoad, true);

        // SceneManager.UnloadSceneAsync(1); // 卸载初始化场景。在这个例子中，初始化场景是唯一的场景，所以它的索引是 0。
        // 这里的索引有点奇怪，输出的0是Initialization，1是PersistentManagers，但删除索引为1的scene才是Initialization
        // 保险起见，这里采用字符串名称卸载场景
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
    }
}