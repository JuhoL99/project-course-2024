using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Move,
    Attack,
    Idle
}
public class EnemyNavigationTest : MonoBehaviour
{
    private NavMeshAgent agent;
    public EnemyState currentState;
    [SerializeField] private GameObject currentTarget;
    [SerializeField] private GameObject mainTarget;
    [SerializeField] private float attackRange = 1.0f;
    [SerializeField] public float timeBetweenAttacks = 1f;
    [SerializeField] public float attackTimer = 0;
    [SerializeField] private bool attackCooldown = false;

    public float debugdistance;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        mainTarget = GameObject.FindGameObjectWithTag("MainTarget");
        currentState = EnemyState.Move;
    }
    private void Update()
    {
        if(currentTarget != null && currentState != EnemyState.Attack)
        {
            TargetProximity();
            debugdistance = Vector3.Distance(transform.position, currentTarget.transform.position);
        }
            
        CurrentState();
        attackTimer += Time.deltaTime;
        if (attackTimer > timeBetweenAttacks)
        {
            attackTimer = 0;
            attackCooldown = false;
        }
    }
    private void CurrentState()
    {
        switch(currentState)
        {
            case EnemyState.Move:
                MoveToObjective();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Idle:
                break;
            default:
                break;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("DestructibleObject"))
        {
            currentTarget = other.gameObject;
        }
    }
    private void TargetProximity()
    {
        agent.SetDestination(currentTarget.transform.position);

        if (Vector3.Distance(transform.position, currentTarget.transform.position) < attackRange)
        {
            agent.isStopped = true;
            agent.ResetPath();
            currentState = EnemyState.Attack;
        }
    }
    private void Attack()
    {
        if (currentTarget != null)
        {
            agent.enabled = false;
            DestructibleObject destructible = currentTarget.GetComponent<DestructibleObject>();
            if (!attackCooldown)
            {
                destructible.TakeDamage(4);
                attackTimer = 0;
                attackCooldown = true;
            }
            else
            {
                return;
            }
        }
        else
        {
            agent.enabled = true;
            agent.ResetPath();
            currentState = EnemyState.Move;
        }

    }
    private void MoveToObjective()
    {
        agent.SetDestination(mainTarget.transform.position);
    }
}
