using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("UI Text")]
    [SerializeField] private TextMeshProUGUI menuHighScoreText;
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private TextMeshProUGUI profileName;

    [Header("Settings UI")]
    [SerializeField] private GameObject instructionPanel;
    //[SerializeField] private Toggle soundToggle;
    //[SerializeField] private Toggle vibrationToggle;
    void Start()
    {
        //high score
        int highScore = PlayerPrefs.GetInt(DataManager.HIGH_SCORE, 0);
        menuHighScoreText.text = "BEST RECORD: " + highScore;
        //Player name
        string savedName = PlayerPrefs.GetString(DataManager.PLAYER_NAME, "Agent");

        nameInputField.text = PlayerPrefs.GetString(DataManager.PLAYER_NAME, "Agent");
        profileName.text =savedName.ToUpper();

        //Toggle
        bool soundOn = PlayerPrefs.GetInt(DataManager.SOUND_SET, 1) == 1;
        bool vibOn = PlayerPrefs.GetInt(DataManager.VIBRAT_SET, 1) == 1;

        UIManager.Instance.soundToggle.isOn = soundOn;
        UIManager.Instance.vibrationToggle.isOn = vibOn;

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
        PlayerPrefs.SetString(DataManager.PLAYER_NAME, inputName);
        //PlayerPrefs.Save();

        UpdateProfileNameUI(inputName);
    }
    #endregion
}
/*
 Playe
     Forces Unity to write all PlayerPrefs data to permanent storage immediately
 */