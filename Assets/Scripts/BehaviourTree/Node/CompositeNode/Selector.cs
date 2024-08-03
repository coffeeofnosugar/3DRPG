using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    /// <summary>
    /// ѡ��ڵ�
    /// �ӽڵ�������жϣ����ִ��ʧ�ܾ��˳�
    /// </summary>
    public class Selector : CompositeNode
    {
        int current;
        protected override void OnStart()
        {
            base.OnStart();
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
                        return State.Failure;
                    case State.Running:
                        return State.Running;
                }
            }

            return State.Failure;
        }
    }
}