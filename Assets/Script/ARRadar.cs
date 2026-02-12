using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class ARRadar : MonoBehaviour
{
    [SerializeField] private RectTransform radarRect; // The Radar Background
    [SerializeField] private GameObject ghostDotPrefab;
    private float mapScale = 20f;

    [Header("Pickups")]
    [SerializeField] private ShootManager shootManager;
    [SerializeField] private Image magazineDotUI;
    [SerializeField] private Image lifeBoxDotUI;

    private float radarRadius;
    private Queue<RectTransform> dotPool = new Queue<RectTransform>();
    private Dictionary<GameObject, RectTransform> ghostDots = new Dictionary<GameObject, RectTransform>();

    void Start()
    {
        radarRadius = radarRect.sizeDelta.x / 2f;
        for (int i = 0; i < 10; i++)
        {
            CreateNewDotForPool();
        }
    }
    void Update()
    {
        float rotationY = Camera.main.transform.eulerAngles.y;

        //HANDLE GHOSTS
        HandleGhosts(radarRadius, rotationY);
        //HANDLE MAGAZINE (Performance Fix: No FindWithTag)

            UpdatePickupDot(shootManager.activeMagazine, magazineDotUI, radarRadius, rotationY);
        
        //HANDLE LIFE BOX (Using the Manager Instance)       
            // Note: Ensure 'lifeBox' is public or has a public getter in LifeBoxManager
            UpdatePickupDot(LifeBoxManager.Instance.activeBox, lifeBoxDotUI, radarRadius, rotationY);

        //ROTATE RADAR BACKGROUND
        radarRect.localRotation = Quaternion.Euler(0, 0, rotationY);
    }
    private void HandleGhosts(float radarRadius, float rotationY)
    {
        List<GameObject> currentGhosts = GhostSpawner.instance.aliveGhosts;

        // direct use from Ghost spawner 
        //remove
        // Remove old dots
        foreach (GameObject ghost in currentGhosts)
        {
            if (ghost == null || !ghost.activeInHierarchy) continue;

            // If we don't have a dot for this ghost, get one
            if (!ghostDots.ContainsKey(ghost))
            {
                ghostDots.Add(ghost, GetDotFromPool());
            }

            // Update Position
            Vector3 relPos = ghost.transform.position - Camera.main.transform.position;
            RectTransform dotRT = ghostDots[ghost];

            dotRT.anchoredPosition = new Vector2(relPos.x, relPos.z) * mapScale;
            ApplyBoundary(dotRT, radarRadius);
            dotRT.localRotation = Quaternion.Euler(0, 0, -rotationY);
        }

        // only remove dots
        if (ghostDots.Count > currentGhosts.Count)
        {
            // We still need a temporary list to avoid the "Collection Modified" error,
            // but now this ONLY runs when a ghost actually dies.
            var keysToRemove = new List<GameObject>();
            foreach (var pair in ghostDots)
            {
                if (pair.Key == null || !pair.Key.activeInHierarchy || !currentGhosts.Contains(pair.Key))
                {
                    keysToRemove.Add(pair.Key);
                }
            }

            foreach (var key in keysToRemove)
            {
                ReturnDotToPool(ghostDots[key]);
                ghostDots.Remove(key);
            }
        }
    }
    private RectTransform GetDotFromPool()
    {
        if (dotPool.Count > 0)
        {
            RectTransform dot = dotPool.Dequeue();
            dot.gameObject.SetActive(true);
            return dot;
        }
        else
        {
            return CreateNewDotForPool(true);
        }
    }

    private void ReturnDotToPool(RectTransform dot)
    {
        dot.gameObject.SetActive(false);
        dotPool.Enqueue(dot);
    }

    private RectTransform CreateNewDotForPool(bool active = false)
    {
        GameObject obj = Instantiate(ghostDotPrefab, radarRect);
        RectTransform rt = obj.GetComponent<RectTransform>();
        obj.SetActive(active);
        if (!active) dotPool.Enqueue(rt);
        return rt;
    }
    // Generic helper for Magazine and LifeBox
    private void UpdatePickupDot(GameObject worldObj, Image dotUI, float radius, float rotY)
    {
        //if (dotUI == null) return;

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