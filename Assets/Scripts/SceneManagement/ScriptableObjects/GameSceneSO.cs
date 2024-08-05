using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;


/// <summary>
/// 所有游戏场景的基类，包含了所有场景（包括地点、菜单、管理器）的共同属性。
/// </summary>
public class GameSceneSO : DescriptionBaseSO
{
    /// <summary>
    /// 场景类型
    /// </summary>
    // [EnumPaging]
    public GameSceneType sceneType;
    
    /// <summary>
    /// 存放Scene
    /// </summary>
    public AssetReference sceneReference;
    public Audio.AudioCueSO musicTrack;

    /// <summary>
    /// 由SceneSelector工具用来辨别需要加载的场景类型
    /// </summary>
    public enum GameSceneType
    {
        /// <summary>
        /// 表示游戏中的关卡场景，SceneSelector工具将同时加载PersistentManagers和Gameplay
        /// </summary>
        [InspectorName("关卡场景")]Location,
        /// <summary>
        /// 表示菜单场景，SceneSelector工具将同时加载Gameplay
        /// </summary>
        [InspectorName("菜单场景")]Menu,

        /// <summary>
        /// 特殊场景，初始化场景
        /// </summary>
        [InspectorName("初始化场景")]Initialisation,
        /// <summary>
        /// 特殊场景，持久管理器场景，用于存放场景管理脚本、音频管理脚本、设置系统脚本
        /// </summary>
        [InspectorName("持久管理器场景")]PersistentManagers,
        /// <summary>
        /// 特殊场景，游戏管理场景，用来与存放游戏进程管理器的场景
        /// </summary>
        [InspectorName("游戏管理场景")]Gameplay,

        // 用于开发中的测试场景，不需要在游戏中播放
        [InspectorName("测试场景")]Art,
    }
}
