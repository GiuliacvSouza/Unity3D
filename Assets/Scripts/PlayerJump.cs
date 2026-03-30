using UnityEngine;

// Controla o pulo simples e o pulo duplo do player
public class PlayerJump : MonoBehaviour
{
    [Header("Configurações de Pulo")]
    public float forcaPulo1 = 6f;   // Força do primeiro pulo (do chão)
    public float forcaPulo2 = 9f;   // Força do segundo pulo (no ar)

    private Rigidbody rb;
    private int pulosFeitos = 0;    // Contador de pulos (máximo 2)
    private bool estaNoChao;        // True quando o player está tocando o chão

    public ScoreManager scoreManager; // Referência para parar o cronômetro em colisão direta

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Não processa pulo se o jogo estiver pausado
        if (Time.timeScale == 0) return;

        // Pula se apertar espaço E estiver no chão OU ainda tiver pulo duplo disponível
        if (Input.GetKeyDown(KeyCode.Space) && (estaNoChao || pulosFeitos < 2))
        {
            Pular();
        }
    }

    void Pular()
    {
        // Zera a velocidade vertical para o pulo duplo ter força consistente
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);

        if (pulosFeitos == 0)
        {
            // Primeiro pulo: força menor (saída do chão)
            rb.AddForce(Vector3.up * forcaPulo1, ForceMode.Impulse);
        }
        else
        {
            // Segundo pulo: força maior (pulo duplo no ar)
            rb.AddForce(Vector3.up * forcaPulo2, ForceMode.Impulse);
        }

        pulosFeitos++;
        estaNoChao = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Ao tocar o chão, reseta o contador de pulos
        if (collision.gameObject.CompareTag("Ground"))
        {
            estaNoChao = true;
            pulosFeitos = 0;
        }

        // Colisão direta com obstáculo (fallback — o fluxo principal passa pelo PlayerCollision)
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Game Over!");
            if (scoreManager != null) scoreManager.PararCronometro();
            this.enabled = false; // Desativa o script de pulo
        }
    }
}
