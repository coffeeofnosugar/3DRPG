using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace BehaviourTree
{
    /// <summary>
    /// 重复执行节点
    /// 重复执行子节点
    /// 继承DecoratorNode一个子节点
    /// </summary>
    public class Repeat : DecoratorNode
    {
        protected override void OnStart()
        {
            base.OnStart();
        }

        protected override void OnStop()
        {
            base.OnStop();
        }

        protected override State OnUpdate()
        {
            child.Update();
            return Node.State.Running;
        }
    }
}