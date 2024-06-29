using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class EnemyParameter
{
    public int health;
    public float moveSpeed;
    public float chaseSpeed;
    public float idleTime;
    public float attackCD;
    public Animator animator;
    public NavMeshAgent agent;
}

public class FSM : MonoBehaviour
{
    public EnemyParameter parameter;
    private IState currentState;

    private IdleState idleState;
    private WalkState walkState;
    private ChaseState chaseState;
    private RunState runState;
    private AttackState attackState;

    
    private void Awake()
    {
        parameter.animator = GetComponent<Animator>();
        parameter.agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        idleState = new IdleState(this);

        TransitionState(StateType.Idle);
    }

    private void Update()
    {
        currentState.OnUpdate();
    }

    public void TransitionState(StateType type)
    {
        var newState = type switch
        {
            StateType.Idle => idleState,
            //StateType.Walk => 
            _ => null
        };

        currentState.OnExit();
        currentState = newState;
        currentState.OnEnter();
    }
}
