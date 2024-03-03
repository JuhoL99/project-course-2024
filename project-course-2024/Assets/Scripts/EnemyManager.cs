using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Idle,
    MainGoal,
    ClearObstacle,
    ChasePlayer,
    Spawn,
    ReturnCamp,
}
public class EnemyManager : MonoBehaviour
{
    EnemyNavigation navigation;
    [SerializeField] private Animator enemyAnim;
    [SerializeField] private EnemyState state;
    //private bool hasSpawned = false;
    void Start()
    {
        navigation = GetComponent<EnemyNavigation>();
        OnSpawn();
    }
    void Update()
    {
        //check for state changes
        StateChange();
    }
    public EnemyState State
    {
        get { return state; }
        set { state = value; }
    }
    public void OnSpawn()
    {
        //set spawn state, change to idle first and main at night?
        State = EnemyState.Spawn;
        StartCoroutine(SpawnCoroutine());
        
    }
    private IEnumerator SpawnCoroutine()
    {
        enemyAnim.Play("Summon");
        yield return new WaitForSeconds(enemyAnim.GetCurrentAnimatorStateInfo(0).length);
        State = EnemyState.MainGoal;
    }
    private void StateChange()
    {
        {
            switch (state)
            {
                case EnemyState.Idle:
                    break;
                case EnemyState.MainGoal:
                    navigation.MainGoal();
                    break;
                case EnemyState.ChasePlayer:
                    navigation.ChasePlayer();
                    break;
                case EnemyState.ClearObstacle:
                    navigation.ClearObstacle();
                    break;
                case EnemyState.ReturnCamp:
                    break;
                case EnemyState.Spawn:
                    break;
                default:
                    break;
            }
        }
    }
}

