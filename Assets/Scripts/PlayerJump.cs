using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    //alterar depois de acordo com os obstaculos
    public float forcaPulo1 = 6f; // Primeiro pulo
    public float forcaPulo2 = 9f;   // Segundo pulo (pulo duplo)
    
    private Rigidbody rb;
    private int pulosFeitos = 0;
    private bool estaNoChao;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Se apertar Espaço E (estiver no chão OU ainda tiver o pulo duplo sobrando)
        if (Input.GetKeyDown(KeyCode.Space) && (estaNoChao || pulosFeitos < 2))
        {
            Pular();
        }
    }

    void Pular()
    {
        // Zera a velocidade vertical antes de pular (importante para o pulo duplo ser consistente)
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);

        if (pulosFeitos == 0)
        {
            // Primeiro Pulo
            rb.AddForce(Vector3.up * forcaPulo1, ForceMode.Impulse);
        }
        else
        {
            // Segundo Pulo
            rb.AddForce(Vector3.up * forcaPulo2, ForceMode.Impulse);
        }

        pulosFeitos++;
        estaNoChao = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Quando encosta no chão, reseta o contador
        if (collision.gameObject.CompareTag("Ground"))
        {
            estaNoChao = true;
            pulosFeitos = 0;
        }
    }
}