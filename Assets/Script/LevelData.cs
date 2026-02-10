using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewLevelData", menuName = "ScriptableObjects/LevelData")]
public class LevelData : ScriptableObject
{
    public List<LevelRequirement> levels;
}

[System.Serializable]
public class LevelRequirement
{
    public int levelNumber;
    public int requiredGifts;
    public int requiredKills;
    public string levelName;
    public float ghostSpeedMultiplier;
}