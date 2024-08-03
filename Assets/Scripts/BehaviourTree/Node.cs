using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;


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
        [ReadOnly, BoxGroup] 
        private bool started = false;
        
        /// <summary>
        /// 当前节点的状态
        /// </summary>
        [ReadOnly, BoxGroup]
        public State state = State.Running;
        
        /// <summary>
        /// guid，节点唯一标识
        /// </summary>
        [ReadOnly, BoxGroup]
        public string guid;
        
        /// <summary>
        /// 记录element元素在视图中的位置
        /// </summary>
        [ReadOnly, BoxGroup]
        public Vector2 position;
        
        /// <summary>
        /// 节点元素的备注
        /// </summary>
        [ReadOnly, BoxGroup, TextArea]
        public string description;

        
        [ReadOnly, BoxGroup]
        public Blackboard blackboard;
        
        [ReadOnly, BoxGroup]
        public MonsterStats monsterStats;
        
        
        public Action AddRunningClass;
        public Action RemoveRunningClass;
        
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

        protected virtual void OnStart()
        {
            if (state is State.Running or State.Success)
            {
                AddRunningClass?.Invoke();
            }
        }

        protected virtual void OnStop()
        {
            if (state is State.Failure or State.Success)
            {
                RemoveRunningClass?.Invoke();
            }
        }
        protected abstract State OnUpdate();
    }
}