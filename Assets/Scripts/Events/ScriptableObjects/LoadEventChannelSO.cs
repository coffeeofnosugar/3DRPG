using UnityEngine;
using UnityEngine.Events;

namespace Events
{
    /// <summary>
    /// 这个类用于场景加载事件。
    /// 获取需要加载的位置或菜单的GameSceneSO，以及指定是否需要显示加载屏幕的bool值。
    /// </summary>
    [CreateAssetMenu(menuName = "Events/Load Event Channel")]
    public class LoadEventChannelSO : DescriptionBaseSO
    {
        /// <summary>
        /// GameSceneSO：要加载的场景
        /// bool：是否显示加载屏幕
        /// bool：是否淡入淡出屏幕
        /// </summary>
        public UnityAction<GameSceneSO, bool, bool> OnLoadingRequested;

        /// <summary>
        /// 用于触发 OnLoadingRequested
        /// </summary>
        /// <param name="locationToLoad">需要加载的场景实例</param>
        /// <param name="showLoadingScreen">是否显示加载屏幕</param>
        /// <param name="fadeScreen">是否进行屏幕淡入淡出效果</param>
        public void RaiseEvent(GameSceneSO locationToLoad, bool showLoadingScreen = false, bool fadeScreen = false)
        {
            if (OnLoadingRequested != null)
            {
                OnLoadingRequested.Invoke(locationToLoad, showLoadingScreen, fadeScreen);
            }
            else
            {
                // 如果没有订阅者，输出警告日志，提示场景加载请求没有被处理，并建议检查是否有监听这个事件的 SceneLoader。
                Debug.LogWarning("A Scene loading was requested, but nobody picked it up. " +
                                 "Check why there is no SceneLoader already present, " +
                                 "and make sure it's listening on this Load Event channel.");
            }
        }
    }
}
