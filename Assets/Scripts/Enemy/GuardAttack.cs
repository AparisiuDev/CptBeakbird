using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardAttack : MonoBehaviour
{
    public Transform attackPoint;
    public float attackRange = 1.5f;

    public float attackRadius = 0.8f;

    public float attackCooldown = 1.5f;
    public int attackDamage = 20;
    public LayerMask playerLayer;

    private GuardMovement guardMovement;
    private NavMeshAgent agent;
    private float lastAttackTime;
    private PlayerHealth health;


    // Start is called before the first frame update
    void Start()
    {
        guardMovement = GetComponent<GuardMovement>();
        agent = GetComponent<NavMeshAgent>();

        if(guardMovement.player != null )
        {
            health = guardMovement.player.GetComponent<PlayerHealth>(); 
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Comprobamos que esten todos los componentes necesarios
        if (guardMovement == null || guardMovement.player == null || health == null)
            return;

        //Comprobamos si el jugador esta vivo
        if (health.IsDead())
        {
            agent.isStopped = false;
            guardMovement.ReturnToPatrol();
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, guardMovement.player.position);

        //Si el jugador esta a rango le intentamos atacar
        if (distanceToPlayer <= attackRange)
        {
            agent.isStopped = true;
            TryAttack();
        }
        else
        {
            agent.isStopped = false;
        }
    }

    private void TryAttack()
    {
        if(Time.time - lastAttackTime >= attackCooldown)
        {
            lastAttackTime = Time.time;
            Collider[] hitCollider = Physics.OverlapSphere(attackPoint.position,
                attackRadius,
                playerLayer);

            foreach(Collider col in hitCollider)
            {
                PlayerHealth h = col.GetComponent<PlayerHealth>();
                if(h != null)
                {
                    health.TakeDamage(attackDamage);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
            Gizmos.color = new Color(1, 0, 0, 0.3f);
            Gizmos.DrawSphere(attackPoint.position, attackRadius);
    }
}
