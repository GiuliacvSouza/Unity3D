using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float speed; // Recebe valor do Spawner
    private Transform player;

    void Start()
    {
        // Encontra o Player na cena
        player = Object.FindFirstObjectByType<PlayerController>().transform;
    }

    void Update()
    {
        // 1. Movimentação (indo para a esquerda)
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        // 2. Destruição por distância
        // Se o player passou pelo cubo e o cubo ficou 15 metros para trás:
        if (player != null && transform.position.x < player.position.x - 15f)
        {
            Destroy(gameObject);
        }
    }
}