using UnityEngine;

public class GhostMovement : MonoBehaviour
{
    public Transform player;

    [Header("Movement Settings")]
    public float moveSpeed = 1.5f;        
    public float rotationSpeed = 5.0f;
    public float stopDistance = 1.2f;    

    [Header("Area Settings")]
    public float roamRadius = 5f;
    public float heightOffset = 1.2f;

    private Vector3 targetPoint;
    private float timer;
    private float changeDirInterval = 4f;

    void Start()
    {
        if (player == null) player = Camera.main.transform;
        SetNewTargetPoint();
    }

    void Update()
    {
        if (player == null) return;

        //Check distance to player
        float distToPlayer = Vector3.Distance(transform.position, player.position);

        //Timer to pick a new random spot near the player
        timer += Time.deltaTime;
        if (timer >= changeDirInterval || distToPlayer > roamRadius)
        {
            SetNewTargetPoint();
            timer = 0;
        }

        //Move towards the target point
        // We use MoveTowards because it is more reliable than simple addition
        transform.position = Vector3.MoveTowards(transform.position, targetPoint, moveSpeed * Time.deltaTime);

        //Always look at the player (more creepy/natural for a ghost)
        Vector3 lookPos = (player.position - transform.position).normalized;
        lookPos.y = 0; // Keep the ghost upright
        if (lookPos != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * rotationSpeed);
        }

        //Visual Debug - Draw a line in the editor so you can see the target
        Debug.DrawLine(transform.position, targetPoint, Color.cyan);
    }

    void SetNewTargetPoint()
    {
        //Pick a random point inside a circle around the player
        Vector2 randomCircle = Random.insideUnitCircle * roamRadius;

        //Target is Player Position + Random Offset + Height
        targetPoint = new Vector3(
            player.position.x + randomCircle.x,
            player.position.y + Random.Range(-1.5f, 2f),
            player.position.z + randomCircle.y
        );

        //Safety: If the point is too close to the player, push it back
        if (Vector3.Distance(targetPoint, player.position) < stopDistance)
        {
            targetPoint += (targetPoint - player.position).normalized * stopDistance;
        }
    }
    void OnEnable()
    {
        timer = 0;

        if (player == null) player = Camera.main.transform;
        if (player != null)
        {
            SetNewTargetPoint();
        }
    }
}