using UnityEngine;


namespace BehaviourTree
{
    /// <summary>
    /// 定序节点
    /// 按顺序执行每个子节点
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

            // 上面的方法是将子节点在这一帧中执行完：执行到一个失败的节点，后续将不在执行
            // 下面的方法是将子节点的分开逐帧执行：不管怎么样都会把所有节点执行完

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