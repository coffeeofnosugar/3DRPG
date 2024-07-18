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
        
        private CharacterStats _characterStats;
        public PlayerInputController _playerInputController;

        public State Update()
        {
            if (!started)
            {
                EnterState();
                started = true;
            }

            state = UpdateState();
            if (state != State.Running)
            {
                ExitState();
                started = false;
            }

            return state;
        }

        protected abstract void EnterState();
        protected abstract void ExitState();
        protected abstract void FixeUpdateState();
        protected abstract State UpdateState();
        protected abstract void LateUpdateState();
    }
}