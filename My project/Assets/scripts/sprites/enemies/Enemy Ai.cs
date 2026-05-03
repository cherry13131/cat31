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

    [SerializeField] private Transform wallFrontCheck;
    [SerializeField] private float wallCheckDistance = 0.2f;

    [Header("Chase Settings")]
    [SerializeField] private float directionChangeThreshold = 0.1f;

    [Header("Lose Target Delay")]
    [SerializeField] private float loseTargetDelay = 2f;

    private Rigidbody2D rb;
    private Transform target;

    private int direction = 1; // 1 = right, -1 = left
    public bool isPlayerDetected { get; private set; }

    private float loseTimer = 0f;
    private bool isLosingTarget = false;
    private bool isPatrolling = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (isPlayerDetected && target != null)
        {
            isLosingTarget = false;
            loseTimer = 0f;
            isPatrolling = false;

            ChasePlayer();
        }
        else
        {
            HandleLoseTarget();
        }

        if (isPatrolling)
        {
            Patrol();
        }

        Move();
    }

    // ------------------------
    // 타겟 놓쳤을 때 처리 (유일한 하나!)
    // ------------------------
    void HandleLoseTarget()
    {
        if (!isLosingTarget)
        {
            isLosingTarget = true;
            loseTimer = loseTargetDelay;
        }

        loseTimer -= Time.fixedDeltaTime;

        Debug.Log("Patrolling 상태: " + isPatrolling + " / Timer: " + loseTimer);

        if (loseTimer <= 0f)
        {
            isPatrolling = true;
        }
    }

    // ------------------------
    // 순찰
    // ------------------------
    void Patrol()
    {
        CheckEdge();
    }

    // ------------------------
    // 추적
    // ------------------------
    void ChasePlayer()
    {
        float xDifference = target.position.x - transform.position.x;

        if (Mathf.Abs(xDifference) > directionChangeThreshold)
        {
            int newDir = xDifference > 0 ? 1 : -1;

            if (newDir != direction)
            {
                direction = newDir;
                UpdateSpriteDirection();
            }
        }
    }

    // ------------------------
    // 이동
    // ------------------------
    void Move()
    {
        float currentSpeed = !isPatrolling
            ? moveSpeed * chaseSpeedMultiplier   // 추적 중
            : moveSpeed;                         // 순찰 중

        rb.linearVelocity = new Vector2(
            direction * currentSpeed,
            rb.linearVelocity.y
        );
    }

    // ------------------------
    // 낙사 방지
    // ------------------------
    void CheckEdge()
    {
        // 아래 체크 (낙사 방지)
        RaycastHit2D groundInfo = Physics2D.Raycast(
            wallCheck.position,
            Vector2.down,
            detectionDistance,
            groundLayer
        );

        // 앞쪽 체크 (벽 감지)
        RaycastHit2D wallInfo = Physics2D.Raycast(
            wallFrontCheck.position,
            Vector2.right * direction,
            wallCheckDistance,
            groundLayer
        );

        if (groundInfo.collider == null || wallInfo.collider != null)
        {
            Flip();
        }
    }

    // ------------------------
    // 방향 반전
    // ------------------------
    void Flip()
    {
        direction *= -1;
        UpdateSpriteDirection();
    }

    void UpdateSpriteDirection()
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * direction;
        transform.localScale = scale;
    }

    // ------------------------
    // 감지
    // ------------------------
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

    // ------------------------
    // 디버그
    // ------------------------
    private void OnDrawGizmosSelected()
    {
        if (wallCheck == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(
            wallCheck.position,
            wallCheck.position + Vector3.down * detectionDistance
        );
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(
            wallFrontCheck.position,
            wallFrontCheck.position + (Vector3)(Vector2.right * direction * wallCheckDistance)
        );
    }
}