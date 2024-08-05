using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;


/// <summary>
/// ������Ϸ�����Ļ��࣬���������г����������ص㡢�˵������������Ĺ�ͬ���ԡ�
/// </summary>
public class GameSceneSO : DescriptionBaseSO
{
    /// <summary>
    /// ��������
    /// </summary>
    // [EnumPaging]
    public GameSceneType sceneType;
    
    /// <summary>
    /// ���Scene
    /// </summary>
    public AssetReference sceneReference;
    public Audio.AudioCueSO musicTrack;

    /// <summary>
    /// ��SceneSelector�������������Ҫ���صĳ�������
    /// </summary>
    public enum GameSceneType
    {
        /// <summary>
        /// ��ʾ��Ϸ�еĹؿ�������SceneSelector���߽�ͬʱ����PersistentManagers��Gameplay
        /// </summary>
        [InspectorName("�ؿ�����")]Location,
        /// <summary>
        /// ��ʾ�˵�������SceneSelector���߽�ͬʱ����Gameplay
        /// </summary>
        [InspectorName("�˵�����")]Menu,

        /// <summary>
        /// ���ⳡ������ʼ������
        /// </summary>
        [InspectorName("��ʼ������")]Initialisation,
        /// <summary>
        /// ���ⳡ�����־ù��������������ڴ�ų�������ű�����Ƶ����ű�������ϵͳ�ű�
        /// </summary>
        [InspectorName("�־ù���������")]PersistentManagers,
        /// <summary>
        /// ���ⳡ������Ϸ������������������Ϸ���̹������ĳ���
        /// </summary>
        [InspectorName("��Ϸ������")]Gameplay,

        // ���ڿ����еĲ��Գ���������Ҫ����Ϸ�в���
        [InspectorName("���Գ���")]Art,
    }
}
