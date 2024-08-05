using UnityEngine;
using UnityEngine.Events;

namespace Events
{
    /// <summary>
    /// 类用于管理屏幕的淡入和淡出效果。它简化了淡入和淡出操作的调用，通过事件系统通知其他组件执行实际的视觉效果。
    /// </summary>
    [CreateAssetMenu(menuName = "Events/UI/Fade Channel")]
    public class FadeChannelSO : DescriptionBaseSO
    {
        public UnityAction<bool, float, Color> OnEventRaised;

        /// <summary>
        /// Fade helper function to simplify usage. Fades the screen in to gameplay.
        /// 淡入辅助函数，简化使用。将屏幕淡入到游戏玩法界面。
        /// </summary>
        /// <param name="duration">淡入的持续时间</param>
        /// <param name="color">目标颜色</param>
        public void FadeIn(float duration)
        {
            Fade(true, duration, Color.clear);
        }

        /// <summary>
        /// 淡出辅助函数，简化使用。将屏幕淡出到黑色。
        /// </summary>
        /// <param name="duration">淡出的持续时间</param>
        public void FadeOut(float duration)
        {
            Fade(false, duration, Color.black);
        }

        /// <summary>
        /// 通用的淡入淡出函数。与 <seealso cref="FadeController.cs"/> 进行通信。
        /// </summary>
        /// <param name="fadeIn">true: 淡入；false: 淡出</param>
        /// <param name="duration">淡入淡出持续时间</param>
        /// <param name="color">目标颜色</param>
        private void Fade(bool fadeIn, float duration, Color color)
        {
            if (OnEventRaised != null)
                OnEventRaised.Invoke(fadeIn, duration, color);
            else
            {
                Debug.LogWarning("触发的Fade事件并没有被注册");
            }
        }
    }
}
