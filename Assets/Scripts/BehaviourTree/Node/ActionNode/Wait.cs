using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;


namespace BehaviourTree
{
    public class Wait : ActionNode
    {
        [Title("节点参数")]
        [SerializeField] private float duration = 1;
        float startTime;
        protected override void OnStart()
        {
            base.OnStart();
            startTime = Time.time;
        }

        protected override void OnStop()
        {
            base.OnStop();
        }

        protected override State OnUpdate()
        {
            if (blackboard.target)
            {
                return State.Failure;
            }
            
            if (Time.time - startTime > duration)
            {
                return State.Success;
            }
            return State.Running;
        }
    }
}