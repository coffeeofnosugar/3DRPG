using Sirenix.OdinInspector;
using UnityEngine;


namespace BehaviourTree
{
    /// <summary>
    /// ��Ϊ���ڵ�����࣬���нڵ㶼Ҫ�̳������
    /// </summary>
    public abstract class Node : ScriptableObject
    {
        public enum State { Running, Failure, Success }

        /// <summary>
        /// ��־�������ж�ִ��OnStart����OnStop
        /// </summary>
        [ReadOnly, FoldoutGroup("Node")] 
        public bool started = false;
        
        /// <summary>
        /// ��ǰ�ڵ��״̬
        /// </summary>
        [ReadOnly, FoldoutGroup("Node")]
        public State state = State.Running;
        
        /// <summary>
        /// guid���ڵ�Ψһ��ʶ
        /// </summary>
        [ReadOnly, FoldoutGroup("Node")]
        public string guid;
        
        /// <summary>
        /// ��¼elementԪ������ͼ�е�λ��
        /// </summary>
        [ReadOnly, FoldoutGroup("Node")]
        public Vector2 position;
        
        /// <summary>
        /// �ڵ�Ԫ�صı�ע
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
}