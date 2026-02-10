using UnityEngine;

public static class DataKeys
{
    public const string SAVED_LEVEL = "SavedLevel";
    public const string TOTAL_KILLS = "TotalKills";
    public const string TOTAL_GIFTS = "TotalGifts";
}
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("Data Source")]
    public LevelData levelDatabase;

    [Header("Current Progress")]
    public int currentLevel = 1;
    public int totalKills = 0;
    public int totalGifts = 0;



    void Awake()
    {
        Instance = this;
        LoadProgress();
    }
    //change to keys
    public void SaveProgress()
    {
        PlayerPrefs.SetInt(DataKeys.SAVED_LEVEL, currentLevel);
        PlayerPrefs.SetInt(DataKeys.TOTAL_KILLS, totalKills);
        PlayerPrefs.SetInt(DataKeys.TOTAL_GIFTS, totalGifts);
        PlayerPrefs.Save(); 
        Debug.Log("Progress Saved!");
    }

    public void LoadProgress()
    {
        currentLevel = PlayerPrefs.GetInt(DataKeys.SAVED_LEVEL, 1);
        totalKills = PlayerPrefs.GetInt(DataKeys.TOTAL_KILLS, 0);
        totalGifts = PlayerPrefs.GetInt(DataKeys.TOTAL_GIFTS, 0);
        Debug.Log("Progress Loaded! Level: " + currentLevel);
    }
    private void CheckLevelUp()
    {
        // Check if there is a next level defined
        if (currentLevel < levelDatabase.levels.Count)
        {
            LevelRequirement nextLevel = levelDatabase.levels[currentLevel]; // levels[1] is actually Level 2

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

        // set popup properly
        //UIManager.Instance.OpenUniversalPopup(
        //    StoryManager.Instance.mysteryIcon,
        //    "LEVEL UP: " + newLevel.levelName,
        //    $"You have reached Level {currentLevel}! The ghosts are getting faster..."
        //);

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
}
