using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;

public class EnemyChaser : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private float angularSpeed = 10f;
    [SerializeField] private float acceleration = 60f;
    [SerializeField] private int damage = 30;
    [SerializeField] private float attackRate = 0.5f;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float noticeRange = 15f;
    [SerializeField] private LayerMask targetLayers;
    private bool los;

    [Space]
    float navMeshRefreshRate = 0.1f;
    float time;
    float attackTime;

    Ray ray;

    [Header("References")]
    private NavMeshAgent agent;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private PlayerInfoSO playerInfo;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        time = 0f;
        attackTime = 0f;

        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        agent.stoppingDistance = attackRange * 2;
        agent.angularSpeed = angularSpeed;
        agent.acceleration = acceleration;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        attackTime += Time.deltaTime;

        CheckLos();

        if (time > navMeshRefreshRate)
        {
            if (los)
            {
                agent.SetDestination(playerInfo.playerPosition);
                RotateAtPlayer();
                if ((playerInfo.playerPosition - transform.position).magnitude < attackRange * 2)
                {
                    animator.SetBool("IsAttacking", true);
                }
                else
                {
                    animator.SetBool("IsAttacking", false);
                }
            }

            time = 0f;
        }

        animator.SetFloat("Speed", agent.velocity.magnitude/agent.speed);
    }

    void CheckLos()
    {
        Vector3 offset = new Vector3(0f, 1.5f, 0f);
        Vector3 direction = (playerInfo.playerPosition + offset) - (transform.position + offset);

        ray = new Ray(transform.position + offset, direction);
        los = Physics.Raycast(ray, noticeRange, targetLayers);
    }

    void RotateAtPlayer()
    { 
        transform.LookAt(playerInfo.playerPosition);
    }

    public void Attack()
    {
        Collider[] hits = Physics.OverlapSphere(attackPoint.position, attackRange, targetLayers);
        Debug.Log(hits);
        if (hits.Length > 0)
        {
            foreach (Collider c in hits)
            {
                if (c.gameObject.TryGetComponent(out PlayerHealth playerhealth))
                {
                    playerhealth.TakeDamage(damage);
                    Debug.Log(damage + " damage was taken");
                    break;
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        if (los)
        {
            Gizmos.color = Color.red;
        }
        else
        {
            Gizmos.color = Color.blue;
        }
        Gizmos.DrawRay(ray);
        Gizmos.DrawSphere(attackPoint.position, attackRange);

    }

}
