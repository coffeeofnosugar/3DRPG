using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using Events;

/// <summary>
/// ����ฺ����س־û�������������������Ϸ
/// �������¼����������˵�
/// </summary>

public class InitializationLoader : MonoBehaviour
{
    [SerializeField] private GameSceneSO _managersScene = default;      // �־û�����������
    [SerializeField] private GameSceneSO _menuToLoad = default;         // �˵�����

    [Title("Broadcasting on")]  // �����㲥
    [SerializeField] private AssetReference _menuLoadChannel = default; // ���ڴ�� LoadEventChannelSO ��Դ

    private void Start()
    {
        // �첽���س���
        // LoadSceneMode.Additive��ʾ��ӵ�ǰ�������������滻
        // true��ʾ�Ƿ����̼���ó���
        // �������������ʱ���ᴥ��LoadEventChannel����
        _managersScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true).Completed += LoadEventChannel;
    }

    /// <summary>
    /// ��ʵ���ǿ��ǵ����ȸ���
    /// </summary>
    /// <param name="obj"></param>
    private void LoadEventChannel(AsyncOperationHandle<SceneInstance> obj)
    {
        // �첽���� LoadEventChannelSO ��Դ
        // ��Դ������Ϻ�ִ�� LoadMainMenu ����
        _menuLoadChannel.LoadAssetAsync<LoadEventChannelSO>().Completed += LoadMainMenu;
    }

    private void LoadMainMenu(AsyncOperationHandle<LoadEventChannelSO> obj)
    {
        // �� _menuLoadChannel ���ݹ����������㲥���������������˵��������¼�
        // true��ʾ��ʾ���ػ���
        obj.Result.RaiseEvent(_menuToLoad, true);

        // SceneManager.UnloadSceneAsync(1); // ж�س�ʼ������������������У���ʼ��������Ψһ�ĳ������������������� 0��
        // ����������е���֣������0��Initialization��1��PersistentManagers����ɾ������Ϊ1��scene����Initialization
        // �����������������ַ�������ж�س���
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
    }
}