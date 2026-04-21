using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [Header("Pulo")]
    public float jumpBaseForce = 6f;
    public float extraHeightMultiplier = 1.2f;

    [Header("Detecção")]
    public float detectionDistance = 10f;
    public float rayHeight = 1f;
    public LayerMask obstacleLayer;

    [Header("Gravidade")]
    public float fallMultiplier = 3f;
    public float maxFallSpeed = -25f;

    private Rigidbody rb;
    private bool estaNoChao;

    public ScoreManager scoreManager;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Time.timeScale == 0) return;

        if (Input.GetKeyDown(KeyCode.Space) && estaNoChao)
        {
            PularAdaptativo();

            if(AudioManager.Instance != null)
            {
                AudioManager.Instance.TocarPulo();
            } else{
                Debug.LogWarning("AudioManager não encontrado! Som de pulo não tocou.");
            }
        }

        AplicarGravidadeAjustada();
    }

    void PularAdaptativo()
    {
        float jumpForce = jumpBaseForce;

        // Origem do raycast (um pouco acima do chão)
        Vector3 origin = transform.position + Vector3.up * rayHeight;

        RaycastHit hit;

        if (Physics.Raycast(origin, Vector3.forward, out hit, detectionDistance, obstacleLayer))
        {
            // Tenta pegar a altura do obstáculo
            Renderer rend = hit.collider.GetComponent<Renderer>();

            if (rend != null)
            {
                float obstacleHeight = rend.bounds.size.y;

                // Ajusta força baseada na altura
                jumpForce = Mathf.Max(jumpBaseForce, obstacleHeight * extraHeightMultiplier);
            }
        }

        // Limpa velocidade vertical
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);

        // Aplica pulo
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        estaNoChao = false;
    }

    void AplicarGravidadeAjustada()
    {
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;

            if (rb.linearVelocity.y < maxFallSpeed)
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, maxFallSpeed, rb.linearVelocity.z);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            estaNoChao = true;
        }

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Game Over!");

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.TocarGameOver();
            }


            if (scoreManager != null)
                scoreManager.PararCronometro();

            this.enabled = false;
        }
    }
}