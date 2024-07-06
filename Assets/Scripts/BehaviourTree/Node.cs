using UnityEngine;


namespace BehaviourTree
{
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

        /// <summary>
        /// 标志，用来判断执行OnStart还是OnStop
        /// </summary>
        private bool started = false;
        /// <summary>
        /// 当前节点的状态
        /// </summary>
        [HideInInspector] public State state = State.Running;
        /// <summary>
        /// guid，节点唯一标识
        /// </summary>
        [HideInInspector] public string guid;
        /// <summary>
        /// 记录element元素在视图中的位置
        /// </summary>
        [HideInInspector] public Vector2 position;

        [HideInInspector] public Transform transform;

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
        /// 克隆节点
        /// 如果不同的怪使用了同一个ScriptableObject行为树，那么他们之间运行的时候会相互影响
        /// 可以克隆一遍节点
        /// </summary>
        /// <returns></returns>
        public virtual Node Clone(Transform t)
        {
            this.transform = t;
            return Instantiate(this);
        }

        protected abstract void OnStart();
        protected abstract void OnStop();
        protected abstract State OnUpdate();
    }
}