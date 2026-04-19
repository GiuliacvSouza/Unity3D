using UnityEngine;

// Detecta colisões do player com obstáculos e aciona o game over
public class PlayerCollision : MonoBehaviour
{
    private GameManager gameManager;

    void Start()
    {
        // Busca o GameManager na cena (padrão Unity 6)
        gameManager = Object.FindFirstObjectByType<GameManager>();
    }

    // Usa Trigger (não Collision) — o Collider do player deve ter "Is Trigger" ativado
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            if (gameManager != null)
            {
                gameManager.AcionarGameOver();
            }

            PlayerController controller = GetComponent<PlayerController>();
            if (controller != null)
            {
                controller.enabled = false;
            }
        }
    }
}
