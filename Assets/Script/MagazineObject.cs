using UnityEngine;

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
        transform.Rotate(0, 100 * Time.deltaTime, 0);
    }

    private void OnMouseDown()
    {
        ShootManager.instance.HandleReload(gameObject);
    }
}