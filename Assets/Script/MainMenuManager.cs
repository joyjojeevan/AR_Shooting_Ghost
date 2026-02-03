using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    public TextMeshProUGUI menuHighScoreText;
    void Start()
    {
        // Retrieve the saved score
        int highScore = PlayerPrefs.GetInt("HighScore", 0);

        // Update the UI text
        if (menuHighScoreText != null)
        {
            menuHighScoreText.text = "BEST RECORD: " + highScore;
        }
    }
    public void PlayGame()
    {
        // Loads the next scene in the build index (Your AR Game)
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Debug.Log("Game Exited");
        Application.Quit();
    }
}