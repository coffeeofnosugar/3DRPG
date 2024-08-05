using UnityEngine.Events;
using UnityEngine;

namespace Events
{
    /// <summary>
    /// 这个类用于带有bool参数的事件
    /// Example: An event to toggle a UI interface
    /// 比如切换UI界面的事件
    /// </summary>

    [CreateAssetMenu(menuName = "Events/Bool Event Channel")]
    public class BoolEventChannelSO : DescriptionBaseSO
    {
        public event UnityAction<bool> OnEventRaised;

        public void RaiseEvent(bool value)
        {
            if (OnEventRaised != null)
                OnEventRaised.Invoke(value);
            else
            {
                Debug.LogWarning("触发的bool事件并没有被注册");
            }
        }
    }
}
