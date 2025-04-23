using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    private CharacterController controller;

    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float sprintSpeed = 10f;
    [SerializeField] private float rotationSpeed;

    [Header("Gravity Settings")]
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float groundGravity = -2f;  // A smaller gravity when grounded

    [Header("Stamina Settings")]
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float staminaDrain = 20f;     // per second
    [SerializeField] private float staminaRegen = 15f;     // per second
    [SerializeField] private float staminaRegenDelay = 1f; // seconds before regen starts

    private float currentStamina;
    private float regenTimer;
    private bool isSprinting;

    public Image StaminaBar;

    [Header("Input")]
    private float moveInput;
    private float turnInput;

    private Quaternion toRotation;
    private Vector3 move;

    // Gravity & Movement
    private Vector3 velocity;
    private bool isGrounded;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        currentStamina = maxStamina;
    }

    private void Update()
    {
        InputManagement();
        Movement();
        UpdateRotation();
        HandleStamina();
    }

    private void InputManagement()
    {
        moveInput = Input.GetAxisRaw("Vertical");
        turnInput = Input.GetAxisRaw("Horizontal");
        move = new Vector3(turnInput, 0, moveInput);
        move.Normalize();

        // Sprinting if holding Shift and moving
        isSprinting = Input.GetKey(KeyCode.LeftShift) && move.magnitude > 0 && currentStamina > 0;
    }

    private void Movement()
    {
        // Check if the player is grounded
        isGrounded = controller.isGrounded;

        // Movement speed based on sprinting status
        float currentSpeed = isSprinting ? sprintSpeed : walkSpeed;

        // Apply movement based on input and camera direction (no Y-axis)
        Vector3 horizontalMove = move * currentSpeed;
        horizontalMove.y = 0;

        // Apply gravity
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = groundGravity; // Keep player grounded with a small gravity
        }
        else
        {
            velocity.y += gravity * Time.deltaTime; // Apply gravity
        }

        // Combine horizontal movement and gravity
        Vector3 totalMovement = (horizontalMove + velocity) * Time.deltaTime;
        controller.Move(totalMovement);
    }

    private void UpdateRotation()
    {
        if (move != Vector3.zero)
        {
            toRotation = Quaternion.LookRotation(move, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed);
        }
    }

    private void HandleStamina()
    {
        StaminaBar.fillAmount = currentStamina / maxStamina;

        if (isSprinting)
        {
            currentStamina -= staminaDrain * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
            regenTimer = staminaRegenDelay; // Reset the regen delay when sprinting
        }
        else
        {
            regenTimer -= Time.deltaTime;
            if (regenTimer <= 0)
            {
                currentStamina += staminaRegen * Time.deltaTime;
                currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
            }
        }
    }
}
