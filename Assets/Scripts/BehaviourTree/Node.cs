using UnityEngine;

/// <summary>
/// ��Ϊ���ڵ�����࣬���нڵ㶼Ҫ�̳������
/// </summary>
public abstract class Node : ScriptableObject
{
    public enum State
    {
        Running,
        Failure,
        Success
    }

    /// <summary>
    /// ��־�������ж�ִ��OnStart����OnStop
    /// </summary>
    private bool started = false;
    /// <summary>
    /// ��ǰ�ڵ��״̬
    /// </summary>
    [HideInInspector] public State state = State.Running;
    /// <summary>
    /// guid���ڵ�Ψһ��ʶ
    /// </summary>
    [HideInInspector] public string guid;
    /// <summary>
    /// ��¼elementԪ������ͼ�е�λ��
    /// </summary>
    [HideInInspector] public Vector2 position;

    public State Update()
    {
        if (!started)
        {
            OnStart();
            started = true;
        }

        state = OnUpdate();

        if (state == State.Failure || state == State.Success)
        {
            OnStop();
            started = false;
        }

        return state;
    }

    /// <summary>
    /// ��¡�ڵ�
    /// �����ͬ�Ĺ�ʹ����ͬһ��ScriptableObject��Ϊ������ô����֮�����е�ʱ����໥Ӱ��
    /// ���Կ�¡һ��ڵ�
    /// </summary>
    /// <returns></returns>
    public virtual Node Clone()
    {
        return Instantiate(this);
    }

    protected abstract void OnStart();
    protected abstract void OnStop();
    protected abstract State OnUpdate();
}