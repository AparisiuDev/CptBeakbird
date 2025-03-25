using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody rb;
    public CharacterController characterController;
    
    // Referencia a la cámara principal
    
    // Vector para almacenar la dirección de movimiento
    private Vector3 moveDirection;
    
    // Variables para almacenar el input del jugador
    private float horizontalInput;
    private float verticalInput;

public float debugSpeed;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    // Capturamos el input en Update para mayor responsividad
    void Update()
    {
        // Usamos GetAxisRaw para un control más directo sin suavizado
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // Procesamos la dirección de movimiento basada en el input
        ProcessMovementDirection();
        

        //rb.velocity = moveDirection * speed;

        characterController.Move(moveDirection * Time.deltaTime);
    }

    // Aplicamos el movimiento físico en FixedUpdate
    void FixedUpdate()
    {
        // Aplicamos la velocidad al rigidbody
        debugSpeed = rb.velocity.magnitude;
    }
    
    // Método para procesar la dirección de movimiento
    private void ProcessMovementDirection()
    {
        // Crear un vector de dirección basado en la entrada del usuario
        Vector3 inputDirection = new Vector3(horizontalInput, 0, verticalInput).normalized;
        
        if (inputDirection.magnitude > 0)
        {
            // Convertir la dirección de entrada a una dirección relativa a la cámara
            // Para una cámara a 45 grados, necesitamos ajustar la dirección
            float cameraRotationY = Camera.main.transform.eulerAngles.y; // Ángulo de la cámara en el eje Y
            
            // Crear una matriz de rotación basada en el ángulo de la cámara
            Quaternion cameraRotation = Quaternion.Euler(0, cameraRotationY, 0);
            
            // Aplicar la rotación a la dirección de entrada y asegurar que sigue normalizada
            moveDirection = (cameraRotation * inputDirection).normalized;
            
            // Opcional: Rotar el personaje hacia la dirección del movimiento
            if (moveDirection != Vector3.zero)
            {
                transform.forward = moveDirection;
            }
        }
        else
        {
            moveDirection = Vector3.zero;
        }
    }
}
