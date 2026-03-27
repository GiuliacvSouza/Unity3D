using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Configurações de Faixa")]
    [SerializeField] private float laneDistance = 3.0f;
    [SerializeField] private float laneSpeed = 10.0f;
    [SerializeField] private bool invertControls = true; // Facilitador para inverter rápido

    [Header("Configurações de Pulo")]
    [SerializeField] private float jumpForce = 7.0f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;

    private Rigidbody rb;
    private int currentLane = 1; // 0: Esquerda, 1: Centro, 2: Direita
    private float initialZ;      // Onde o player estava no Z quando o jogo deu Start
    private Vector3 targetPosition;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        initialZ = transform.position.z;

        // Forçamos o targetPosition a ser EXATAMENTE onde o player começa
        targetPosition = transform.position;
    }

    void Update()
    {
        HandleInput();
        CalculateLanePosition();
    }

    void FixedUpdate()
    {
        ApplyMovement();
        CheckGround();
    }

    private void HandleInput()
    {
        // Lógica de inversão baseada no seu feedback
        int direction = invertControls ? -1 : 1;

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            // Se invertido, A aumenta a lane, se normal, diminui.
            if (invertControls) MoveRight(); else MoveLeft();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            if (invertControls) MoveLeft(); else MoveRight();
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    private void MoveLeft() { if (currentLane > 0) currentLane--; }
    private void MoveRight() { if (currentLane < 2) currentLane++; }

    private void CalculateLanePosition()
    {
        // Agora somamos o offset (laneDistance) ao initialZ (seu -10)
        float targetZ = initialZ + ((currentLane - 1) * laneDistance);

        // Mantemos o X e Y atuais para ele não "teletransportar" no eixo do movimento
        targetPosition = new Vector3(transform.position.x, transform.position.y, targetZ);
    }

    private void ApplyMovement()
    {
        // MoveTowards é mais estável para movimentação física direta que o Lerp
        float newZ = Mathf.MoveTowards(transform.position.z, targetPosition.z, laneSpeed * Time.fixedDeltaTime);

        // Aplicamos a posição mantendo o X fixo e o Y controlado pela gravidade/pulo
        rb.MovePosition(new Vector3(transform.position.x, rb.position.y, newZ));
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void CheckGround()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
    }
}