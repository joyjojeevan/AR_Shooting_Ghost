using UnityEngine;
using System.Collections;
public class MagazineObject : MonoBehaviour
{
    public static MagazineObject instance;
    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        //Spin
        transform.Rotate(50 * Time.deltaTime, 100 * Time.deltaTime, 0);
    }
    //Do raycast
    //private void OnMouseDown()
    //{
    //    ShootManager.instance.HandleReload(gameObject);
    //}
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera") || other.GetComponent<PlayerHealth>() != null)
        {
            CollectMe();
        }
    }
    private void CollectMe()
    {
        if (ShootManager.instance != null)
        {
            ShootManager.instance.HandleReload(gameObject);

            if (AudioManager.Instance != null)
                AudioManager.Instance.PlaySound(SoundType.Claim);

            StartCoroutine(FlyToPlayer());
        }
        gameObject.SetActive(false);
    }
    private IEnumerator FlyToPlayer()
    {
        float duration = 0.3f; // Fast and snappy
        float elapsed = 0;
        Vector3 startPos = transform.position;

        while (elapsed < duration)
        {
            // Move towards the camera every frame
            transform.position = Vector3.Lerp(startPos, Camera.main.transform.position, elapsed / duration);
            transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        gameObject.SetActive(false);
    }
}