using UnityEngine;

public class FishJump : MonoBehaviour
{
    public Vector3 offset = new Vector3(2f, 0, 0);
    public float jumpHeight = 2f;
    public float jumpDuration = 1f;
    public float jumpInterval = 3f;

    public GameObject spawnParticlesPrefab;
    public GameObject despawnParticlesPrefab;

    private Vector3 startPos;
    private Vector3 endPos;
    private float timer;
    private bool isJumping = false;

    private float cooldownTimer = 0f;
    private Renderer rend;

    private bool isFirstJump = true;
    private float firstJumpDelay;

    void Start()
    {
        startPos = transform.position;
        endPos = startPos + offset;

        rend = GetComponent<Renderer>();
        if (rend != null)
            rend.enabled = false;

        // Generar retardo aleatorio para el primer salto (entre 1 y 6 segundos)
        firstJumpDelay = Random.Range(1f, 6f);
    }

    void Update()
    {
        if (isJumping)
        {
            timer += Time.deltaTime;
            float t = timer / jumpDuration;

            if (t >= 1f)
            {
                transform.position = startPos;
                isJumping = false;
                timer = 0f;

                if (rend != null)
                    rend.enabled = false;

                SpawnParticlesAtPosition(despawnParticlesPrefab, endPos);
                return;
            }

            Vector3 currentPos = GetParabolicPosition(t);
            Vector3 nextPos = GetParabolicPosition(t + 0.01f);

            transform.position = currentPos;

            Vector3 direction = nextPos - currentPos;
            if (direction != Vector3.zero)
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            }
        }
        else
        {
            cooldownTimer += Time.deltaTime;

            float targetInterval = isFirstJump ? firstJumpDelay : jumpInterval;

            if (cooldownTimer >= targetInterval)
            {
                isJumping = true;
                timer = 0f;
                cooldownTimer = 0f;

                if (rend != null)
                    rend.enabled = true;

                SpawnParticlesAtPosition(spawnParticlesPrefab, transform.position);

                isFirstJump = false; // ya no es el primer salto
            }
        }
    }

    private Vector3 GetParabolicPosition(float t)
    {
        t = Mathf.Clamp01(t);
        Vector3 horizontal = Vector3.Lerp(startPos, endPos, t);
        float height = 4 * jumpHeight * t * (1 - t);
        return new Vector3(horizontal.x, startPos.y + height, horizontal.z);
    }

    private void SpawnParticlesAtPosition(GameObject particlePrefab, Vector3 position)
    {
        if (particlePrefab != null)
        {
            GameObject particlesInstance = Instantiate(particlePrefab, position, Quaternion.identity);
            Destroy(particlesInstance, 3f);
        }
    }
}
