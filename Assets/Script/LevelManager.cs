using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelRequirement
{
    public int levelNumber;
    public int requiredGifts;
    public int requiredKills;
    public string levelName;
    public float ghostSpeedMultiplier;
}

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("Current Progress")]
    public int currentLevel = 1;
    public int totalKills = 0;
    public int totalGifts = 0;

    [Header("Level Settings")]
    public List<LevelRequirement> levels;

    void Awake() => Instance = this;

    // Call this from ShootManager when a ghost dies
    public void AddKill()
    {
        totalKills++;
        CheckLevelUp();
    }

    // Call this from StoryManager when a gift is touched
    public void AddGift()
    {
        totalGifts++;
        CheckLevelUp();
    }

    private void CheckLevelUp()
    {
        // Check if there is a next level defined
        if (currentLevel < levels.Count)
        {
            LevelRequirement nextLevel = levels[currentLevel]; // levels[1] is actually Level 2

            if (totalGifts >= nextLevel.requiredGifts && totalKills >= nextLevel.requiredKills)
            {
                LevelUp(nextLevel);
            }
        }
    }

    private void LevelUp(LevelRequirement newLevel)
    {
        currentLevel = newLevel.levelNumber;

        // Use your Universal Popup to tell the player!
        UIManager.Instance.OpenUniversalPopup(
            StoryManager.Instance.mysteryIcon,
            "LEVEL UP: " + newLevel.levelName,
            $"You have reached Level {currentLevel}! The ghosts are getting faster..."
        );

        // Make ghosts harder
        if (GhostSpawner.instance != null)
        {
        
        }
    }
}
