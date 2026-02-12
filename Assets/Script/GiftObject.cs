using UnityEngine;

public class GiftObject : MonoBehaviour, ICollectable
{
    public void Collect()
    {
        // Unique Logic: Progress Story/Level
        StoryManager.Instance.OnGiftFound();
        LevelManager.Instance.AddGift();
        AudioManager.Instance.PlaySound(SoundType.Claim);
    }
}