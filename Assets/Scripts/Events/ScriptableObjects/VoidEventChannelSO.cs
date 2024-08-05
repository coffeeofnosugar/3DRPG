using UnityEngine;
using UnityEngine.Events;

namespace Events
{
    /// <summary>
    /// ���ڹ���û�в������¼��������ڲ���Ҫ�������ݵ��¼��������˳���Ϸ�ȡ�
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
                Debug.LogWarning("�������¼���û�б�ע��");
            }
        }
    }
}
