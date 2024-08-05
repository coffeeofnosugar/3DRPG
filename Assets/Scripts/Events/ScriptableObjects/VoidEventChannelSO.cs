using UnityEngine;
using UnityEngine.Events;

namespace Events
{
    /// <summary>
    /// 用于管理没有参数的事件，适用于不需要额外数据的事件，比如退出游戏等。
    /// </summary>

    [CreateAssetMenu(menuName = "Events/Void Event Channel")]
    public class VoidEventChannelSO : DescriptionBaseSO
    {
        public UnityAction OnEventRaised;

        public void RaiseEvent()
        {
            if (OnEventRaised != null)
                OnEventRaised.Invoke();
            else
            {
                Debug.LogWarning("触发的事件并没有被注册");
            }
        }
    }
}
