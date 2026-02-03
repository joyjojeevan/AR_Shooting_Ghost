using TMPro;
using UnityEngine;

public class StoryManager : MonoBehaviour
{
    public static StoryManager Instance;

    public GameObject giftPrefab; // Drag your Gift Prefab/Object here
    public GameObject investigationBox;
    public TextMeshProUGUI instructionText;

    [Header("Progress")]
    public int giftsFound = 0;
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        investigationBox.SetActive(true);
        if (giftPrefab != null) giftPrefab.SetActive(false);
        instructionText.text = "Find the Investigation Box to begin.";
    }

    // This is called by your StartTrigger script
    public void OnBoxTouched()
    {
        Debug.Log("Step 1: Box Touched!");
        investigationBox.SetActive(false);
        instructionText.text = "Box Secure. Spawning the Gift...";

        // Trigger the gift spawn
        SpawnGift();
    }

    public void SpawnGift()
    {
        Vector3 spawnPos = Camera.main.transform.position + (Camera.main.transform.forward * 2.5f);
        spawnPos.y = Camera.main.transform.position.y;

        GameObject newGift = Instantiate(giftPrefab, spawnPos, Quaternion.identity);
        newGift.SetActive(true);

        newGift.transform.localScale = Vector3.one;

        Debug.Log("Gift Spawned and Forced to Active!");
    }
    public void OnGiftFound()
    {
        giftsFound++; 

        if (instructionText != null)
        {
            instructionText.text = "Echo secured. The anomaly is spreading! SURVIVE.";
        }

        if (GhostSpawner.instance != null)
        {
            GhostSpawner.instance.gameStarted = true;

            GhostSpawner.instance.SpawnMultiple(3 + giftsFound);

            Debug.Log("Ghosts Spawned! Game officially started.");
        }
        else
        {
            Debug.LogError("GhostSpawner instance not found!");
        }

         Invoke("SpawnGift", 50.0f);
    }
}