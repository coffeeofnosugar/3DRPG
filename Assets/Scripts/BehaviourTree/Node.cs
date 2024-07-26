using UnityEngine;


namespace BehaviourTree
{
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
        //[HideInInspector] 
        public bool started = false;
        /// <summary>
        /// ��ǰ�ڵ��״̬
        /// </summary>
        //[HideInInspector] 
        public State state = State.Running;
        /// <summary>
        /// guid���ڵ�Ψһ��ʶ
        /// </summary>
        [HideInInspector] public string guid;
        /// <summary>
        /// ��¼elementԪ������ͼ�е�λ��
        /// </summary>
        [HideInInspector] public Vector2 position;
        /// <summary>
        /// �ڵ�Ԫ�صı�ע
        /// </summary>
        [TextArea] public string description;

        
        [HideInInspector] public Blackboard blackboard;
        [HideInInspector] public MonsterStats monsterStats;
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