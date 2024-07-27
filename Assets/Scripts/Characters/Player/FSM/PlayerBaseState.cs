namespace Player
{
    public abstract class PlayerBaseState : BaseState<PlayStateMachine.PlayerState>
    {
        protected readonly PlayStateMachine _fsm;
        protected readonly PlayerStats _playerStats;
        
        protected PlayerBaseState(PlayStateMachine fsm, PlayStateMachine.PlayerState key) : base(key)
        {
            _fsm = fsm;
            _playerStats = fsm.characterStats;
        }

        public override void EnterState() { }
        public override void ExitState() { }
        public override void FixedUpdate() { }
        public override void UpdateState() { }
        public override void LateUpdate() { }
        public override void OnAnimatorMove() { }

        public override PlayStateMachine.PlayerState GetNextState()
        {
            return StateKey;
        }
    }
}