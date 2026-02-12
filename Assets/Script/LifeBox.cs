using UnityEngine;
using System.Collections;

public class LifeBox : MonoBehaviour, ICollectable
{
    public void Collect()
    {
        // Unique Logic: Fix health
        PlayerHealth.Instance.ResetHealth();

        AudioManager.Instance.PlaySound(SoundType.Claim);
    }
    //void Update()
    //{
    //    //transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    //    UIManager.Instance.SpinRightToLeft();
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("MainCamera") || other.GetComponent<PlayerHealth>() != null)
    //    {
    //        PlayerHealth health = other.GetComponent<PlayerHealth>();

    //        StartCoroutine(FlyToPlayer());

    //        if (health != null)
    //        {
    //            health.ResetHealth();
    //            gameObject.SetActive(false);
    //            LifeBoxManager.Instance.CollectBox();
    //        }
    //    }
    //}
    //private IEnumerator FlyToPlayer()
    //{
    //    float duration = 0.3f; // Fast and snappy
    //    float elapsed = 0;
    //    Vector3 startPos = transform.position;

    //    while (elapsed < duration)
    //    {
    //        // Move towards the camera every frame
    //        transform.position = Vector3.Lerp(startPos, Camera.main.transform.position, elapsed / duration);
    //        transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, elapsed / duration);
    //        elapsed += Time.deltaTime;
    //        yield return null;
    //    }

    //    gameObject.SetActive(false);
    //}
}
/*
 Is a MonoBehaviour
 AND promises to implement ICollectable
*/