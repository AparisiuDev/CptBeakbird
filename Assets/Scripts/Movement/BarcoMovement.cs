using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarcoMovement : MonoBehaviour
{
    public Animator remador;
    public Animator remoIZQ;
    public Animator remoDER;
    [Header("References")]
    private CharacterController controller;

    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float rotationSpeed = 100f;

    [Header("Input")]
    private float moveInput;
    private float turnInput;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        //Debug.Log(LevelLocker.VariablesGlobales._lvl1);
        InputManagement();
        Movement();
    }

    private void InputManagement()
    {
        moveInput = Input.GetAxis("Vertical");

        // Bloquear movimiento hacia atrás
        if (moveInput <= 0)
            moveInput = 0;

        turnInput = Input.GetAxis("Horizontal");
    }


    private void Movement()
    {
        // Movimiento hacia adelante o atrás
        Vector3 forward = transform.forward * moveInput;
        controller.SimpleMove(forward * walkSpeed);

        // Invertir el giro si estamos en reversa
        float effectiveTurn = moveInput < 0 ? -turnInput : turnInput;

        // Rotación
        transform.Rotate(Vector3.up * effectiveTurn * rotationSpeed * Time.deltaTime);

        if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            if (moveInput <= 0 && turnInput == 0) return;
            remador.SetBool("remar", true);
            remoIZQ.SetBool("remar", true);
            remoDER.SetBool("remar", true);
        }
        else
        {
            remador.SetBool("remar", false);
            remoIZQ.SetBool("remar", false);
            remoDER.SetBool("remar", false);
        }
    }
}
