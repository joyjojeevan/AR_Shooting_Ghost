using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class ARRadar : MonoBehaviour
{
    public RectTransform radarRect; // The Radar Background
    public GameObject ghostDotPrefab;
    public float mapScale = 20f;

    [Header("Pickups")]
    public ShootManager shootManager;
    public Image magazineDotUI;
    public Image lifeBoxDotUI;

    private Dictionary<GameObject, RectTransform> ghostDots = new Dictionary<GameObject, RectTransform>();

    void Update()
    {
        float radarRadius = radarRect.sizeDelta.x / 2f;
        float rotationY = Camera.main.transform.eulerAngles.y;

        // 1. HANDLE GHOSTS
        HandleGhosts(radarRadius, rotationY);

        // 2. HANDLE MAGAZINE (Performance Fix: No FindWithTag)
        if (shootManager != null)
        {
            UpdatePickupDot(shootManager.activeMagazine, magazineDotUI, radarRadius, rotationY);
        }

        // 3. HANDLE LIFE BOX (Using the Manager Instance)
        if (LifeBoxManager.Instance != null)
        {
            // Note: Ensure 'lifeBox' is public or has a public getter in LifeBoxManager
            UpdatePickupDot(LifeBoxManager.Instance.activeBox, lifeBoxDotUI, radarRadius, rotationY);
        }

        // 4. ROTATE RADAR BACKGROUND
        radarRect.localRotation = Quaternion.Euler(0, 0, rotationY);
    }
    private void HandleGhosts(float radarRadius, float rotationY)
    {
        List<GameObject> currentGhosts = GhostSpawner.instance.aliveGhosts;

        // Remove old dots
        List<GameObject> keysToRemove = new List<GameObject>();
        foreach (var ghost in ghostDots.Keys)
        {
            if (ghost == null || !currentGhosts.Contains(ghost))
                keysToRemove.Add(ghost);
        }

        foreach (var ghost in keysToRemove)
        {
            Destroy(ghostDots[ghost].gameObject);
            ghostDots.Remove(ghost);
        }

        // Update/Add dots
        foreach (GameObject ghost in currentGhosts)
        {
            if (ghost == null || !ghost.activeInHierarchy) continue;

            if (!ghostDots.ContainsKey(ghost))
            {
                GameObject dot = Instantiate(ghostDotPrefab, radarRect);
                ghostDots.Add(ghost, dot.GetComponent<RectTransform>());
            }

            Vector3 relPos = ghost.transform.position - Camera.main.transform.position;
            RectTransform dotRT = ghostDots[ghost];

            dotRT.anchoredPosition = new Vector2(relPos.x, relPos.z) * mapScale;
            ApplyBoundary(dotRT, radarRadius);

            // Keep dot upright
            dotRT.localRotation = Quaternion.Euler(0, 0, -rotationY);
        }
    }
    // Generic helper for Magazine and LifeBox
    private void UpdatePickupDot(GameObject worldObj, Image dotUI, float radius, float rotY)
    {
        if (dotUI == null) return;

        if (worldObj != null && worldObj.activeInHierarchy)
        {
            dotUI.gameObject.SetActive(true);
            Vector3 relPos = worldObj.transform.position - Camera.main.transform.position;

            dotUI.rectTransform.anchoredPosition = new Vector2(relPos.x, relPos.z) * mapScale;
            ApplyBoundary(dotUI.rectTransform, radius);

            // Keep dot upright
            dotUI.rectTransform.localRotation = Quaternion.Euler(0, 0, -rotY);
        }
        else
        {
            dotUI.gameObject.SetActive(false);
        }
    }

    private void ApplyBoundary(RectTransform rt, float radius)
    {
        if (rt.anchoredPosition.magnitude > radius)
        {
            rt.anchoredPosition = rt.anchoredPosition.normalized * radius;
        }
    }
}
//void Update()
//{
//    //GameObject magazine = GameObject.FindWithTag("Reload");
//    //destroyed ghost

//    /* move HandleGhosts */

//    //List<GameObject> currentGhosts = GhostSpawner.instance.aliveGhosts;

//    //List<GameObject> keysToRemove = new List<GameObject>();
//    //foreach (var ghost in ghostDots.Keys)
//    //{
//    //    if (!currentGhosts.Contains(ghost) || ghost == null)
//    //    {
//    //        keysToRemove.Add(ghost);
//    //    }
//    //}
//    //// update to get 
//    //foreach (var ghost in keysToRemove)
//    //{
//    //    Destroy(ghostDots[ghost].gameObject);
//    //    ghostDots.Remove(ghost);
//    //}

//    ////Update spawner's list
//    //foreach (GameObject ghost in currentGhosts)
//    //{
//    //    if (ghost == null) continue;

//    //    if (!ghostDots.ContainsKey(ghost))
//    //    {
//    //        GameObject dot = Instantiate(ghostDotPrefab, radarRect);
//    //        ghostDots.Add(ghost, dot.GetComponent<RectTransform>());
//    //    }

//    //    // Make player pos (0,0)
//    //    Vector3 relPos = ghost.transform.position - Camera.main.transform.position;

//    //    // Convert World to Rader
//    //    float posX = relPos.x * mapScale;
//    //    float posY = relPos.z * mapScale;

//    //    ghostDots[ghost].anchoredPosition = new Vector2(posX, posY);

//    //    //Keep dots in boundary
//    //    float radarRadius = radarRect.sizeDelta.x / 2f;
//    //    if (ghostDots[ghost].anchoredPosition.magnitude > radarRadius)
//    //    {
//    //        ghostDots[ghost].anchoredPosition = ghostDots[ghost].anchoredPosition.normalized * radarRadius;
//    //    }
//    //}

//    // Rotate camera -> not need
//    float rotationY = Camera.main.transform.eulerAngles.y;
//    radarRect.localRotation = Quaternion.Euler(0, 0, rotationY);

//    GameObject magazine = GameObject.FindWithTag("Reload");
//    if (magazine != null && magazine.activeInHierarchy)
//    {
//        magazineDotUI.gameObject.SetActive(true);

//        Vector3 relPos = magazine.transform.position - Camera.main.transform.position;
//        magazineDotUI.rectTransform.anchoredPosition = new Vector2(relPos.x, relPos.z) * mapScale;

//        // Boundary Check
//        float radarRadius = radarRect.sizeDelta.x / 2f;
//        if (magazineDotUI.rectTransform.anchoredPosition.magnitude > radarRadius)
//        {
//            magazineDotUI.rectTransform.anchoredPosition = magazineDotUI.rectTransform.anchoredPosition.normalized * radarRadius;
//        }

//        // USE THE EXISTING rotationY (Don't use 'float' here)
//        magazineDotUI.rectTransform.localRotation = Quaternion.Euler(0, 0, -rotationY);
//    }
//    else
//    {
//        if (magazineDotUI != null) magazineDotUI.gameObject.SetActive(false);
//    }


//    //so they stay upright
//    foreach (var dot in ghostDots.Values)
//    {
//        dot.localRotation = Quaternion.Euler(0, 0, -rotationY);
//    }
//    if (magazine != null && magazine.activeInHierarchy)
//    {
//        magazineDotUI.rectTransform.localRotation = Quaternion.Euler(0, 0, -rotationY);
//    }
//}