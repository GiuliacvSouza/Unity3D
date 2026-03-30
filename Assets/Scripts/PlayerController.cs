using UnityEngine;

// Controla o movimento lateral do player entre 3 faixas (esquerda, centro, direita)
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Configurações de Faixa")]
    [SerializeField] private float laneDistance = 3.0f;  // Distância entre as faixas no eixo Z
    [SerializeField] private float laneSpeed = 10.0f;    // Velocidade de transição entre faixas
    [SerializeField] private bool invertControls = true; // Inverte esquerda/direita (perspectiva de câmera)

    private Rigidbody rb;
    private int currentLane = 1;    // Faixa atual: 0 = esquerda, 1 = centro, 2 = direita
    private float initialZ;         // Posição Z inicial (centro)
    private Vector3 targetPosition; // Posição alvo calculada para suavização

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        initialZ = transform.position.z;
        targetPosition = transform.position;
    }

    void Update()
    {
        // Não processa input se o jogo estiver pausado
        if (Time.timeScale == 0) return;

        HandleInput();
        CalculateLanePosition();
    }

    void FixedUpdate()
    {
        // Movimento físico no FixedUpdate para suavidade e consistência com a física
        ApplyMovement();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            if (invertControls) MoveRight(); else MoveLeft();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            if (invertControls) MoveLeft(); else MoveRight();
        }
    }

    // Move para a faixa da esquerda (se não estiver na faixa 0)
    private void MoveLeft() { if (currentLane > 0) currentLane--; }

    // Move para a faixa da direita (se não estiver na faixa 2)
    private void MoveRight() { if (currentLane < 2) currentLane++; }

    private void CalculateLanePosition()
    {
        // Calcula o Z alvo com base na faixa atual
        // currentLane - 1 mapeia: 0 → -1, 1 → 0, 2 → +1
        float targetZ = initialZ + ((currentLane - 1) * laneDistance);

        // Mantém X e Y atuais para não interferir no pulo ou avanço
        targetPosition = new Vector3(transform.position.x, transform.position.y, targetZ);
    }

    private void ApplyMovement()
    {
        // Interpola suavemente o Z atual até o Z alvo
        float newZ = Mathf.MoveTowards(transform.position.z, targetPosition.z, laneSpeed * Time.fixedDeltaTime);

        // MovePosition respeita a física (colisões) enquanto move o Rigidbody
        rb.MovePosition(new Vector3(transform.position.x, rb.position.y, newZ));
    }
}
