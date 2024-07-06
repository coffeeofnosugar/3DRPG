using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BehaviourTree
{
    public class DebugLogNode : ActionNode
    {
        public string startMessage;
        public string stopMessage;
        public string updateMessage;
        protected override void OnStart()
        {
            startMessage = name;
            Debug.Log($"OnStart{startMessage}");
        }

        protected override void OnStop()
        {
            Debug.Log($"OnStop{stopMessage}");
        }

        protected override State OnUpdate()
        {
            Debug.Log($"OnUpdate{updateMessage}");
            return State.Success;
        }
    }
}