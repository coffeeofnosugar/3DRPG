using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;


namespace BehaviourTree
{
    public class CoolDown : ActionNode
    {
        [Title("节点参数")]
        [SerializeField] private float coolDown;

        [SerializeField, ReadOnly] private float _time = float.MinValue;

        protected override void OnStart()
        {
            if (blackboard.target && Time.time - _time > coolDown)
            {
                _time = Time.time;
                state = State.Success;
            }
            else
                state = State.Failure;
            base.OnStart();
        }

        protected override State OnUpdate()
        {
            return state;
        }
    }

}