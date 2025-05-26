using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;


public class TowerMovement : MonoBehaviour
{
    private VisionConicaTower visionConicaTower;
    public Transform player;

    private NavMeshAgent agent;
    private int currentWaypointIndex = 0;
    private float timeSinceLastSeen = Mathf.NegativeInfinity;

    public float chaseDuration = 2f;
    public float stopDuration = 1f;
    private float stopTimer = 0f;

    // Rotation Dude
    public float rotationInterval = 6f; // cada cuántos segundos gira 180 grados
    private float rotationTimer = 0f;
    private Quaternion targetRotation;
    private bool isRotating = false;
    public float rotationSpeed = 180f; // grados por segundo
    private Vector3 initialPosition;
    public Transform graffitiTarget; // Asigna este desde el inspector
    private bool lookTowardsTarget = true;



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
        visionConicaTower = GetComponent<VisionConicaTower>();
        agent = GetComponent<NavMeshAgent>();

        initialPosition = transform.position; // Guardamos la posición original

        agent.isStopped = true; // No queremos que camine hasta que sea necesario

        LookAtTarget(graffitiTarget);
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
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

    private void Patrol()
    {
        if (visionConicaTower != null && visionConicaTower.canSeePlayer)
        {
            currentState = State.Stopping;
            agent.isStopped = true;
            stopTimer = 0f;
            isRotating = false;
            return;
        }

        if (!isRotating)
        {
            rotationTimer += Time.deltaTime;
            if (rotationTimer >= rotationInterval)
            {
                Vector3 direction = graffitiTarget.position - transform.position;
                direction.y = 0f;

                if (direction != Vector3.zero)
                {
                    Quaternion lookRotation = Quaternion.LookRotation(direction);

                    if (lookTowardsTarget)
                    {
                        targetRotation = lookRotation;
                    }
                    else
                    {
                        targetRotation = Quaternion.Euler(0f, lookRotation.eulerAngles.y + 180f, 0f);
                    }

                    lookTowardsTarget = !lookTowardsTarget; // Alternar dirección
                    isRotating = true;
                    rotationTimer = 0f;
                }
            }
        }
        else
        {
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );

            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
            {
                isRotating = false;
            }
        }
    }
    private void StopAndObserve()
    {
        if (visionConicaTower != null && visionConicaTower.canSeePlayer)
        {
            stopTimer += Time.deltaTime;
            if (stopTimer >= stopDuration)
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
            agent.SetDestination(initialPosition);
        }
    }
    private void Chase()
    {
       // visionConicaTower.distanciaVision /= 2;
        if (visionConicaTower != null && (visionConicaTower.canSeePlayer || visionConicaTower.activeChase))
        {
            visionConicaTower.activeChase = true;
            timeSinceLastSeen = Time.time;
            agent.SetDestination(player.position);
        }
        else if (Time.time - timeSinceLastSeen < chaseDuration)
        {
            agent.SetDestination(player.position);
        }
        else
        {
            currentState = State.Patrolling;
            agent.SetDestination(initialPosition);
        }
    }

    public void ReturnToPatrol()
    {
       // visionConicaTower.distanciaVision *= 2;
        agent.isStopped = false;
        agent.SetDestination(initialPosition);
        if (!agent.pathPending && agent.remainingDistance < 0.01f)
        {
            LookAtTarget(graffitiTarget);
            currentState = State.Patrolling;
        }
    }

    void LookAtTarget(Transform target)
    {
        Vector3 direction = target.position - transform.position;
        direction.y = 0f; // This removes vertical tilt

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = targetRotation;
        }
    }
}
