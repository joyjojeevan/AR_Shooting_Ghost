using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoryManager : MonoBehaviour
{
    public static StoryManager Instance;

    [SerializeField] private GameObject giftPrefab;
    [SerializeField] private GameObject investigationBox;
    [SerializeField] private TextMeshProUGUI instructionText;

    [Header("Progress")]
    private int giftsFound = 0;

    [Header("Icons")]
    [SerializeField] private Sprite boxIcon;
    [SerializeField] private Sprite giftIcon;           
    [SerializeField] private Sprite mysteryIcon;        
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
    public void OnBoxTouched()
    {
        Debug.Log("Box Touched!");

        UIManager.Instance.OpenUniversalPopup(boxIcon, "INVESTIGATION BOX", "Scanning complete. Try to find Key for Open this . Find the first Gift.");
        investigationBox.SetActive(false);
        instructionText.text = "Box Secure. Spawning the Gift...";

        SpawnGift();
    }

    public void SpawnGift()
    {
        Vector2 randomPoint = Random.insideUnitCircle.normalized;

        float randomDistance = Random.Range(4f, 6f);
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

        UIManager.Instance.OpenUniversalPopup(giftIcon, "GIFT", "You have stabilized a reality anchor. Warning: Entity activity increasing.");
        instructionText.text = "Echo secured. The anomaly is spreading! SURVIVE.";

        //UIManager.Instance.StartContinuousPopup(giftIcon);
        GhostSpawner.instance.gameStarted = true;
        GhostSpawner.instance.SpawnMultiple(3 + giftsFound);

        Debug.Log("Ghosts Spawned! Game officially started.");
        LevelManager.Instance.AddGift();

        Invoke(DataManager.SPAWN_GIFT, 50.0f);
    }
}