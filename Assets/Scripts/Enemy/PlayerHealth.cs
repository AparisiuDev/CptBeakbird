using UnityEngine;


public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public bool hasFinishedDiedAnim;
    private PlayerController playerController;
    public Transform cameraTransform;

    void Start()
    {
        currentHealth = maxHealth;
        hasFinishedDiedAnim = false;
        playerController = GetComponent<PlayerController>();
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

    void Die()
    {
        Debug.Log("Player has died!");
        playerController.inputEnabled = false;
        Animations.AnimatorManager.myAnimator.SetTrigger("hasDied");
        RotateToFaceCameraInstant();
        // normalizedTime = 1.0 means the animation has finished one full loop
        if (Animations.AnimatorManager.myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 3.4f)
        {
            hasFinishedDiedAnim = true;
        }
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


}