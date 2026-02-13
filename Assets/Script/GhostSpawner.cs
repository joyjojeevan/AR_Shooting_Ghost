using UnityEngine;
using System.Collections.Generic;

public class GhostSpawner : MonoBehaviour
{
    public static GhostSpawner instance;

    public GameObject ghostPrefab;

    [Header("Story Control")]
    internal bool gameStarted = false;

    private int startGhostCount = 10;
    private int respawnGhostCount = 8;
    private int respawnThreshold = 2;

    private float minHeight = 0.5f;
    private float maxHeight = 1.5f;

    [Header("Pool Settings")]
    private int poolSize = 10;

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
    void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject ghost = Instantiate(ghostPrefab);
            ghost.SetActive(false); // Hide them initially
            ghostPool.Enqueue(ghost);
        }
    }
    public void StartSpawner()
    {
        gameStarted = true;
        SpawnMultiple(startGhostCount);
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
        Vector3 deathPosition = ghost.transform.position;

        ghost.SetActive(false);
        aliveGhosts.Remove(ghost);
        ghostPool.Enqueue(ghost);

        //Try to spawn a shield at the death position
        LevelManager.Instance.TrySpawnShield(deathPosition);
        // Auto-respawn logic
        if (gameStarted && aliveGhosts.Count <= respawnThreshold)
        {
            SpawnMultiple(respawnGhostCount);
        }
    }
    Vector3 GetRandomPositionAround(Transform player)
    {
        Vector2 circle = Random.insideUnitCircle.normalized * Random.Range(1.5f, 5f);

        Vector3 pos = player.position +
                      player.right * circle.x +
                      player.forward * circle.y;

        pos.y += Random.Range(minHeight, maxHeight);

        return pos;
    }

}
