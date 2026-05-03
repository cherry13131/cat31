using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 2f;
    public float chaseSpeedMultiplier = 1.5f;

    [Header("Ground Detection")]
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float detectionDistance = 0.5f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Chase Settings")]
    [SerializeField] private float directionChangeThreshold = 0.1f;

    private Rigidbody2D rb;
    private Transform target;

    private int direction = 1; // 1 = right, -1 = left
    public bool isPlayerDetected { get; private set; }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (isPlayerDetected && target != null)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }

        Move();
        UpdateSpriteDirection();
    }

    void Patrol()
    {
        CheckEdge();
    }

    void ChasePlayer()
    {
        float xDifference = target.position.x - transform.position.x;

        // 너무 가까울 때 좌우로 떨리는 현상 방지
        if (Mathf.Abs(xDifference) > directionChangeThreshold)
        {
            direction = xDifference > 0 ? 1 : -1;
        }
    }

    void Move()
    {
        float currentSpeed = isPlayerDetected
            ? moveSpeed * chaseSpeedMultiplier
            : moveSpeed;

        rb.linearVelocity = new Vector2(
            direction * currentSpeed,
            rb.linearVelocity.y
        );
    }

    void CheckEdge()
    {
        RaycastHit2D groundInfo = Physics2D.Raycast(
            wallCheck.position,
            Vector2.down,
            detectionDistance,
            groundLayer
        );

        if (groundInfo.collider == null)
        {
            Flip();
        }
    }

    void UpdateSpriteDirection()
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * direction;
        transform.localScale = scale;
    }

    public void Flip()
    {
        direction *= -1;
        UpdateSpriteDirection();
    }

    public void SetTarget(Transform player)
    {
        target = player;
        isPlayerDetected = true;
    }

    public void RemoveTarget()
    {
        target = null;
        isPlayerDetected = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (wallCheck == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(
            wallCheck.position,
            wallCheck.position + Vector3.down * detectionDistance
        );
    }
}