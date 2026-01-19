using UnityEngine;
using System.Collections;

public class ShootManager : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 8f;

    public void ShootButtonPressed()
    {
        Shoot();
    }

    void Shoot()
    {
        Ray ray = Camera.main.ViewportPointToRay(
            new Vector3(0.5f, 0.5f, 0)
        );
        // Draw a Ray  s

        RaycastHit hit;

        Vector3 targetPoint;

        GameObject hitGhost = null;

        // create veriable for maxium point  
        if (Physics.Raycast(ray, out hit, 20f))
        {
            targetPoint = hit.point;

            if (hit.collider.CompareTag("Ghost"))
            {
                hitGhost = hit.collider.gameObject;
            }
        }
        else
        {
            targetPoint = ray.GetPoint(20f); // shoot forward
        }
        // object pooling

        GameObject bullet = Instantiate(
            bulletPrefab,
            firePoint.position,
            Quaternion.identity
        );

        StartCoroutine(
            MoveBullet(bullet, targetPoint, hitGhost)
        );
    }
    
    IEnumerator MoveBullet(GameObject bullet, Vector3 target, GameObject ghost)
    {
        while (bullet != null &&
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

        // Bullet reached target
        if (ghost != null)
        {
            GhostSpawner.instance.OnGhostDestroyed(ghost);
            Destroy(ghost); // ghost destroyed ON TOUCH
        }

        Destroy(bullet);
    }
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