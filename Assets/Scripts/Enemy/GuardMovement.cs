using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;


public class GuardMovement : MonoBehaviour
{
    public Transform[] waypoints;
    //public TowerDetectorMultiRay towerDetectorMultiRay;
    public VisionConica visionConica;
    public Transform player;
    public Animator animator; // Asigna el Animator en el Inspector
    public ParticleSystem sawPlayerVFX;

    private NavMeshAgent agent;
    private int currentWaypointIndex = 0;
    private float timeSinceLastSeen = Mathf.NegativeInfinity;

    public float chaseDuration = 2f;
    public float stopDuration = 1f;
    private float stopTimer = 0f;

    private enum State
    {
        Patrolling,
        Stopping,
        Chasing,
    }

    private State currentState = State.Patrolling;


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        //Si existen suficientes waypoints asingamos uno
        if(waypoints.Length > 0 )
        {
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch(currentState)
        {
            case State.Patrolling:
                animator.SetBool("walkingToggle", true);
                Patrol();
                break;
            case State.Stopping:
                animator.SetBool("walkingToggle", false); 
                StopAndObserve();
                break;
            case State.Chasing:
                animator.SetBool("walkingToggle", true);
                Chase();
                break;
        }
    }

    private void Patrol() {

        //Si vemos al jugador
        if(visionConica != null && visionConica.canSeePlayer)
        {
            //Paramos el agente y cambiamos el estado a parado
            currentState = State.Stopping; 
            agent.isStopped = true;
            stopTimer = 0f; //Reseteamos el tiempo parado
        }
        else
        {
            if(!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
                agent.SetDestination(waypoints[currentWaypointIndex].position);
            }
        }
    }
    private void StopAndObserve() {
        if (visionConica != null && visionConica.canSeePlayer)
        {
            agent.ResetPath();
            LookAtPlayer();
            stopTimer += Time.deltaTime;
            if(stopTimer >= stopDuration)
            {
                sawPlayerVFX.Play();
                currentState = State.Chasing;
                agent.isStopped = false;
                timeSinceLastSeen = Time.time;
            }
        }
        else
        {
            currentState = State.Patrolling;
            agent.isStopped = false;
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }
    }
    private void Chase() {
        if(visionConica != null && (visionConica.canSeePlayer || visionConica.activeChase))
        {
            visionConica.activeChase = true;
            timeSinceLastSeen = Time.time;
            agent.SetDestination(player.position);
        }
        else if(Time.time - timeSinceLastSeen < chaseDuration)
        {
            agent.SetDestination(player.position);
        }
        else
        {
            currentState = State.Patrolling;
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }
    }

    public void ReturnToPatrol()
    {
        currentState = State.Patrolling;
        agent.isStopped = false;
        agent.SetDestination(waypoints[currentWaypointIndex].position);
    }

    private void LookAtPlayer()
    {
        if (player != null)
        {
            Vector3 direccion = player.position - transform.position;
            direccion.y = 0; // Opcional: ignora la altura para rotar solo en el eje Y

            if (direccion != Vector3.zero)
            {
                Quaternion rotacionDeseada = Quaternion.LookRotation(direccion);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotacionDeseada, Time.deltaTime * 5f);
            }
        }
    }

}
