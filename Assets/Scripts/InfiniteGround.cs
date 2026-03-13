using System.Collections.Generic;
using UnityEngine;

public class InfiniteGround : MonoBehaviour
{
    public Transform player;
    public GameObject groundPrefab;

    public int tilesOnScreen = 5;
    public float tileLength = 10f;

    private float spawnZ = 0;
    private List<GameObject> activeTiles = new List<GameObject>();

    void Start()
    {
        for (int i = 0; i < tilesOnScreen; i++)
        {
            SpawnTile();
        }
    }

    void Update()
    {
        if (player.position.z - 20 > spawnZ - (tilesOnScreen * tileLength))
        {
            SpawnTile();
            DeleteTile();
        }
    }

    void SpawnTile()
    {
        GameObject tile = Instantiate(groundPrefab, Vector3.forward * spawnZ, Quaternion.identity);
        activeTiles.Add(tile);
        spawnZ += tileLength;
    }

    void DeleteTile()
    {
        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);
    }
}