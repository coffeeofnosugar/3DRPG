using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BehaviourTree
{
    public class AnimatorNode : ActionNode
    {
        public string triggerParameter;
        protected override void OnStart()
        {
            transform.GetComponent<Animator>().SetTrigger(triggerParameter);
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