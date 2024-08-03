using Unity.VisualScripting;
using UnityEngine;



namespace BehaviourTree
{
    /// <summary>
    /// ����ڵ�
    /// ��˳��ִ�е�һ���ڵ�
    /// ����ýڵ�����һ֡����running������һ֡��Ȼִ�иýڵ�
    /// ���Ǹýڵ�����һ֡����success\failure����һ��Ż�ִ����һ���ӽڵ�
    /// ֱ��ִ���������ӽڵ�󱾽ڵ�Ż᷵�أ������ӽڵ㷵�سɹ�����ʧ��
    /// </summary>
    public class Sequencer : CompositeNode
    {
        int current;
        protected override void OnStart()
        {
            base.OnStart();
            Debug.Log($"{guid}  {state}");
            current = 0;
        }

        protected override void OnStop()
        {
            base.OnStop();
        }

        protected override State OnUpdate()
        {
            for (int i = current; i < children.Count; ++i)
            {
                current = i;
                var child = children[current];

                switch (child.Update())
                {
                    case State.Success:
                        continue;
                    case State.Failure:
                        continue;
                    case State.Running:
                        return State.Running;
                }
            }

            return State.Success;
        }
    }
}