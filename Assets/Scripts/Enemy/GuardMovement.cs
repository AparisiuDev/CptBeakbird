using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;
using DarkTonic.MasterAudio;



public class GuardMovement : MonoBehaviour
{
    public Transform[] waypoints;
    //public TowerDetectorMultiRay towerDetectorMultiRay;
    public VisionConica visionConica;
    public float visionConicaAux;

    public Transform player;
    public Animator animator; // Asigna el Animator en el Inspector
    public ParticleSystem sawPlayerVFX;
    private PlayerHealth playerHealth; // Referencia al script de salud del jugador


    private NavMeshAgent agent;
    private int currentWaypointIndex = 0;
    private float timeSinceLastSeen = Mathf.NegativeInfinity;

    [Header("Chase Settings")]
    public float chaseDuration = 2f;
    public float stopDuration = 1f;
    private float stopTimer = 0f;
    // Running speed
    [Header("Running Speed")]
    private float runSpeed;
    public float runSpeedMult;
    [Header("WaitAtWaypoint")]
    public bool waitAtWaypoint = true;
    public float waitTimeAtWaypoint = 2f;
    private float waitTimer = 0f;
    [Header("Patrol Look At Settings")]
    public bool lookAtDuringPatrol;
    public Transform lookAtTarget;

    public enum State
    {
        Patrolling,
        Stopping,
        Chasing,
    }

    public State currentState = State.Patrolling;


    // Start is called before the first frame update
    void Start()
    {
        playerHealth = player.GetComponent<PlayerHealth>();
        agent = GetComponent<NavMeshAgent>();
        visionConicaAux = visionConica.distanciaVision;
        //Si existen suficientes waypoints asingamos uno
        if(waypoints.Length > 0 )
        {
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }
        runSpeed = agent.speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerHealth.IsDead())
        {
            animator.SetBool("walkingToggle", true);
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }
        if (playerHealth.IsDead()) return;
        // No pasa si estas muerto
        switch(currentState)
        {
            case State.Patrolling:
                animator.SetBool("runningToggle", false);
                animator.SetBool("walkingToggle", true);
                visionConica.distanciaVision = visionConicaAux;
                agent.speed = runSpeed;
                Patrol();
                break;
            case State.Stopping:
                animator.SetBool("walkingToggle", false);
                animator.SetBool("runningToggle", false);
                StopAndObserve();
                break;
            case State.Chasing:
                animator.SetBool("runningToggle", true);
                animator.SetBool("walkingToggle", true);
                visionConica.distanciaVision = visionConicaAux / 1.5f;
                agent.speed = runSpeed * runSpeedMult; // Aumentamos la velocidad al correr
                Chase();
                break;
        }
    }

    private void Patrol()
    {
        if (visionConica != null && visionConica.canSeePlayer)
        {
            currentState = State.Stopping;
            agent.isStopped = true;
            stopTimer = 0f;
            return;
        }

        // Si ya llegó al waypoint
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            if (waitAtWaypoint)
            {
                animator.SetBool("walkingToggle", false);
                animator.SetBool("runningToggle", false);
                agent.isStopped = true;
                waitTimer += Time.deltaTime;
                if (lookAtDuringPatrol && lookAtTarget != null)
                {
                    LookAtTarget(lookAtTarget);
                }

                if (waitTimer >= waitTimeAtWaypoint)
                {
                    animator.SetBool("walkingToggle", true);
                    waitTimer = 0f;
                    agent.isStopped = false;
                    currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
                    agent.SetDestination(waypoints[currentWaypointIndex].position);
                }
            }
            else
            {
                animator.SetBool("walkingToggle", true);
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
                MasterAudio.PlaySound("Alert");
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

    private void LookAtTarget(Transform target)
    {
        if (target != null)
        {
            Vector3 direccion = target.position - transform.position;
            direccion.y = 0;

            if (direccion != Vector3.zero)
            {
                Quaternion rotacionDeseada = Quaternion.LookRotation(direccion);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotacionDeseada, Time.deltaTime * 5f);
            }
        }
    }

}
