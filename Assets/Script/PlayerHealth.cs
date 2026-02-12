using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance;

    internal int maxHealth = 10;
    internal int currentHealth;

    private bool isInvulnerable = false;
    private float invulnerabilityDuration = 1.5f;
    private void Awake()
    {
        // Set the instance as early as possible
        Instance = this;
    }
    void Start()
    {
        currentHealth = maxHealth;
        UIManager.Instance.UpdateHealthUI();
        UIManager.Instance.gameOverPanel.SetActive(false);

        // overlay in begining Alpha = 0
        UIManager.Instance.StartCoroutine(UIManager.Instance.FlashOverlay(Color.red, 0));
    }
    //void Update()
    //{
    //    // Press 'T' to test damage manually
    //    if (Input.GetKeyDown(KeyCode.T))
    //    {
    //        TakeDamage(1);
    //    }
    //}
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
        Debug.Log("Hit detected! Current Invulnerable status: " + isInvulnerable);
        if (isInvulnerable) return;

        currentHealth -= damage;
        UIManager.Instance.UpdateHealthUI();
        StartCoroutine(BecomeInvulnerable());

        // Visual feedback
        //StopAllCoroutines(); // Stop healing flash if taking damage
        //UIManager.Instance.StartCoroutine(damageOverlay.FlashOverlay(Color.red));
        UIManager.Instance.StartCoroutine(UIManager.Instance.FlashOverlay(Color.red));

        // Haptic feedback for mobile
        if (PlayerPrefs.GetInt("VibSetting", 1) == 1)
        {
            Handheld.Vibrate();
        }
        AudioManager.Instance.PlaySound(SoundType.Hit);

        if (currentHealth <= 0)
        {
            GameOver();
        }
    }
    public IEnumerator BecomeInvulnerable()
    {
        isInvulnerable = true;
        // Optional: make your gun or HUD blink to show you are safe
        yield return new WaitForSeconds(invulnerabilityDuration);
        isInvulnerable = false;
        Debug.Log("Player is now vulnerable again!");
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
        UIManager.Instance.gameOverPanel.SetActive(true);
        Time.timeScale = 0; // Pause 
        AudioManager.Instance.PlaySound(SoundType.GameOver);
    }

    public void RestartGame()
    {
        Time.timeScale = 1; // Play
        AudioManager.Instance.StopAllSounds();

        //SceneManager.LoadScene(1);
        SceneManager.LoadSceneAsync(1);
    }
    public void GoMenu()
    {
        AudioManager.Instance.StopAllSounds();

        //SceneManager.LoadScene(0);
        SceneManager.LoadSceneAsync(0);
    }
    public void ResetHealth()
    {
        currentHealth = maxHealth;
        UIManager.Instance.UpdateHealthUI();

        // Stop red and start a green one
        StopAllCoroutines();
        UIManager.Instance.StartCoroutine(UIManager.Instance.FlashOverlay(Color.green));

        Debug.Log("Player Health has been refilled to: " + currentHealth);
    }
    public void ActivateShield(float duration)
    {
        StopCoroutine(nameof(ShieldRoutine)); // Stop any existing shield timer
        StartCoroutine(ShieldRoutine(duration));
    }

    private IEnumerator ShieldRoutine(float duration)
    {
        isInvulnerable = true;
        Debug.Log("Shield ON!");

        UIManager.Instance.StartCoroutine(UIManager.Instance.FlashOverlay(Color.blue, 0.3f));

        yield return new WaitForSeconds(duration);

        isInvulnerable = false;
        Debug.Log("Shield OFF!");
    }
}

//public IEnumerator FlashDamageOverlay()
//{
//    // Quickly show red screen, then fade out
//    float intensity = 0.4f;
//    damageOverlay.color = new Color(1, 0, 0, 0.4f);

//    while (intensity > 0)
//    {
//        intensity -= Time.deltaTime;
//        damageOverlay.color = new Color(1, 0, 0, intensity);
//        yield return null;
//    }
//}
//public IEnumerator FlashHealingOverlay()
//{
//    if (damageOverlay == null) yield break;

//    float intensity = 0.4f;
//    damageOverlay.color = new Color(0, 1, 0, intensity); // Green color
//    AudioManager.Instance.PlaySound(SoundType.Claim);
//    while (intensity > 0)
//    {
//        intensity -= Time.deltaTime;
//        damageOverlay.color = new Color(0, 1, 0, intensity);
//        yield return null;
//    }
//}
/*
 ****Time.timeScale
 Time.timeScale controls how fast time runs in your game.
1	 Normal playback
0.5	 Slow motion
2	 Fast forward
0	 Pause

****Viberation 
Android only have 
    longer vibration
    custom patterns
  You must use Android Java vibration API

You can fake vibration strength by calling it multiple times.
    IEnumerator VibratePattern(int times, float delay)
    {
        for (int i = 0; i < times; i++)
        {
            Handheld.Vibrate();
            yield return new WaitForSeconds(delay);
        }
    }
    Usage:
    // Light hit
    StartCoroutine(VibratePattern(1, 0.1f));
    // Medium hit
    StartCoroutine(VibratePattern(3, 0.1f));
    // Strong hit (boss hit)
    StartCoroutine(VibratePattern(6, 0.05f));
*/