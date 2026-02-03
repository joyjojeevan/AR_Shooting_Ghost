using UnityEngine;
using TMPro;

public class StartTrigger : MonoBehaviour
{
    public TextMeshProUGUI instructionText;
    //public GameObject firstPracticeGhost;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            StoryManager.Instance.OnBoxTouched();
            BeginInvestigation();
        }
    }

    void BeginInvestigation()
    {
        instructionText.text = "Signal located. You are in the Anomaly Zone. Find 'The Gift' to stabilize reality.";

        //firstPracticeGhost.SetActive(true);

        gameObject.SetActive(false);

        AudioManager.Instance.PlaySound(SoundType.Claim);
    }
}