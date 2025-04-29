using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerDetectorMultiRay : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private int numberOfRays = 5;
    [SerializeField] private float fieldOfView = 45f;
    [SerializeField] private float rayLength = 20f; // NUEVA VARIABLE MODIFICABLE

    public bool canSeePlayer = false;

    public struct RayInfo
    {
        public Vector3 direction;
        public bool hitSomething;
        public Vector3 hitPoint;
        public bool hitPlayer;
        public float hitDistance;
    }

    private RayInfo[] rayInfos;

    public int NumberOfRays => numberOfRays;
    public float FieldOfView => fieldOfView;
    public float RayLength => rayLength;
    public LayerMask LayerMask => layerMask;
    public RayInfo[] RayInfos => rayInfos;

    private void Awake()
    {
        rayInfos = new RayInfo[numberOfRays];
    }

    private void FixedUpdate()
    {
        float angleStep = fieldOfView / (numberOfRays - 1);
        float startAngle = -fieldOfView / 2;

        canSeePlayer = false;

        for (int i = 0; i < numberOfRays; i++)
        {
            float angle = startAngle + (angleStep * i);
            Vector3 direction = Quaternion.Euler(0, angle, 0) * transform.forward;

            RaycastHit hit;

            rayInfos[i].direction = direction;
            rayInfos[i].hitSomething = false;
            rayInfos[i].hitPlayer = false;
            rayInfos[i].hitDistance = rayLength;
            rayInfos[i].hitPoint = transform.position + direction * rayLength;

            if (Physics.Raycast(transform.position, direction, out hit, rayLength, layerMask))
            {
                rayInfos[i].hitSomething = true;
                rayInfos[i].hitPoint = hit.point;
                rayInfos[i].hitDistance = hit.distance;

                Debug.DrawRay(transform.position, direction * hit.distance, Color.yellow);
                if (hit.collider.CompareTag("Player"))
                {
                    canSeePlayer = true;
                    rayInfos[i].hitPlayer = true;
                }
            }
            else
            {
                Debug.DrawRay(transform.position, direction * rayLength, Color.white);
            }
        }
    }
}
