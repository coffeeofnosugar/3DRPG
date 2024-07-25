using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        
        public CharacterStats _characterStats;
        public Blackboard blackboard;
        public PlayerInputController _playerInputController;

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
                Debug.Log(name);
                AddRunningClass?.Invoke();
            }
        }

        protected virtual void ExitState()
        {
            RemoveRunningClass?.Invoke();
        }
        protected abstract State FixeUpdateState();
    }
}