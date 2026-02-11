using UnityEngine;


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
        // late few second
        Invoke("SpawnNewBox", 40f);
    }


    public void CollectBox()
    {
        //use invoke
        Invoke("SpawnNewBox", respawnDelay);
        //StartCoroutine(RespawnTimer());
    }

    //IEnumerator RespawnTimer()
    //{
    //    // The box is already set to inactive by the LifeBox script
    //    yield return new WaitForSeconds(respawnDelay);
    //    SpawnNewBox();
    //}

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
/*0° → right

90° → forward

180° → left

270° → back

But Sin/Cos use radians, not degrees
👉 that’s why we use Mathf.Deg2Rad
for the perfect circle area , feel better */