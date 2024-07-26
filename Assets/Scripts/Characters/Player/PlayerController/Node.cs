using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player.PlayerController
{
    public abstract class Node : ScriptableObject
    {
        public enum State { Running, Failure, Success }

        public bool started = false;

        public State state = State.Running;

        public string guid;
        public Vector2 position;
        public string description;
        
        public PlayerStats playerStats;
        public Blackboard blackboard;
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