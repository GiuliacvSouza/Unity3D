using UnityEngine;

// Spawna obstáculos em intervalos regulares em faixas aleatórias
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
        // Escolhe uma faixa aleatória: -1 (esquerda), 0 (centro), 1 (direita)
        int randomLane = Random.Range(-1, 2);
        float targetZ = initialZ + (randomLane * laneDistance);

        Vector3 spawnPos = new Vector3(transform.position.x + spawnXDistance, spawnHeight, targetZ);

        // Instancia o obstáculo e passa a velocidade do spawner para ele
        GameObject go = Instantiate(obstaclePrefab, spawnPos, Quaternion.identity);
        Obstacle scriptDoObstaculo = go.GetComponent<Obstacle>();

        if (scriptDoObstaculo != null)
        {
            scriptDoObstaculo.speed = obstacleSpeed;
        }
    }
}
