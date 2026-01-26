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

    [Header("Pool Settings")]
    public int poolSize = 20;
    private Queue<GameObject> ghostPool = new Queue<GameObject>();
    
    internal List<GameObject> aliveGhosts = new List<GameObject>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            InitializePool();
        }
        else
            Destroy(gameObject);
    }
    void Start()
    {
        SpawnMultiple(startGhostCount);
    }
    void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject ghost = Instantiate(ghostPrefab);
            ghost.SetActive(false); // Hide them initially
            ghostPool.Enqueue(ghost);
        }
    }
    public void SpawnMultiple(int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (ghostPool.Count > 0)
            {
                GameObject ghost = ghostPool.Dequeue();

                // Set Position
                ghost.transform.position = GetRandomPositionAround(Camera.main.transform);
                ghost.SetActive(true);

                aliveGhosts.Add(ghost);
            }
        }
    }

    public void ReturnGhostToPool(GameObject ghost)
    {
        ghost.SetActive(false);
        aliveGhosts.Remove(ghost);
        ghostPool.Enqueue(ghost);

        // Auto-respawn logic
        if (aliveGhosts.Count <= 2)
        {
            SpawnMultiple(8);
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
