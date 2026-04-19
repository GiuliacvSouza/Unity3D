using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ObstacleEntry
{
    public GameObject prefab;
    [Range(0f, 1f)]
    public float spawnWeight = 1f;
    public bool occupiesAllLanes = false; // Spawna no centro, ocupa tudo por tamanho
}

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Configurações")]
    public List<ObstacleEntry> obstacleTypes = new List<ObstacleEntry>();
    public float spawnInterval = 2.0f;
    public float laneDistance = 3.0f;
    public float spawnXDistance = 40.0f;
    public float obstacleSpeed = 5.0f;

    [Header("Altura e Colisão")]
    public float raycastHeight = 50f;
    public float raycastDistance = 100f;
    public LayerMask groundLayer;

    [Header("Referências")]
    public Transform player;

    private float timer;
    private float initialZ;

    void Start()
    {
        initialZ = transform.position.z;
    }

    void Update()
    {
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

    ObstacleEntry PickRandomObstacleEntry()
    {
        float totalWeight = 0f;
        foreach (var entry in obstacleTypes)
            totalWeight += entry.spawnWeight;

        float roll = Random.Range(0f, totalWeight);
        float cumulative = 0f;

        foreach (var entry in obstacleTypes)
        {
            cumulative += entry.spawnWeight;
            if (roll <= cumulative)
                return entry;
        }

        return obstacleTypes[obstacleTypes.Count - 1];
    }

    void SpawnObstacle()
    {
        if (obstacleTypes == null || obstacleTypes.Count == 0)
        {
            Debug.LogWarning("Nenhum obstáculo configurado!");
            return;
        }

        ObstacleEntry chosen = PickRandomObstacleEntry();

        float targetZ = chosen.occupiesAllLanes
            ? initialZ
            : GetLaneZ();

        TrySpawnAt(chosen.prefab, targetZ);
    }

    float GetLaneZ()
    {
        int playerLane = Mathf.RoundToInt((player.position.z - initialZ) / laneDistance);
        playerLane = Mathf.Clamp(playerLane, -1, 1);

        int chosenLane;
        if (Random.value < 0.7f)
        {
            chosenLane = playerLane;
        }
        else
        {
            int[] lanes = new int[] { -1, 0, 1 };
            do
            {
                chosenLane = lanes[Random.Range(0, lanes.Length)];
            }
            while (chosenLane == playerLane);
        }

        return initialZ + (chosenLane * laneDistance);
    }

    void TrySpawnAt(GameObject prefab, float targetZ)
    {
        Vector3 rayOrigin = new Vector3(
            transform.position.x + spawnXDistance,
            raycastHeight,
            targetZ
        );

        RaycastHit hit;
        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, raycastDistance, groundLayer))
        {
            // Instancia uma única vez
            GameObject go = Instantiate(prefab);

            float yOffset = 0f;
            Collider col = go.GetComponent<Collider>();

            if (col != null)
            {
                // distância REAL do pivot até a base do collider
                yOffset = go.transform.position.y - col.bounds.min.y;
            }

            Vector3 spawnPos = new Vector3(
                rayOrigin.x,
                hit.point.y + yOffset,
                targetZ
            );

            go.transform.position = spawnPos;

            // Mantém rotação original + adapta ao terreno
            Quaternion groundRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
            go.transform.rotation = groundRotation * prefab.transform.rotation;

            // Aplica velocidade
            Obstacle obstacleScript = go.GetComponent<Obstacle>();
            if (obstacleScript != null)
            {
                obstacleScript.speed = obstacleSpeed;
            }
        }
        else
        {
            Debug.LogWarning($"Raycast não encontrou chão na faixa Z={targetZ}!");
        }
    }
}