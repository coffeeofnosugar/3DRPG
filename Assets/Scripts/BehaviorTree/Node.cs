using UnityEngine;

/// <summary>
/// 行为树节点抽象类，所有节点都要继承这个类
/// </summary>
public abstract class Node : ScriptableObject
{
    public enum State
    {
        Running,
        Failure,
        Success
    }

    public State state = State.Running;
    public bool started = false;
    public string guid;
    /// <summary>
    /// 记录element元素在视图中的位置
    /// </summary>
    public Vector2 position;

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

    protected abstract void OnStart();
    protected abstract void OnStop();
    protected abstract State OnUpdate();
}