using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class GuardMovement : MonoBehaviour
{
    public Transform[] waypoints;
    //public TowerDetectorMultiRay towerDetectorMultiRay;
    private VisionConica visionConica;
    public Transform player;


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
        visionConica = GetComponent<VisionConica>();
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
                Patrol();
                break;
            case State.Stopping:
                StopAndObserve();
                break;
            case State.Chasing:
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
            stopTimer += Time.deltaTime;
            if(stopTimer >= stopDuration)
            {
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

    private void playerHPManager()
    {

    }
}
