using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;
using DarkTonic.MasterAudio;



public class FoxMovement : MonoBehaviour
{
    [Header("Declaromg stuff")]

    private VisionConicaTower visionConicaTower;
    public float visionConicaAux;

    public Transform player;
    public Animator Animator;
    public ParticleSystem sawPlayerVFX;

    private NavMeshAgent agent;
    private int currentWaypointIndex = 0;
    private float timeSinceLastSeen = Mathf.NegativeInfinity;

    public float chaseDuration = 2f;
    public float stopDuration = 1f;
    private float stopTimer = 0f;

    // Rotation Dude
    [Header("Rotation Stuff")]

    public float rotationInterval = 6f; // cada cu�ntos segundos gira 180 grados
    private float rotationTimer = 0f;
    private Quaternion targetRotation;
    private bool isRotating = false;
    public float rotationSpeed = 180f; // grados por segundo
    private Vector3 initialPosition;
    public Transform graffitiTarget; // Asigna este desde el inspector
    private bool lookTowardsTarget = true;

    private float offsetTimer = 0f;
    private float offsetInterval;// Cambiar cada 3 segundos
    private bool offsetFlipped = false;
    private float targetOffset = 0f;
    public float offsetLerpSpeed = 120f; // Velocidad en grados por segundo

    [Header("Limites rotacion")]
    public float limit1;
    public float limit2;

    public float offsetInterval1;
    public float offsetInterval2;

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

        initialPosition = transform.position; // Guardamos la posici�n original

        agent.isStopped = true; // No queremos que camine hasta que sea necesario

        LookAtTarget(graffitiTarget);
        visionConicaAux = visionConicaTower.distanciaVision;

    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case State.Patrolling:
                Patrol();
                visionConicaTower.distanciaVision = visionConicaAux;
                break;
            case State.Stopping:
                StopAndObserve();
                break;
            case State.Chasing:
                Chase();
                visionConicaTower.distanciaVision = visionConicaAux / 1.5f;

                break;
        }
    }
    private void Patrol()
    {
        float distance = Vector3.Distance(transform.position, initialPosition);
        LookAtTarget(graffitiTarget);
        if (!visionConicaTower.canSeePlayer && distance > 0.5f)
        {
            agent.SetDestination(initialPosition);
            return;
        }
        if (visionConicaTower != null)
        {
            offsetTimer += Time.deltaTime;
            if (offsetTimer >= offsetInterval)
            {
                offsetFlipped = !offsetFlipped;
                targetOffset = offsetFlipped ? limit1 : limit2;
                offsetTimer = 0f;
                //Animaciones
                if (targetOffset == limit2)
                {
                    offsetInterval = offsetInterval2;
                    Animator.SetBool("chaseToggle", false);
                    Animator.SetBool("girarseToggle", false);
                    Animator.SetBool("pintarToggle", true);
                }
                if (targetOffset == limit1){
                    offsetInterval = offsetInterval1;
                    Animator.SetBool("chaseToggle", false);
                    Animator.SetBool("girarseToggle", true);
                    Animator.SetBool("pintarToggle", false);
                }
            }

            // Interpolaci�n suave del offsetAngulo hacia targetOffset
            visionConicaTower.offsetAngulo = Mathf.MoveTowards(
                visionConicaTower.offsetAngulo,
                targetOffset,
                offsetLerpSpeed * Time.deltaTime
            );
        }

        if (visionConicaTower != null && visionConicaTower.canSeePlayer)
        {
            currentState = State.Stopping;
            agent.isStopped = true;
            stopTimer = 0f;
            return;
        }

        // Si no se ve al jugador, patrullar sin hacer nada m�s
        agent.isStopped = false;
        agent.SetDestination(initialPosition);
    }

    private void StopAndObserve()
    {
        if (visionConicaTower != null && visionConicaTower.canSeePlayer)
        {
            stopTimer += Time.deltaTime;
            if (stopTimer >= stopDuration)
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
            agent.SetDestination(initialPosition);
        }
    }
    private void Chase()
    {
        Animator.SetBool("chaseToggle", true);
        Animator.SetBool("girarseToggle", false);
        Animator.SetBool("pintarToggle", false);
        // Interpolaci�n suave del offsetAngulo hacia estar recto
        visionConicaTower.offsetAngulo = Mathf.MoveTowards(
            visionConicaTower.offsetAngulo,
            0,
            offsetLerpSpeed * Time.deltaTime
        );
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
            Animator.SetBool("chaseToggle", false);
            Animator.SetBool("girarseToggle", false);
            //Animator.SetBool("pintarToggle", true);
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
