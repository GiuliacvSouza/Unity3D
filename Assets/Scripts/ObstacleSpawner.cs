using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Configurações")]
    public GameObject obstaclePrefab;
    public float spawnInterval = 2.0f; // Tempo entre cada obstáculo
    public float laneDistance = 3.0f; // Deve ser igual à do PlayerController
    public float spawnXDistance = 20.0f; // Quão longe na frente do player ele surge

    private float timer;
    private float initialZ;

    void Start()
    {
        // Captura o Z inicial (mesma lógica do player)
        initialZ = transform.position.z;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnObstacle();
            timer = 0;
        }
    }

    void SpawnObstacle()
    {
        // Escolhe uma das 3 faixas (-1, 0, 1) aleatoriamente
        int randomLane = Random.Range(-1, 2);
        float targetZ = initialZ + (randomLane * laneDistance);

        // Define a posição de spawn (X na frente, Z na faixa sorteada)
        Vector3 spawnPos = new Vector3(transform.position.x + spawnXDistance, 1.5f, targetZ);

        Instantiate(obstaclePrefab, spawnPos, Quaternion.identity);
    }
}