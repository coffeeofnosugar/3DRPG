using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace BehaviourTree
{
    /// <summary>
    /// �ظ�ִ�нڵ�
    /// �ظ�ִ���ӽڵ�
    /// �̳�DecoratorNodeһ���ӽڵ�
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