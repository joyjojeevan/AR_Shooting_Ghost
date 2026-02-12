using UnityEngine;
using TMPro;
using System.Collections;

public class StartTrigger : MonoBehaviour, ICollectable
{
    public TextMeshProUGUI instructionText;
    private float displayDuration = 5f;

    public void Collect()
    {
        StoryManager.Instance.OnBoxTouched();
        instructionText.text = "Signal located. You are in the Anomaly Zone. Find 'The Gift' to stabilize reality.";
        StartCoroutine(FadeText());
        AudioManager.Instance.PlaySound(SoundType.Claim);
    }
    private IEnumerator FadeText()
    {
        yield return new WaitForSeconds(displayDuration);
        float elapsed = 0;
        float fadeTime = 1f;

        while (elapsed < fadeTime)
        {
            elapsed += Time.deltaTime;
            // Gradually lower the alpha to 0
            instructionText.color = new Color(instructionText.color.r, instructionText.color.g, instructionText.color.b, 1 - (elapsed / fadeTime));
            yield return null;
        }
        instructionText.text = "";
    }
    //public GameObject firstPracticeGhost;

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("MainCamera"))
    //    {
    //        StoryManager.Instance.OnBoxTouched();
    //        BeginInvestigation();
    //    }
    //}

    //void BeginInvestigation()
    //{
    //    instructionText.text = "Signal located. You are in the Anomaly Zone. Find 'The Gift' to stabilize reality.";

    //    gameObject.SetActive(false);

    //    AudioManager.Instance.PlaySound(SoundType.Claim);
    //}
}