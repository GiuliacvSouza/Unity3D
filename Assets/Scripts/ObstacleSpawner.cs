using UnityEngine;

// Spawna obstáculos em intervalos regulares com tendência à faixa do jogador
public class ObstacleSpawner : MonoBehaviour
{
    [Header("Configurações")]
    public GameObject obstaclePrefab;       // Prefab do obstáculo a ser spawnado
    public float spawnInterval = 2.0f;      // Tempo em segundos entre cada spawn
    public float laneDistance = 3.0f;       // Distância entre as faixas no eixo Z
    public float spawnXDistance = 40.0f;    // Distância à frente do spawner onde o obstáculo aparece
    public float obstacleSpeed = 5.0f;      // Velocidade passada para cada obstáculo spawnado

    [Header("Posicionamento")]
    public float spawnHeight = 1.5f;        // Altura (eixo Y) em que o obstáculo aparece

    [Header("Referências")]
    public Transform player;                // Referência ao jogador

    private float timer;    // Acumulador de tempo
    private float initialZ; // Posição Z inicial do spawner (centro das faixas)

    void Start()
    {
        initialZ = transform.position.z;
    }

    void Update()
    {
        // Só spawna enquanto o jogo estiver rodando
        if (Time.timeScale > 0)
        {
            timer += Time.deltaTime;
            if (timer >= spawnInterval)
            {
                SpawnObstacle();
                timer = 0;
            }
        }
    }

    void SpawnObstacle()
    {
        // Descobre em qual faixa o jogador está
        int playerLane = Mathf.RoundToInt((player.position.z - initialZ) / laneDistance);
        playerLane = Mathf.Clamp(playerLane, -1, 1);

        int chosenLane;

        // 70% de chance de spawnar na mesma faixa do jogador
        if (Random.value < 0.7f)
        {
            chosenLane = playerLane;
        }
        else
        {
            // 30% distribui entre as outras faixas
            int[] lanes = new int[] { -1, 0, 1 };

            do
            {
                chosenLane = lanes[Random.Range(0, lanes.Length)];
            }
            while (chosenLane == playerLane);
        }

        float targetZ = initialZ + (chosenLane * laneDistance);

        Vector3 spawnPos = new Vector3(
            transform.position.x + spawnXDistance,
            spawnHeight,
            targetZ
        );

        // Instancia o obstáculo e passa a velocidade do spawner para ele
        GameObject go = Instantiate(obstaclePrefab, spawnPos, Quaternion.identity);
        Obstacle scriptDoObstaculo = go.GetComponent<Obstacle>();

        if (scriptDoObstaculo != null)
        {
            scriptDoObstaculo.speed = obstacleSpeed;
        }
    }
}