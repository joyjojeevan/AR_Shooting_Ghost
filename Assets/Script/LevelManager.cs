using UnityEngine;

public static class DataManager
{
    public const string SAVED_LEVEL = "SavedLevel";
    public const string TOTAL_KILLS = "TotalKills";
    public const string TOTAL_GIFTS = "TotalGifts";
    //MainMenu
    public const string HIGH_SCORE = "HighScore";
    public const string PLAYER_NAME = "PlayerName";
    public const string SOUND_SET = "SoundSetting";
    public const string VIBRAT_SET = "VibSetting";
    ////StoryManager
    //public const string SPAWN_GIFT = "SpawnGift";


}
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("Data Source")]
    [SerializeField] private LevelData levelDatabase;

    [Header("Current Progress")]
    private int currentLevel = 1;
    private int totalKills = 0;
    private int totalGifts = 0;

    [Header("Shield Spawning")]
    [SerializeField] private GameObject shieldPrefab;
    private float shieldSpawnChance = 0.1f;

    void Awake()
    {
        Instance = this;
        LoadProgress();
    }
    public void SaveProgress()
    {
        PlayerPrefs.SetInt(DataManager.SAVED_LEVEL, currentLevel);
        PlayerPrefs.SetInt(DataManager.TOTAL_KILLS, totalKills);
        PlayerPrefs.SetInt(DataManager.TOTAL_GIFTS, totalGifts);
        PlayerPrefs.Save(); 
        Debug.Log("Progress Saved!");
    }

    public void LoadProgress()
    {
        currentLevel = PlayerPrefs.GetInt(DataManager.SAVED_LEVEL, 1);
        totalKills = PlayerPrefs.GetInt(DataManager.TOTAL_KILLS, 0);
        totalGifts = PlayerPrefs.GetInt(DataManager.TOTAL_GIFTS, 0);
        Debug.Log("Progress Loaded! Level: " + currentLevel);
    }
    private void CheckLevelUp()
    {
        // Check if there is a next level defined
        if (currentLevel < levelDatabase.levels.Count)
        {
            LevelRequirement nextLevel = levelDatabase.levels[currentLevel]; 

            if (totalGifts >= nextLevel.requiredGifts && totalKills >= nextLevel.requiredKills)
            {
                LevelUp(nextLevel);
            }
        }
    }

    private void LevelUp(LevelRequirement newLevel)
    {
        currentLevel = newLevel.levelNumber;
        SaveProgress();

        //TODO : Make ghosts harder
        if (GhostSpawner.instance != null)
        {
        
        }
    }
    public void AddKill()
    {
        totalKills++;
        CheckLevelUp();
    }
    public void AddGift()
    {
        totalGifts++;
        CheckLevelUp();
    }

    public void TrySpawnShield(Vector3 position)
    {
        if (currentLevel >= 3)
        {
            if (Random.value < shieldSpawnChance)
            {
                Instantiate(shieldPrefab, position + Vector3.up, Quaternion.identity);
            }
        }
    }
}
