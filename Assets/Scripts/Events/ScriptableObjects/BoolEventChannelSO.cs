using UnityEngine.Events;
using UnityEngine;

namespace Events
{
    /// <summary>
    /// ��������ڴ���bool�������¼�
    /// Example: An event to toggle a UI interface
    /// �����л�UI������¼�
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
                Debug.LogWarning("������bool�¼���û�б�ע��");
            }
        }
    }
}
