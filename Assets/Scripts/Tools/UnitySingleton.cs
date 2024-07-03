using UnityEngine;

public class UnitySingleton<T> : MonoBehaviour
    where T : Component
{
    private static bool applicationIsQuitting = false;
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                if (applicationIsQuitting)
                    return _instance;
                _instance = FindObjectOfType(typeof(T)) as T;
                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    _instance = obj.AddComponent<T>();
                }
            }
            return _instance;
        }
    }

    protected virtual void OnDestory()
    {
        applicationIsQuitting = true;
    }
}