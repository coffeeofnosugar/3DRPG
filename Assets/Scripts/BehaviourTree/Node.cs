using Sirenix.OdinInspector;
using UnityEngine;


namespace BehaviourTree
{
    /// <summary>
    /// 行为树节点抽象类，所有节点都要继承这个类
    /// </summary>
    public abstract class Node : ScriptableObject
    {
        public enum State { Running, Failure, Success }

        /// <summary>
        /// 标志，用来判断执行OnStart还是OnStop
        /// </summary>
        [ReadOnly, FoldoutGroup("Node")] 
        public bool started = false;
        
        /// <summary>
        /// 当前节点的状态
        /// </summary>
        [ReadOnly, FoldoutGroup("Node")]
        public State state = State.Running;
        
        /// <summary>
        /// guid，节点唯一标识
        /// </summary>
        [ReadOnly, FoldoutGroup("Node")]
        public string guid;
        
        /// <summary>
        /// 记录element元素在视图中的位置
        /// </summary>
        [ReadOnly, FoldoutGroup("Node")]
        public Vector2 position;
        
        /// <summary>
        /// 节点元素的备注
        /// </summary>
        [ReadOnly, FoldoutGroup("Node"), TextArea]
        public string description;

        
        [ReadOnly, FoldoutGroup("Node")]
        public Blackboard blackboard;
        
        [ReadOnly, FoldoutGroup("Node")]
        public MonsterStats monsterStats;
        
        public State Update()
        {
            if (!started)
            {
                OnStart();
                started = true;
            }

            state = OnUpdate();

            if (state != State.Running)
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
        public virtual Node Clone()
        {
            return Instantiate(this);
        }

        protected abstract void OnStart();
        protected abstract void OnStop();
        protected abstract State OnUpdate();
    }
}