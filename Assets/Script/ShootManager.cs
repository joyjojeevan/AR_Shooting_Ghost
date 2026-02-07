using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI; 
using TMPro;

public class ShootManager : MonoBehaviour
{
    public static ShootManager instance;

    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 15f;
    private float bulletMoveDis = 20f;
    public GameObject magazinePrefab;
    public GameObject activeMagazine;
    private Camera mainCam;

    [Header("Ammo Settings")]
    public int maxAmmo = 10;
    internal int currentAmmo;
    public bool isReloading = false;

    [Header("Pool Settings")]
    public int bulletPoolSize = 15;
    private Queue<GameObject> bulletPool = new Queue<GameObject>();



    internal int killedCount = 0;

    void Awake()
    {
        instance = this;
        mainCam = Camera.main;
        InitializeBulletPool();
    }
    void Start()
    {
        currentAmmo = maxAmmo;
        UIManager.Instance.UpdateGameUI();
    }
    void InitializeBulletPool()
    {
        for (int i = 0; i < bulletPoolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            bulletPool.Enqueue(bullet);
        }
    }
    public void ShootButtonPressed()
    {
        if (!isReloading && currentAmmo > 0)
        {
            Shoot();
        }
    }
    public void Reload()
    {
        currentAmmo = maxAmmo;
        isReloading = false;
        UIManager.Instance.UpdateGameUI();
        Debug.Log("Reloaded!");
    }
    void Shoot()
    {
        currentAmmo--;
        UIManager.Instance.UpdateGameUI();
        Debug.Log("Ammo left: " + currentAmmo);

        Ray ray = mainCam.ViewportPointToRay( new Vector3(0.5f, 0.5f, 0));
        // Draw a Ray  s
        RaycastHit hit;
        Vector3 targetPoint;
        GameObject hitGhost = null;
        AudioManager.Instance.PlaySound(SoundType.Fire);

        // create veriable for maxium point  
        if (Physics.Raycast(ray, out hit, bulletMoveDis))
        {
            targetPoint = hit.point;

            if (hit.collider.CompareTag("Reload"))
            {
                HandleReload(hit.collider.gameObject);
                //hit.collider.gameObject.SetActive(false);
                //AudioManager.Instance.PlaySound(SoundType.Claim);
                return;
            }
            if (hit.collider.CompareTag("Ghost"))
            {
                hitGhost = hit.collider.gameObject;
                AudioManager.Instance.PlaySound(SoundType.Kill);
            }
        }
        else
        {
            targetPoint = ray.GetPoint(bulletMoveDis); 
        }

        GameObject bullet = GetBulletFromPool();
        bullet.transform.position = firePoint.position;

        bullet.SetActive(true);
        StartCoroutine(MoveBullet(bullet, targetPoint, hitGhost));
        
        
        if (currentAmmo <= 0 )
        {
            SpawnMagazine();
        }
    }
    public void SpawnMagazine()
    {
        if (ShootManager.instance.magazinePrefab == null) return;

        ShootManager.instance.isReloading = true;

        if (activeMagazine == null)
        {
            activeMagazine = Instantiate(ShootManager.instance.magazinePrefab);
            activeMagazine.tag = "Reload";
        }

        float distance = Random.Range(2f, 4f);
        float angle = Random.Range(0, 360) * Mathf.Deg2Rad;

        float x = Mathf.Cos(angle) * distance;
        float z = Mathf.Sin(angle) * distance;

        Vector3 spawnPos = new Vector3(
            mainCam.transform.position.x + x,
            mainCam.transform.position.y - 0.3f,
            mainCam.transform.position.z + z
        );

        activeMagazine.transform.position = spawnPos;
        activeMagazine.transform.LookAt(mainCam.transform);
        activeMagazine.SetActive(true);

    }
    // get bullet -> return game object
    GameObject GetBulletFromPool()
    {
        if (bulletPool.Count > 0)
        {
            return bulletPool.Dequeue();
        }

        //create new bullet
        GameObject bullet = Instantiate(bulletPrefab);
        bullet.SetActive(false);
        return bullet;
    }
    void ReturnBulletToPool(GameObject bullet)
    {
        bullet.SetActive(false);
        bulletPool.Enqueue(bullet);
    }
    IEnumerator MoveBullet(GameObject bullet, Vector3 target, GameObject ghost)
    {
        while (bullet.activeInHierarchy &&
               Vector3.Distance(bullet.transform.position, target) > 0.05f)
        {
            bullet.transform.position = Vector3.MoveTowards(
                bullet.transform.position,
                target,
                bulletSpeed * Time.deltaTime
            );

            bullet.transform.LookAt(target);
            yield return null;
        }

        if (ghost != null)
        {
            GhostSpawner.instance.ReturnGhostToPool(ghost);
            AddScore();
        }

        ReturnBulletToPool(bullet);
    }
    public void HandleReload(GameObject magazine)
    {
        Reload();
        magazine.SetActive(false);
    }

    public void AddScore()
    {
        killedCount++;
        LevelManager.Instance.AddKill();
        UIManager.Instance.UpdateGameUI();
    }
    public int GetKilledCount()
    {
        return killedCount;
    }
    //private void OnDisable() // Triggered when ReturnToPool or SetActive(false) is called
    //{
    //    if (StoryManager.Instance != null)
    //    {
    //        StoryManager.Instance.StartMainGame();
    //    }
    //}
}

/* Raycast = Checking if that ray hits something

Physics.Raycast(ray, out hit, 50f)
Means:
➡️ Shoot the ray
➡️ Check up to 50 units distance
➡️ If it touches any collider:
    Return true
    Save hit information in hit
*/