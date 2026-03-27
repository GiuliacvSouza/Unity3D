using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Configurações")]
    public GameObject obstaclePrefab;
    public float spawnInterval = 2.0f;
    public float laneDistance = 3.0f;
    public float spawnXDistance = 40.0f;
    public float obstacleSpeed = 5.0f; // <--- NOVA VARIÁVEL APARECERÁ NO PAINEL

    [Header("Posicionamento")]
    public float spawnHeight = 1.5f; // Para você mudar o Y pelo painel também

    private float timer;
    private float initialZ;

    void Start()
    {
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
        int randomLane = Random.Range(-1, 2);
        float targetZ = initialZ + (randomLane * laneDistance);

        // Define a posição usando a variável de altura
        Vector3 spawnPos = new Vector3(transform.position.x + spawnXDistance, spawnHeight, targetZ);

        // Criamos o objeto e guardamos uma referência a ele na variável 'go'
        GameObject go = Instantiate(obstaclePrefab, spawnPos, Quaternion.identity);

        // AQUI ESTÁ O SEGREDO:
        // Pegamos o script 'Obstacle' que está dentro do cubo que acabou de nascer
        Obstacle scriptDoObstaculo = go.GetComponent<Obstacle>();

        if (scriptDoObstaculo != null)
        {
            // E passamos a velocidade do Spawner para o Obstáculo
            scriptDoObstaculo.speed = obstacleSpeed;
        }
    }
}