using UnityEngine;
using System.Collections.Generic;

public class GhostSpawner : MonoBehaviour
{
    public static GhostSpawner instance;

    public GameObject ghostPrefab;

    public int startGhostCount = 10;
    public int respawnGhostCount = 8;
    public int respawnThreshold = 2;

    public float spawnRadius = 5f;   
    public float minHeight = 0.5f;
    public float maxHeight = 1.5f;

    internal List<GameObject> aliveGhosts = new List<GameObject>();

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    void Start()
    {
        SpawnMultiple(startGhostCount);
    }
    // intrnel metho and call ,distroy call call spawn
    internal void OnGhostDestroyed(GameObject ghost)
    {
        aliveGhosts.Remove(ghost);

        if (aliveGhosts.Count <= respawnThreshold)
        {
            SpawnMultiple(respawnGhostCount);
        }
    }
    void SpawnMultiple(int count)
    {
        Camera cam = Camera.main;

        for (int i = 0; i < count; i++)
        {
            Vector3 spawnPos = GetRandomPositionAround(cam.transform);
            GameObject ghost = Instantiate(ghostPrefab, spawnPos, Quaternion.identity);
            aliveGhosts.Add(ghost);
        }
    }

    Vector3 GetRandomPositionAround(Transform player)
    {
        Vector2 circle = Random.insideUnitCircle.normalized * spawnRadius;

        Vector3 pos = player.position +
                      player.right * circle.x +
                      player.forward * circle.y;

        pos.y += Random.Range(minHeight, maxHeight);

        return pos;
    }
}
