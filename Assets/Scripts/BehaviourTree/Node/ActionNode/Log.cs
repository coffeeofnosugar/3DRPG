using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;


namespace BehaviourTree
{
    public class Log : ActionNode
    {
        [Title("节点参数")]
        public string message;
        protected override void OnStart()
        {
            base.OnStart();
            Debug.Log($"{message}");
        }

        protected override void OnStop()
        {
            base.OnStop();
        }

        protected override State OnUpdate()
        {
            return State.Success;
        }
    }
}