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
    ReturnCamp,
}
public class EnemyManager : MonoBehaviour
{
    EnemyNavigation navigation;
    [SerializeField] private EnemyState state;
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
                default:
                    break;
            }
        }
    }
}

