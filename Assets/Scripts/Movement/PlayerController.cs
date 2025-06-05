using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Animations;
using DarkTonic.MasterAudio;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    private CharacterController controller;
    public InventoryManager inventoryManager;

    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float sprintSpeed = 10f;
    [SerializeField] private float rotationSpeed;
    public bool inputEnabled = true;

    [Header("Gravity Settings")]
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float groundGravity = -2f;  // A smaller gravity when grounded

    [Header("Stamina Settings")]
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float staminaDrain = 20f;     // per second
    [SerializeField] private float staminaRegen = 15f;     // per second
    [SerializeField] private float staminaRegenDelay = 1f; // seconds before regen starts

    [Header("Idle Break settings")]
    [SerializeField] private int frameCounter;


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

    [Header("SFX Manager")]
    public GaviotoSFXManager sounds;
    private bool isWalking;
    private bool isRunning;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        currentStamina = maxStamina;
    }

    private void Update()
    {
        if(!inputEnabled) return;
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

        
        /*** Manager Animations ***/
        ExtraIdleExitCheck();
        if (!isSprinting && horizontalMove != Vector3.zero)
        {
            isRunning = false;
            MasterAudio.StopAllOfSound("Correr");

            Animations.AnimatorManager.myAnimator.SetBool("isWalking", true);
            Animations.AnimatorManager.myAnimator.SetBool("isRunning", false);
            ExitIdle();
            // SOUNDS //
            //sounds.WalkingSFX();
            if(!isWalking) // Play sound only if not already walking
                MasterAudio.PlaySound("Caminar");
            isWalking = true;
        }
        if (isSprinting && horizontalMove != Vector3.zero)
        {
            isWalking = false;
            MasterAudio.StopAllOfSound("Caminar");
            //Animations.AnimatorManager.myAnimator.SetBool("isWalking", false);
            Animations.AnimatorManager.myAnimator.SetBool("isRunning", true);
            ExitIdle();

            if (!isRunning) // Play sound only if not already walking
                MasterAudio.PlaySound("Correr");
            isRunning = true;

        }
        else if (horizontalMove == Vector3.zero)
        {
            Animations.AnimatorManager.myAnimator.SetBool("isWalking", false);
            Animations.AnimatorManager.myAnimator.SetBool("isRunning", false);
            Tick();
            // SOUNDS //
            //sounds.IdleSFX();
            MasterAudio.StopAllOfSound("Caminar");
            MasterAudio.StopAllOfSound("Correr");

            isWalking = false;
            isRunning = false;

        }

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
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * inventoryManager.SlowDown);
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

    /*** IDLE BREAK ***/
    public void Tick()
    {
        frameCounter++;

        if (frameCounter > 3000 && frameCounter % 20 == 0)
        {
            // Chance to do the action
            if (Random.value < 0.2f)
            {
                frameCounter = 0;
                Animations.AnimatorManager.myAnimator.SetBool("idleBreakToggle", true);
            }
        }
    }

    public void ExitIdle()
    {
        if (Animations.AnimatorManager.myAnimator.GetCurrentAnimatorStateInfo(0).IsName("IdleBreak"))
        {
            Animations.AnimatorManager.myAnimator.SetBool("idleBreakToggle", false);
        }
    }

    public void ExtraIdleExitCheck()
    {
        if (Animations.AnimatorManager.myAnimator.GetCurrentAnimatorStateInfo(0).IsName("IdleBreak"))
        {
            // normalizedTime = 1.0 means the animation has finished one full loop
            if (Animations.AnimatorManager.myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.975f)
            {
                Animations.AnimatorManager.myAnimator.SetBool("idleBreakToggle", false);
            }
        }
    }
}
