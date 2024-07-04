using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    /// <summary>
    /// �ظ�ִ�нڵ�
    /// �ظ�ִ���ӽڵ�
    /// �̳�DecoratorNodeһ���ӽڵ�
    /// </summary>
    public class RepeatNode : DecoratorNode
    {
        protected override void OnStart()
        {

        }

        protected override void OnStop()
        {

        }

        protected override State OnUpdate()
        {
            child.Update();
            return Node.State.Running;
        }
    }
}