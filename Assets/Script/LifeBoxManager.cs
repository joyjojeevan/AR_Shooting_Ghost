using UnityEngine;
using System.Collections;

public class LifeBoxManager : MonoBehaviour
{
    public static LifeBoxManager Instance;

    public GameObject lifeBoxPrefab; 
    public GameObject activeBox;    

    public float respawnDelay = 30f;
    public float spawnRadius = 3.5f;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // late 2 second
        Invoke("SpawnNewBox", 2.0f);
    }


    public void CollectBox()
    {
        //use invoke
        StartCoroutine(RespawnTimer());
    }

    IEnumerator RespawnTimer()
    {
        // The box is already set to inactive by the LifeBox script
        yield return new WaitForSeconds(respawnDelay);
        SpawnNewBox();
    }

    void SpawnNewBox()
    {
        //Calculate Random Position
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        float distance = Random.Range(1f, spawnRadius);

        // check math
        Vector3 spawnPos = new Vector3(
            Camera.main.transform.position.x + Mathf.Cos(angle) * distance,
            Camera.main.transform.position.y - 0.2f,
            Camera.main.transform.position.z + Mathf.Sin(angle) * distance
        );

        //If we don't have a box in the scene yet, CREATE ONE
        if (activeBox == null)
        {
            activeBox = Instantiate(lifeBoxPrefab, spawnPos, Quaternion.identity);
        }
        else
        {
            //If we already have one, just move it and turn it on
            activeBox.transform.position = spawnPos;
            activeBox.SetActive(true);
        }

        Debug.Log("Life Box Spawned at: " + spawnPos);
    }
}