using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoryManager : MonoBehaviour
{
    public static StoryManager Instance;

    public GameObject giftPrefab; // Drag your Gift Prefab/Object here
    public GameObject investigationBox;
    public TextMeshProUGUI instructionText;

    [Header("Progress")]
    public int giftsFound = 0;

    [Header("Reusable Popup UI")]
    public GameObject popupPanel;
    public Image popupIcon;          
    public TextMeshProUGUI popupTitle;
    public TextMeshProUGUI popupBody;

    [Header("Icons")]
    public Sprite boxIcon;            
    public Sprite giftIcon;           
    public Sprite mysteryIcon;        
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        investigationBox.SetActive(true);
        giftPrefab.SetActive(false);
        instructionText.text = "Find the Investigation Box to begin.";
    }
    public void OpenUniversalPopup(Sprite icon, string title, string message)
    {
        popupIcon.sprite = icon;
        popupTitle.text = title;
        popupBody.text = message;

        popupPanel.SetActive(true);
        Time.timeScale = 0;

        AudioManager.Instance.PlaySound(SoundType.Claim);
    }

    public void ClosePopup()
    {
        Time.timeScale = 1;
        popupPanel.SetActive(false);
    }

    public void OnBoxTouched()
    {
        Debug.Log("Box Touched!");

        OpenUniversalPopup(boxIcon, "INVESTIGATION BOX", "Scanning complete. Try to find Key for Open this . Find the first Gift.");
        investigationBox.SetActive(false);
        instructionText.text = "Box Secure. Spawning the Gift...";

        SpawnGift();
    }

    public void SpawnGift()
    {
        Vector2 randomPoint = Random.insideUnitCircle.normalized;

        float randomDistance = Random.Range(2.0f, 5.0f);
        Vector3 spawnOffset = new Vector3(randomPoint.x, 0, randomPoint.y) * randomDistance;
        Vector3 spawnPos = Camera.main.transform.position + spawnOffset;
        spawnPos.y = Camera.main.transform.position.y - 0.2f;

        GameObject newGift = Instantiate(giftPrefab, spawnPos, Quaternion.identity);
        newGift.SetActive(true);

        newGift.transform.localScale = Vector3.one;

        Debug.Log("Gift Spawned and Forced to Active!");
    }
    public void OnGiftFound()
    {
        giftsFound++;
        OpenUniversalPopup(giftIcon, "GIFT", "You have stabilized a reality anchor. Warning: Entity activity increasing.");

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