using UnityEngine;


namespace BehaviourTree
{
    /// <summary>
    /// ����ڵ�
    /// ��˳��ִ��ÿ���ӽڵ�
    /// </summary>
    public class SequencerNode : CompositeNode
    {
        private int current;
        protected override void OnStart()
        {
            current = 0;
        }

        protected override void OnStop()
        {

        }

        protected override State OnUpdate()
        {
            //foreach (var item in children)
            //{
            //    state = item.Update();
            //    if (state == Node.State.Failure)
            //        return state;
            //}
            //return state;

            // ����ķ����ǽ��ӽڵ�����һ֡��ִ���꣺ִ�е�һ��ʧ�ܵĽڵ㣬����������ִ��
            // ����ķ����ǽ��ӽڵ�ķֿ���ִ֡�У�������ô����������нڵ�ִ����

            var child = children[current];
            switch (child.Update())
            {
                case State.Running:
                    return State.Running;
                case State.Failure:
                    return State.Failure;
                case State.Success:
                    current++;
                    break;
                default:
                    break;
            }
            return current == children.Count ? State.Success : State.Running;
        }
    }
}