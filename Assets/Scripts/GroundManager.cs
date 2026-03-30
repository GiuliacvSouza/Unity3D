using System.Collections.Generic;
using UnityEngine;

// Gerencia o loop infinito do chão, reciclando os segmentos para trás do player
public class GroundManager : MonoBehaviour
{
    public List<Transform> grounds; // Lista de segmentos de chão na cena
    public Transform player;        // Referência ao player para calcular distância
    public float speed = 5f;        // Velocidade de deslocamento do chão

    private float groundLength;     // Comprimento de um segmento de chão

    void Start()
    {
        // Calcula o comprimento baseado no primeiro segmento
        groundLength = GetGroundLength(grounds[0]);
    }

    void Update()
    {
        // Não move o chão se o jogo estiver pausado
        if (Time.timeScale == 0) return;

        // Move todos os segmentos para a esquerda
        foreach (Transform ground in grounds)
        {
            ground.Translate(Vector3.left * speed * Time.deltaTime);
        }

        // Verifica se o primeiro segmento já passou do player
        Transform firstGround = grounds[0];
        if (firstGround.position.x < player.position.x - groundLength)
        {
            RecycleGround();
        }
    }

    // Retorna o comprimento do segmento via Renderer, com fallback de 10 unidades
    float GetGroundLength(Transform ground)
    {
        Renderer rend = ground.GetComponent<Renderer>();
        if (rend != null)
        {
            return rend.bounds.size.x;
        }

        Debug.LogWarning("Ground sem Renderer!");
        return 10f;
    }

    // Move o primeiro segmento para depois do último, criando o efeito de loop infinito
    void RecycleGround()
    {
        Transform firstGround = grounds[0];
        Transform lastGround = grounds[grounds.Count - 1];

        float length = GetGroundLength(firstGround);

        // Reposiciona logo após o último segmento
        firstGround.position = new Vector3(
            lastGround.position.x + length,
            firstGround.position.y,
            firstGround.position.z
        );

        // Atualiza a lista: remove do início e adiciona no fim
        grounds.RemoveAt(0);
        grounds.Add(firstGround);
    }
}
