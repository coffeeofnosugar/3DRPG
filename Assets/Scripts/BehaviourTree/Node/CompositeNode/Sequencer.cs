using Unity.VisualScripting;
using UnityEngine;



namespace BehaviourTree
{
    /// <summary>
    /// 定序节点
    /// 按顺序执行第一个节点
    /// 如果该节点在这一帧返回running，则下一帧依然直接该节点
    /// 如果该节点在这一帧返回success\failure，则下一针执行下一个子节点
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