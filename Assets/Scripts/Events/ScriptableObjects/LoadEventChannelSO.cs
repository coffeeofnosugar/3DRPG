using UnityEngine;
using UnityEngine.Events;

namespace Events
{
    /// <summary>
    /// ��������ڳ��������¼���
    /// ��ȡ��Ҫ���ص�λ�û�˵���GameSceneSO���Լ�ָ���Ƿ���Ҫ��ʾ������Ļ��boolֵ��
    /// </summary>
    [CreateAssetMenu(menuName = "Events/Load Event Channel")]
    public class LoadEventChannelSO : DescriptionBaseSO
    {
        /// <summary>
        /// GameSceneSO��Ҫ���صĳ���
        /// bool���Ƿ���ʾ������Ļ
        /// bool���Ƿ��뵭����Ļ
        /// </summary>
        public UnityAction<GameSceneSO, bool, bool> OnLoadingRequested;

        /// <summary>
        /// ���ڴ��� OnLoadingRequested
        /// </summary>
        /// <param name="locationToLoad">��Ҫ���صĳ���ʵ��</param>
        /// <param name="showLoadingScreen">�Ƿ���ʾ������Ļ</param>
        /// <param name="fadeScreen">�Ƿ������Ļ���뵭��Ч��</param>
        public void RaiseEvent(GameSceneSO locationToLoad, bool showLoadingScreen = false, bool fadeScreen = false)
        {
            if (OnLoadingRequested != null)
            {
                OnLoadingRequested.Invoke(locationToLoad, showLoadingScreen, fadeScreen);
            }
            else
            {
                // ���û�ж����ߣ����������־����ʾ������������û�б��������������Ƿ��м�������¼��� SceneLoader��
                Debug.LogWarning("A Scene loading was requested, but nobody picked it up. " +
                                 "Check why there is no SceneLoader already present, " +
                                 "and make sure it's listening on this Load Event channel.");
            }
        }
    }
}
