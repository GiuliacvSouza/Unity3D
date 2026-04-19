using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Configurações de Faixa")]
    [SerializeField] private float laneDistance = 3.0f;
    [SerializeField] private float laneSpeed = 10.0f;
    [SerializeField] private bool invertControls = true;

    private Rigidbody rb;
    private int currentLane = 1; // 0 = esquerda, 1 = centro, 2 = direita
    private float initialZ;
    private Vector3 targetPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        initialZ = transform.position.z;
        targetPosition = transform.position;
    }

    void Update()
    {
        if (Time.timeScale == 0) return;

        HandleInput();
        CalculateLanePosition();
    }

    void FixedUpdate()
    {
        ApplyMovement();
    }

    private void HandleInput()
    {
        // ESQUERDA (A ou ←)
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (invertControls)
            {
                MoveRight();
            }
            else
            {
                MoveLeft();
            }
        }

        // DIREITA (D ou →)
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (invertControls)
            {
                MoveLeft();
            }
            else
            {
                MoveRight();
            }
        }
    }

    private void MoveLeft()
    {
        if (currentLane > 0)
            currentLane--;
    }

    private void MoveRight()
    {
        if (currentLane < 2)
            currentLane++;
    }

    private void CalculateLanePosition()
    {
        float targetZ = initialZ + ((currentLane - 1) * laneDistance);

        targetPosition = new Vector3(
            transform.position.x,
            transform.position.y,
            targetZ
        );
    }

    private void ApplyMovement()
    {
        float newZ = Mathf.MoveTowards(
            transform.position.z,
            targetPosition.z,
            laneSpeed * Time.fixedDeltaTime
        );

        rb.MovePosition(new Vector3(
            transform.position.x,
            rb.position.y,
            newZ
        ));
    }
}