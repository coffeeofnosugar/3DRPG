using Unity.VisualScripting;
using UnityEngine;



namespace BehaviourTree
{
    /// <summary>
    /// 定序节点
    /// 按顺序执行第一个节点
    /// 如果该节点在这一帧返回running，则下一帧依然执行该节点
    /// 除非该节点在这一帧返回success\failure，下一针才会执行下一个子节点
    /// 直到执行完所有子节点后本节点才会返回，无论子节点返回成功还是失败
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