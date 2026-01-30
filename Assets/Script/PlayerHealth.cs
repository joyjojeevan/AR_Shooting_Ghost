using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 10;
    private int currentHealth;

    [Header("UI")]
    public TextMeshProUGUI healthText;
    public Image healthBarFill;
    public Image damageOverlay;
    public GameObject gameOverPanel;

    private bool isInvulnerable = false;
    public float invulnerabilityDuration = 1.5f;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
        gameOverPanel.SetActive(false);

        // overlay in begining Alpha = 0
        if (damageOverlay != null) damageOverlay.color = new Color(1, 0, 0, 0);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ghost"))
        {
            TakeDamage(1);

            // Want disappar ghost
            GhostSpawner.instance.ReturnGhostToPool(other.gameObject);
        }
    }
    public void TakeDamage(int damage)
    {
        if (isInvulnerable) return;

        currentHealth -= damage;
        UpdateHealthUI();
        StartCoroutine(BecomeInvulnerable());

        // Visual feedback
        StopCoroutine(FlashDamageOverlay()); // Stop healing flash if taking damage
        StartCoroutine(FlashDamageOverlay());

        // Haptic feedback for mobile
        /* vibrate featchres low /hard*/
        Handheld.Vibrate();
        AudioManager.Instance.PlaySound(SoundType.Hit);

        if (currentHealth <= 0)
        {
            GameOver();
        }
    }

    void UpdateHealthUI()
    {
        if (healthText != null)
            healthText.text = "HP: " + currentHealth + " / " + maxHealth;
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = (float)currentHealth / maxHealth;
            float hpPercent = (float)currentHealth / maxHealth;
            if (hpPercent < 0.3f)
                healthBarFill.color = Color.red;
            else
                healthBarFill.color = Color.green;
        }
    }

    System.Collections.IEnumerator FlashDamageOverlay()
    {
        // Quickly show red screen, then fade out
        float intensity = 0.4f;
        damageOverlay.color = new Color(1, 0, 0, 0.4f);

        while (intensity > 0)
        {
            intensity -= Time.deltaTime;
            damageOverlay.color = new Color(1, 0, 0, intensity);
            yield return null;
        }
    }
    System.Collections.IEnumerator FlashHealingOverlay()
    {
        if (damageOverlay == null) yield break;

        float intensity = 0.4f;
        damageOverlay.color = new Color(0, 1, 0, intensity); // Green color
        AudioManager.Instance.PlaySound(SoundType.Claim);
        while (intensity > 0)
        {
            intensity -= Time.deltaTime;
            damageOverlay.color = new Color(0, 1, 0, intensity);
            yield return null;
        }
    }
    System.Collections.IEnumerator BecomeInvulnerable()
    {
        isInvulnerable = true;
        // Optional: make your gun or HUD blink to show you are safe
        yield return new WaitForSeconds(invulnerabilityDuration);
        isInvulnerable = false;

    }

    void GameOver()
    {
        int finalScore = ShootManager.instance.GetKilledCount(); // Create this getter in ShootManager
        int highScore = PlayerPrefs.GetInt("HighScore", 0);

        if (finalScore > highScore)
        {
            PlayerPrefs.SetInt("HighScore", finalScore);
            PlayerPrefs.Save();
            // Maybe play a "New High Score" sound or show a special UI effect
        }
        Debug.Log("Game Over!");
        gameOverPanel.SetActive(true);
        Time.timeScale = 0; // Pause 
        AudioManager.Instance.PlaySound(SoundType.GameOver);
    }

    public void RestartGame()
    {
        Time.timeScale = 1; // Play
        if (AudioManager.Instance != null)
            AudioManager.Instance.StopAllSounds();

        SceneManager.LoadScene(1);
    }
    public void GoMenu()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.StopAllSounds();

        SceneManager.LoadScene(0);
    }
    public void ResetHealth()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();

        // Stop red and start a green one
        StopAllCoroutines();
        StartCoroutine(FlashHealingOverlay());

        Debug.Log("Player Health has been refilled to: " + currentHealth);
    }
}
/*Time.timeScale controls how fast time runs in your game.
1	 Normal playback
0.5	 Slow motion
2	 Fast forward
0	 Pause
*/