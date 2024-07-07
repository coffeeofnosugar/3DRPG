using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BehaviourTree
{
    public class Log : ActionNode
    {
        public string message;
        protected override void OnStart()
        {
            Debug.Log($"{message}");
        }

        protected override void OnStop()
        {

        }

        protected override State OnUpdate()
        {
            return State.Success;
        }
    }
}