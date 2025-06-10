using UnityEngine;

public class IdleBreakController : MonoBehaviour
{
    public float idleDuration = 2f;   // Duración aproximada del Idle (segundos)
    public int idleRepetitions = 2;   // Cuántas veces repetir Idle antes de IdleBreak

    private Animator animator;
    private float timer;
    private int currentIdleCount = 0;

    void Start()
    {
        animator = GetComponent<Animator>();
        timer = idleDuration;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            currentIdleCount++;

            if (currentIdleCount >= idleRepetitions)
            {
                animator.SetTrigger("IdleBreak");
                currentIdleCount = 0; // Reset contador para la próxima vez
            }

            timer = idleDuration;  // Reset timer para el siguiente ciclo Idle
        }
    }
}
