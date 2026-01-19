using UnityEngine;
using System.Collections.Generic;

public class ARRadar : MonoBehaviour
{
    public RectTransform radarRect; // The Radar Background
    public GameObject ghostDotPrefab;
    public float mapScale = 20f;  

    private Dictionary<GameObject, RectTransform> ghostDots = new Dictionary<GameObject, RectTransform>();

    void Update()
    {
        //destroyed ghost
        List<GameObject> currentGhosts = GhostSpawner.instance.aliveGhosts;

        List<GameObject> keysToRemove = new List<GameObject>();
        foreach (var ghost in ghostDots.Keys)
        {
            if (!currentGhosts.Contains(ghost) || ghost == null)
            {
                keysToRemove.Add(ghost);
            }
        }

        foreach (var ghost in keysToRemove)
        {
            Destroy(ghostDots[ghost].gameObject);
            ghostDots.Remove(ghost);
        }

        //Update spawner's list
        foreach (GameObject ghost in currentGhosts)
        {
            if (ghost == null) continue;

            if (!ghostDots.ContainsKey(ghost))
            {
                GameObject dot = Instantiate(ghostDotPrefab, radarRect);
                ghostDots.Add(ghost, dot.GetComponent<RectTransform>());
            }

            // Make player pos (0,0)
            Vector3 relPos = ghost.transform.position - Camera.main.transform.position;

            // Convert World to Rader
            float posX = relPos.x * mapScale;
            float posY = relPos.z * mapScale;

            ghostDots[ghost].anchoredPosition = new Vector2(posX, posY);

            //Keep dots in boundary
            float radarRadius = radarRect.sizeDelta.x / 2f;
            if (ghostDots[ghost].anchoredPosition.magnitude > radarRadius)
            {
                ghostDots[ghost].anchoredPosition = ghostDots[ghost].anchoredPosition.normalized * radarRadius;
            }
        }
    }
}