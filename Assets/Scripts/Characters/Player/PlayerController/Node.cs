using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player.PlayerController
{
    public abstract class Node : ScriptableObject
    {
        public enum State { Running, Failure, Success }

        [ReadOnly, BoxGroup]
        public bool started = false;

        [ReadOnly, BoxGroup]
        public State state = State.Running;

        [ReadOnly, BoxGroup]
        public string guid;
        
        [ReadOnly, BoxGroup]
        public Vector2 position;
        
        [BoxGroup, TextArea]
        public string description;
        
        [ReadOnly, BoxGroup]
        public PlayerStats playerStats;
        [ReadOnly, BoxGroup]
        public Blackboard blackboard;
        [ReadOnly, BoxGroup]
        public PlayerInputController playerInputController;

        public Action AddRunningClass;
        public Action RemoveRunningClass;

        public State FixedUpdate()
        {
            if (!started)
            {
                EnterState();
                started = true;
            }

            state = FixeUpdateState();
            if (state != State.Running)
            {
                ExitState();
                started = false;
            }

            return state;
        }

        public virtual Node Clone()
        {
            return Instantiate(this);
        }

        protected virtual void EnterState()
        {
            if (state is State.Running or State.Success)
            {
                AddRunningClass?.Invoke();
            }
        }

        protected virtual void ExitState()
        {
            if (state is State.Failure or State.Success)
            {
                RemoveRunningClass?.Invoke();
            }
        }
        protected abstract State FixeUpdateState();
    }
}