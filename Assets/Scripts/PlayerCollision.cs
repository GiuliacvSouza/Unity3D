using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public ScoreManager scoreManager; 

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Obstacle"))
        {
            Debug.Log("TRIGGER ATIVADO! Batemos no obstáculo.");

            if (scoreManager != null)
            {
                scoreManager.PararCronometro();
            }

            // Para o movimento do player
            if (GetComponent<PlayerController>() != null)
            {
                GetComponent<PlayerController>().enabled = false;
            }
        }
    }
}