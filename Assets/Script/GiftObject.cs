using UnityEngine;

public class GiftObject : MonoBehaviour
{
    private float spawnTime;

    void Start()
    {
        spawnTime = Time.time;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (Time.time - spawnTime < 1.0f) return;
        // Ensure your AR Camera is tagged "MainCamera"
        if (other.CompareTag("MainCamera"))
        {
             StoryManager.Instance.OnGiftFound();

             if (AudioManager.Instance != null)
                 AudioManager.Instance.PlaySound(SoundType.Claim);

             // Hide the gift until the next one spawns
             gameObject.SetActive(false);
            
        }
    }
}