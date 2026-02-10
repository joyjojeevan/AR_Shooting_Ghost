using UnityEngine;
public enum SoundType
{
    GameOver,
    Hit,
    Fire,
    Kill,
    Claim
}
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Sound Effects")]
    public AudioClip GameOverSound;
    public AudioClip HitSound;
    public AudioClip FireSound;
    public AudioClip KilledSound;
    public AudioClip ClaimSound;

    private AudioSource audioSource;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(SoundType type)
    {
        AudioClip clip = null;

        if (PlayerPrefs.GetInt("SoundSetting", 1) == 0)
        {
            return; 
        }

        switch (type)
        {
            case SoundType.GameOver:
                clip = GameOverSound;
                break;
            case SoundType.Hit:
                clip = HitSound;
                break;
            case SoundType.Fire:
                clip = FireSound;
                break;
            case SoundType.Kill:
                clip = KilledSound;
                break;
            case SoundType.Claim:
                clip = ClaimSound;
                break;
        }

        if (clip != null)
            audioSource.PlayOneShot(clip);
    }
    public void StopAllSounds()
    {
        audioSource.Stop();
    }
}

