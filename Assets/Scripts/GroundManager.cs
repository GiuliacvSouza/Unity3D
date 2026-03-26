using System.Collections.Generic;
using UnityEngine;

public class GroundManager : MonoBehaviour
{
    public List<Transform> grounds;
    public Transform player;
    public float speed = 5f;

    private float groundLength;

    void Start()
    {
        groundLength = GetGroundLength(grounds[0]);
    }

    void Update()
    {
        // move os chãos para a esquerda
        foreach (Transform ground in grounds)
        {
            ground.Translate(Vector3.left * speed * Time.deltaTime);
        }

        // verifica o chão mais atrás
        Transform firstGround = grounds[0];

        // se ele já ficou atrás do player
        if (firstGround.position.x < player.position.x - groundLength)
        {
            RecycleGround();
        }
    }

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

    void RecycleGround()
    {
        Transform firstGround = grounds[0];
        Transform lastGround = grounds[grounds.Count - 1];

        float length = GetGroundLength(firstGround);

        // coloca na frente do último
        firstGround.position = new Vector3(
            lastGround.position.x + length,
            firstGround.position.y,
            firstGround.position.z
        );

        grounds.RemoveAt(0);
        grounds.Add(firstGround);
    }
}