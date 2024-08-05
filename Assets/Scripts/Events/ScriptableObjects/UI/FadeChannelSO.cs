using UnityEngine;
using UnityEngine.Events;

namespace Events
{
    /// <summary>
    /// �����ڹ�����Ļ�ĵ���͵���Ч���������˵���͵��������ĵ��ã�ͨ���¼�ϵͳ֪ͨ�������ִ��ʵ�ʵ��Ӿ�Ч����
    /// </summary>
    [CreateAssetMenu(menuName = "Events/UI/Fade Channel")]
    public class FadeChannelSO : DescriptionBaseSO
    {
        public UnityAction<bool, float, Color> OnEventRaised;

        /// <summary>
        /// Fade helper function to simplify usage. Fades the screen in to gameplay.
        /// ���븨����������ʹ�á�����Ļ���뵽��Ϸ�淨���档
        /// </summary>
        /// <param name="duration">����ĳ���ʱ��</param>
        /// <param name="color">Ŀ����ɫ</param>
        public void FadeIn(float duration)
        {
            Fade(true, duration, Color.clear);
        }

        /// <summary>
        /// ����������������ʹ�á�����Ļ��������ɫ��
        /// </summary>
        /// <param name="duration">�����ĳ���ʱ��</param>
        public void FadeOut(float duration)
        {
            Fade(false, duration, Color.black);
        }

        /// <summary>
        /// ͨ�õĵ��뵭���������� <seealso cref="FadeController.cs"/> ����ͨ�š�
        /// </summary>
        /// <param name="fadeIn">true: ���룻false: ����</param>
        /// <param name="duration">���뵭������ʱ��</param>
        /// <param name="color">Ŀ����ɫ</param>
        private void Fade(bool fadeIn, float duration, Color color)
        {
            if (OnEventRaised != null)
                OnEventRaised.Invoke(fadeIn, duration, color);
            else
            {
                Debug.LogWarning("������Fade�¼���û�б�ע��");
            }
        }
    }
}
