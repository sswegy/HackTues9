using System.Xml;
using UnityEngine;
using UnityEngine.AI;


public class ChrisBateAI : MonoBehaviour
{
    public Animator animator;
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask playerMask = 1 << 3;
    public float health = 100f;

    public float attackDelay = 1f;
    public bool haveAttacked = false;
    public GameObject projectile;

    public float attackRange = 3f;
    public float sightRange = 18f;

    void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()  
    {
        bool playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerMask);
        bool playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerMask);

        if (!playerInSightRange && !playerInAttackRange) 
            Idle();
        else if (playerInAttackRange)
            AttackPlayer();
        else
            ChasePlayer();
    }
       
    private void Idle()
    {
        animator.SetBool("isRunning", false);
        animator.SetBool("isIdeal", true);
    }

    private void ChasePlayer()
    {
        animator.SetBool("isRunning", true);
        animator.SetBool("isIdle", false);
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        animator.SetBool("isRunning", false);
        animator.SetBool("isIdle", false);
        animator.SetFloat("punchType", Random.Range(-2f, 2f));
        agent.SetDestination(transform.position);
        //transform.LookAt(player);
        
        if (!haveAttacked)
        {
            haveAttacked = true;
            Invoke(nameof(ResetAttack), attackDelay);
        }
    }

    private void ResetAttack()
    {
         haveAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
