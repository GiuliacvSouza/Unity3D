using UnityEngine;

// Controla o pulo simples e o pulo duplo do player
public class PlayerJump : MonoBehaviour
{
    [Header("Configurações de Pulo")]
    public float forcaPulo1 = 6f;
    public float forcaPulo2 = 9f;

    [Header("Formato do Pulo")]
    public float fallMultiplier = 3f;   // Quanto maior, mais curto o pulo
    public float maxFallSpeed = -25f;   // Limite da queda (evita absurdos)

    private Rigidbody rb;
    private int pulosFeitos = 0;
    private bool estaNoChao;

    public ScoreManager scoreManager;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Time.timeScale == 0) return;

        if (Input.GetKeyDown(KeyCode.Space) && (estaNoChao || pulosFeitos < 2))
        {
            Pular();
        }

        AplicarGravidadeAjustada();
    }

    void Pular()
    {
        // Mantém consistência do pulo (importantíssimo pro design)
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);

        if (pulosFeitos == 0)
        {
            rb.AddForce(Vector3.up * forcaPulo1, ForceMode.Impulse);
        }
        else
        {
            rb.AddForce(Vector3.up * forcaPulo2, ForceMode.Impulse);
        }

        pulosFeitos++;
        estaNoChao = false;
    }

    void AplicarGravidadeAjustada()
    {
        // Só altera a DESCIDA → mantém altura do pulo
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;

            // Clamp da velocidade de queda
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
            pulosFeitos = 0;
        }

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Game Over!");
            if (scoreManager != null) scoreManager.PararCronometro();
            this.enabled = false;
        }
    }
}