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
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            // Aciona o game over pelo GameManager, que delega ao ScoreManager
            if (gameManager != null)
            {
                gameManager.AcionarGameOver();
            }

            // Desativa o PlayerController para o player parar de se mover após a morte
            PlayerController controller = GetComponent<PlayerController>();
            if (controller != null)
            {
                controller.enabled = false;
            }
        }
    }
}
