using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Reusable Popup UI")]
    public GameObject popupPanel;
    public Image popupIcon;
    public TextMeshProUGUI popupTitle;
    public TextMeshProUGUI popupBody;

    [Header("UI References")]
    public TextMeshProUGUI ammoText;  
    public TextMeshProUGUI scoreText;

    [Header("Health UI")]
    public TextMeshProUGUI healthText;
    public Image healthBarFill;
    public Image damageOverlay;
    public GameObject gameOverPanel;

    public float rotationSpeed = 100f;

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
        if (ammoText != null)
            ammoText.text = "Ammo: " + ShootManager.instance.currentAmmo + " / " + ShootManager.instance.maxAmmo;

        if (scoreText != null)
            scoreText.text = "Killed: " + ShootManager.instance.killedCount;
    }
    #endregion
    #region Spin Right to Left
    public void SpinRightToLeft()
    {
        transform.Rotate(50 * Time.deltaTime, 100 * Time.deltaTime, 0);
    }
    #endregion
    # region UpdateHealthUI
    public void UpdateHealthUI()
    {
        if (healthText != null)
            healthText.text = "HP: " + PlayerHealth.Instance.currentHealth + " / " + PlayerHealth.Instance.maxHealth;
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = (float)PlayerHealth.Instance.currentHealth / PlayerHealth.Instance.maxHealth;
            float hpPercent = (float)PlayerHealth.Instance.currentHealth / PlayerHealth.Instance.maxHealth;
            if (hpPercent < 0.3f)
                healthBarFill.color = Color.red;
            else
                healthBarFill.color = Color.green;
        }
    }
    #endregion
    #region FlashOverlay
    public IEnumerator FlashOverlay(Color flashColor, float startIntensity = 0.4f)
    {
        float intensity = startIntensity;

        // Set initial color with intensity
        damageOverlay.color = new Color(flashColor.r,flashColor.g, flashColor.b,intensity);

        while (intensity > 0)
        {
            intensity -= Time.deltaTime;

            damageOverlay.color = new Color(flashColor.r,flashColor.g,flashColor.b,intensity);

            yield return null;
        }
    }
    #endregion
}
