using Unity.VisualScripting;
using UnityEngine;



namespace BehaviourTree
{
    /// <summary>
    /// ����ڵ�
    /// ��˳��ִ�е�һ���ڵ�
    /// ����ýڵ�����һ֡����running������һ֡��Ȼֱ�Ӹýڵ�
    /// ����ýڵ�����һ֡����success\failure������һ��ִ����һ���ӽڵ�
    /// </summary>
    public class Sequencer : CompositeNode
    {
        int current;
        protected override void OnStart()
        {
            current = 0;
        }

        protected override void OnStop()
        {

        }

        protected override State OnUpdate()
        {
            for (int i = current; i < children.Count; ++i)
            {
                current = i;
                var child = children[current];

                switch (child.Update())
                {
                    case State.Running:
                        return State.Running;
                    case State.Failure:
                        return State.Failure;
                    case State.Success:
                        continue;
                }
            }

            return State.Success;
        }
    }
}