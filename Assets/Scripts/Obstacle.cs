using UnityEngine;

// Controla o movimento e destruição de cada obstáculo individualmente
public class Obstacle : MonoBehaviour
{
    public float speed; // Velocidade recebida do ObstacleSpawner no momento do spawn

    private Transform player;

    void Start()
    {
        // Busca o player na cena para calcular distância
        player = Object.FindFirstObjectByType<PlayerController>().transform;
    }

    void Update()
    {
        // Move o obstáculo para a esquerda
        transform.Translate(Vector3.left * speed * Time.deltaTime, Space.World);

        // Destrói o obstáculo quando ficar 15 unidades atrás do player
        // evitando acúmulo de objetos invisíveis na cena
        if (player != null && transform.position.x < player.position.x - 15f)
        {
            Destroy(gameObject);
        }
    }
}
