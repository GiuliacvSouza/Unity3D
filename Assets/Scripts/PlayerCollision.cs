using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private GameManager gameManager; 

    void Start()
    {
        // Padrão Unity 6
        gameManager = Object.FindFirstObjectByType<GameManager>();
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Obstacle"))
        {
            if (gameManager != null)
            {
                gameManager.AcionarGameOver();
            }

            // Desativa o movimento para a bola não continuar andando no fundo
            if (GetComponent<PlayerController>() != null)
            {
                GetComponent<PlayerController>().enabled = false;
            }
        }
    }
}