using UnityEngine;
//using MaskTransitions;
using EasyTransition;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public bool preHasFinishedDiedAnim = false;
    public bool hasFinishedDiedAnim = false;
    public bool hasFinishedCircle;
    private PlayerController playerController;
    public TransitionSettings transition;
    public TransitionManager transitionManager;
    public Transform cameraTransform;


    void Start()
    {
        //Animations.AnimatorManager.myAnimator.SetBool("hasDied", false);
        currentHealth = maxHealth;
        preHasFinishedDiedAnim = false;
        hasFinishedDiedAnim = false;
        playerController = GetComponent<PlayerController>();
    }
    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Die();
        }
        if (preHasFinishedDiedAnim)
        {
            transitionManager.Transition("LevelSelect", transition, 2.5f);
            preHasFinishedDiedAnim = false;

        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log("Player Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Animations.AnimatorManager.myAnimator.SetTrigger("hasDied");
        Debug.Log("Player has died!");
        playerController.inputEnabled = false;
        RotateToFaceCameraInstant();
        preHasFinishedDiedAnim = true;
        // normalizedTime = 1.0 means the animation has finished one full loop
        
    }

    public bool IsDead()
    {
        return currentHealth <= 0;
    }

    public void RotateToFaceCameraInstant()
    {
        Vector3 direction = cameraTransform.position - transform.position;
        direction.y = 0f; // ignore vertical

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = targetRotation; // instant rotation
        }
    }

    /****
    public void Tick()
    {
        //Debug.Log(hasFinishedDiedAnim);
        if (Animations.AnimatorManager.myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            hasFinishedDiedAnim = true;
            
        }
    }
    ***/
}