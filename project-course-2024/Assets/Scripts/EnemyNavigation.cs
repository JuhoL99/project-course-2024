using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.AI;


// TO DO:
// MAKE CHASE DISTANCE LONGER WHEN PLAYER HAS BEEN DETECTED, IMPROVE CHASING IN GENERAL (currently tries to chase player through walls if detected and in range)
// FIX ROTATIONS
// ANIMATIONS?
// SEPARATE ATTACK SCRIPT?


public class EnemyNavigation : MonoBehaviour
{
    private EnemyManager enemyManager;
    private NavMeshAgent agent;
    private Rigidbody rb;
    private PlayerManager player;
    private Vector3 mainGoal = Vector3.zero;
    [SerializeField] private GameObject egg;
    [SerializeField] private LayerMask layerToHit;
    [SerializeField] private Animator enemyAnim;

    [SerializeField] private GameObject currentObstacle;

    [SerializeField] private DestructibleObject obstacle;
    private bool validTarget;
    private bool knockBackEffect = false;

    private float timeNav;
    private float timerNav = 0.1f;

    private float timeStruct;
    private float timerStruct = 3f;

    [SerializeField] private Vector3 playerDirection;
    [SerializeField] private float playerAngle;
    [SerializeField] private float maxAngle = 120f;
    public float chaseDistance;
    [SerializeField] private float attackRange;

    public Vector3 navmeshVel; //debug
    private Vector3 rbVel; //debug
    public Vector3 moveAmount;
    private bool isDead = false;
    private bool isAttacking = false;
    [SerializeField] private Collider weaponCollider;

    Egg eggScript;
    public int damageToEgg = 5;
    bool hitSwitch;
    private Coroutine currentHitSwitchTimer;

    private void Start()
    {
        enemyManager = GetComponent<EnemyManager>();
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        player = PlayerManager.instance;
        egg = GameObject.FindWithTag("MainTarget");
        eggScript = egg.GetComponent<Egg>();
        mainGoal = egg.transform.position;
    }
    private void Update()
    {
        if (knockBackEffect)
            return;
        enemyAnim.SetBool("isMoving", !agent.isStopped);
        rb.velocity = Vector3.zero;
        navmeshVel = agent.velocity; //debug
        rbVel = rb.velocity;  //debug
        moveAmount = new Vector3(agent.velocity.x, 0, agent.velocity.z).normalized;

        //MoveToMouse();
        CheckPlayer();
        //rigidbody makes agents jitter and bug out when they touch eachother, setting velocity to zero fixes it?
        //remove rigidbody later?
        //if (!knockBackEffect) rb.velocity = Vector3.zero;
    }
    public void KnockBack(float knockBackAmount)
    {
        StartCoroutine(KnockBackCoroutine(knockBackAmount));
    }
    private IEnumerator KnockBackCoroutine(float knockBackAmount)
    {
        enemyAnim.Play("KnockBack");
        //enemyAnim.Play("TakingDamage");
        agent.enabled = false;
        knockBackEffect = true;
        rb.AddForce(player.transform.forward * 5 * knockBackAmount, ForceMode.Impulse);
        yield return new WaitForSeconds(0.5f);
        agent.enabled = true; 
        knockBackEffect = false;
    }
    //just for testing navigation, remove later
    /*void MoveToMouse()
    {
        agent.isStopped = false;
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
            {
                agent.destination = hit.point;
            }
        }
    }*/
    public void MainGoal()
    {
        //move towards main goal and check for obstacles with raycast few times a second
        if (knockBackEffect)
            return;
        timeNav += Time.deltaTime;
        if (timeNav > timerNav)
        {
            timeNav = 0;
            agent.SetDestination(mainGoal);
            CheckObstacles();
            float distance = Vector3.Distance(transform.position, mainGoal);
            Debug.Log(distance);
            if(distance < 3 && !isDead)
            {
                DamageEggAndDie();
            }
        }
    }
    void CheckPlayer()
    {
        playerDirection = player.transform.position - transform.position;
        playerAngle = Vector3.Angle(playerDirection, transform.forward);
        //if player in allowed angle, try to catch it with raycast
        if (playerAngle < maxAngle && playerDirection.magnitude < chaseDistance)
        {
            RaycastHit hit;
            //Debug.DrawRay(transform.position, playerDirection * chaseDistance, Color.blue, 20f);
            if (Physics.Raycast(transform.position, playerDirection, out hit, chaseDistance))
            {
                //if player is detected start chasing it no matter what doing
                if (hit.collider.gameObject == player.gameObject)
                {
                    //Debug.Log("hit player");
                    agent.isStopped = false;
                    validTarget = false;
                    enemyManager.State = EnemyState.ChasePlayer;
                }
            }
        }
    }
    void CheckObstacles()
    {
        RaycastHit hit;
        Vector3 castLocation = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        Debug.DrawRay(castLocation, transform.forward * 1.5f, Color.red, 20f);
        if (Physics.Raycast(castLocation, transform.forward, out hit, 1f, layerToHit.value))
        {
            //if obstacle is detected, set it as target and start attacking it
            currentObstacle = hit.collider.gameObject;
            enemyManager.State = EnemyState.ClearObstacle;
        }
    }
    public void ClearObstacle()
    {
        if (currentObstacle != null && !validTarget)
        {
            //Debug.Log("finding script");
            obstacle = currentObstacle.GetComponent<DestructibleObject>();
            if (obstacle != null)
            {
                validTarget = true;
            }
        }
        else if (currentObstacle != null)
        {
            agent.isStopped = true;
            //Debug.Log("attacking");
            timeStruct += Time.deltaTime;
            if (timeStruct > timerStruct)
            {
                timeStruct = 0;
                Hit();
                obstacle.TakeDamage(5);
            }
        }
        else
        {
            validTarget = false;
            agent.isStopped = false;
            enemyManager.State = EnemyState.MainGoal;
        }

    }
    void Hit()
    {
        enemyAnim.Play(hitSwitch ? "Hit2" : "Hit");
        hitSwitch = !hitSwitch;
        if (currentHitSwitchTimer != null) StopCoroutine(currentHitSwitchTimer);
        currentHitSwitchTimer = StartCoroutine("HitSwitchTimer");
    }
    IEnumerable HitSwitchTimer()
    {
        yield return new WaitForSeconds(5);
        hitSwitch = false;
    }
    public void ChasePlayer()
    {
        if (agent.enabled)
        {
            agent.SetDestination(player.transform.position);
        }
        if (playerDirection.magnitude < attackRange && !isAttacking)
        {
            Attack();
        }
        //later: if player in attack range, initiate attack anim
        if (playerDirection.magnitude > chaseDistance)
        {
            enemyManager.State = EnemyState.MainGoal;
        }
    }
    void DamageEggAndDie()
    {
        Destroy(gameObject, 1f);
        isDead = true;
        eggScript.ChangeHealth(-damageToEgg);
        return;
    }
    void Attack()
    {
        weaponCollider.enabled = true; agent.isStopped = true; isAttacking = true;
        agent.speed = 1;
        Hit();
    }
    public void AttackDone()
    {
        isAttacking = false; agent.isStopped = false; weaponCollider.enabled = false;
        agent.speed = 2;
    }
}
