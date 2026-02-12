using UnityEngine;
using System.Collections;

public class CollectableItem : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 100f;
    private bool isCollected = false;

    void Update()
    {
        // All items will spin using this one piece of code
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isCollected) return;

        if (other.CompareTag("MainCamera") || other.GetComponent<PlayerHealth>() != null)
        {
            isCollected = true;

            // Look for the "Label" (Interface) on this object(Does this object have a script that implements ICollectable?)
            ICollectable itemLogic = GetComponent<ICollectable>();

            if (itemLogic != null)
            {
                itemLogic.Collect(); // Run the specific logic (Add HP, Add Ammo, etc.)
                StartCoroutine(FlyToPlayerAnimation()); // Run the visual effect
            }
        }
    }

    private IEnumerator FlyToPlayerAnimation()
    {
        float duration = 0.3f;
        float elapsed = 0;
        Vector3 startPos = transform.position;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPos, Camera.main.transform.position, elapsed / duration);
            transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        gameObject.SetActive(false); // Hide the object after it "hits" the camera
    }
}

/*
One Master script → handles common behavior
Small specific scripts → handle unique logic
Interface → works like a “label”

itemLogic.Collect();
    This runs:
        LifeBox Collect()
        Magazine Collect()
        Gift Collect()
    Depending on which object this is.
    Master script doesn’t care what type it is.
This is called:
    🔥 Polymorphism
 */