using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Reusable Popup UI")]
    [SerializeField] private GameObject popupPanel;
    [SerializeField] private Image popupIcon;
    [SerializeField] private TextMeshProUGUI popupTitle;
    [SerializeField] private TextMeshProUGUI popupBody;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("Health UI")]
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Image healthBarFill;
    [SerializeField] private Image damageOverlay;
    [SerializeField] internal GameObject gameOverPanel;

    [Header("Pause Menu")]
    [SerializeField] private GameObject pausePanel;

    private float flashFadeSpeed = 1.5f;

    private void Awake()
    {
        Instance = this;
    }

    #region Gift Popup
    public void OpenUniversalPopup(Sprite icon, string title, string message)
    {
        popupIcon.sprite = icon;
        popupTitle.text = title;
        popupBody.text = message;

        popupPanel.SetActive(true);
        Time.timeScale = 0;

        AudioManager.Instance.PlaySound(SoundType.Claim);
    }
    public void ClosePopup()
    {
        Time.timeScale = 1;
        popupPanel.SetActive(false);
    }
    #endregion
    #region Game UI
    public void UpdateGameUI()
    {
            ammoText.text = "Ammo: " + ShootManager.instance.currentAmmo + " / " + ShootManager.instance.maxAmmo;

            scoreText.text = "Killed: " + ShootManager.instance.killedCount;
    }
    #endregion
    # region UpdateHealthUI
    public void UpdateHealthUI()
    {
        healthText.text = "HP: " + PlayerHealth.Instance.currentHealth + " / " + PlayerHealth.Instance.maxHealth;

        healthBarFill.fillAmount = (float)PlayerHealth.Instance.currentHealth / PlayerHealth.Instance.maxHealth;
        float hpPercent = (float)PlayerHealth.Instance.currentHealth / PlayerHealth.Instance.maxHealth;
        if (hpPercent < 0.3f)
            healthBarFill.color = Color.red;
        else
            healthBarFill.color = Color.green;
       
    }
    #endregion
    #region FlashOverlay
    public IEnumerator FlashOverlay(Color flashColor, float startIntensity = 0.4f)
    {
        damageOverlay.gameObject.SetActive(true);
        float intensity = startIntensity;

        // Set initial color with intensity
        damageOverlay.color = new Color(flashColor.r,flashColor.g, flashColor.b,intensity);

        while (intensity > 0)
        {
            intensity -= Time.unscaledDeltaTime * flashFadeSpeed;

            damageOverlay.color = new Color(flashColor.r,flashColor.g,flashColor.b,Mathf.Clamp01(intensity));

            yield return null;
        }
        damageOverlay.color = new Color(flashColor.r, flashColor.g, flashColor.b, 0);
    }
    #endregion
    #region PauseAndPlay
    public void PressPause()
    {
        Time.timeScale = 0;

        pausePanel.SetActive(true); 
    }

    public void PressPlay()
    {
        Time.timeScale = 1;

        pausePanel.SetActive(false);
    }
    #endregion
}
