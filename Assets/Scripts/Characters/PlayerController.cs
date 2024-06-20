using UnityEngine;
using UnityEngine.AI;
public class PlayerController : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        MouseManager.Instance.OnMouseClicked += MoveToPoint;
    }

    private void Update()
    {
        SwitchAnimation();
    }


    private void MoveToPoint(Vector3 target)
    {
        agent.destination = target;
    }

    private void SwitchAnimation()
    {
        float speed = agent.velocity.sqrMagnitude;
        float maxSpeed = agent.speed;
        
        animator.SetFloat("Speed", speed / (maxSpeed * maxSpeed));
    }
}
