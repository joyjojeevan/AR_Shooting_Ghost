using UnityEngine;

public class ShieldObject : MonoBehaviour, ICollectable
{
    public float shieldDuration = 5f;

    public void Collect()
    {
        // Tell the player to be invulnerable
        PlayerHealth.Instance.ActivateShield(shieldDuration);

        AudioManager.Instance.PlaySound(SoundType.Claim);
    }
}