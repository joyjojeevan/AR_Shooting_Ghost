using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public static class SaveKeys
{
    public const string HIGH_SCORE = "HighScore";
    public const string PLAYER_NAME = "PlayerName";
    public const string SOUND_SET = "SoundSetting";
    public const string VIBRAT_SET = "VibSetting";
}

public class MainMenuManager : MonoBehaviour
{
    [Header("UI Text")]
    public TextMeshProUGUI menuHighScoreText;
    public TMP_InputField nameInputField;
    public TextMeshProUGUI profileName;

    [Header("Settings UI")]
    public GameObject instructionPanel;
    public Toggle soundToggle;
    public Toggle vibrationToggle;
    void Start()
    {
        //high score
        int highScore = PlayerPrefs.GetInt(SaveKeys.HIGH_SCORE, 0);
        menuHighScoreText.text = "BEST RECORD: " + highScore;
        //Player name
        string savedName = PlayerPrefs.GetString(SaveKeys.PLAYER_NAME, "Agent");

        nameInputField.text = PlayerPrefs.GetString(SaveKeys.PLAYER_NAME, "Agent");
        profileName.text =savedName.ToUpper();

        //Toggle
        bool soundOn = PlayerPrefs.GetInt(SaveKeys.SOUND_SET, 1) == 1;
        bool vibOn = PlayerPrefs.GetInt(SaveKeys.VIBRAT_SET, 1) == 1;

        soundToggle.isOn = soundOn;
        vibrationToggle.isOn = vibOn;

        instructionPanel.SetActive(false);

    }
    #region Navigation
    public void ToggleInstructions(bool show)
    {
        instructionPanel.SetActive(show);
    }
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
        // TODO : search lodescene Ascyn 
    }

    public void QuitGame()
    {
        Debug.Log("Game Exited");
        Application.Quit();
    }
    #endregion
    #region Settings Logic
    public void UpdateProfileNameUI(string newName)
    {
        profileName.text = newName.ToUpper();

    }
    public void SaveName(string inputName)
    {
        if (string.IsNullOrEmpty(inputName)) inputName = "Agent";
        PlayerPrefs.SetString(SaveKeys.PLAYER_NAME, inputName);
        //PlayerPrefs.Save();

        UpdateProfileNameUI(inputName);
    }

    public void ToggleSound(bool isOn)
    {
        PlayerPrefs.SetInt(SaveKeys.SOUND_SET, isOn ? 1 : 0);

        AudioListener.pause = !isOn;
        AudioListener.volume = isOn ? 1f : 0f;
    }

    public void ToggleVibration(bool isOn)
    {
        PlayerPrefs.SetInt(SaveKeys.VIBRAT_SET, isOn ? 1 : 0);
        //PlayerPrefs.Save();
    }
    #endregion
}
/*
 Playe
     Forces Unity to write all PlayerPrefs data to permanent storage immediately
 */