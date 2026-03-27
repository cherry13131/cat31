using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 5f;
    public float jumpForce = 12f;
    public float gravityScale = 3f;
    public float fallMultiplier = 2.5f;

    [Header("Ground Detection")]
    public Transform groundCheck;     // 발밑에 배치할 빈 오브젝트 연결
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;     // 'Ground' 레이어만 선택

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;

    private float moveInput;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        rb.gravityScale = gravityScale;
        rb.freezeRotation = true;
    }

    void Update()
    {
        // 👉 입력
        moveInput = Input.GetAxisRaw("Horizontal");

        // 👉 바닥 감지 (OverlapCircle 사용)
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // 👉 점프
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        // 👉 방향 (항상 유지)
        if (moveInput > 0)
            sr.flipX = false;
        else if (moveInput < 0)
            sr.flipX = true;

        // 👉 애니메이션
        if (anim != null)
        {
            // 좌우 이동
            anim.SetBool("isMoving", moveInput != 0 && isGrounded);

            // 공중에 있는지, 땅에 있는지 상태 전달
            anim.SetBool("isGrounded", isGrounded);

            // 핵심! Y축 속도를 전달 (+면 상승, -면 하강)
            anim.SetFloat("velocityY", rb.linearVelocity.y);
        }
    }

    void FixedUpdate()
    {
        //  이동 (공중에서도 움직이게 하려면 if(isGrounded) 제거, 원치 않으면 유지)
        // 여기서는 공중 제어가 가능하도록 if문을 제거했습니다.
        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);

        //  낙하 가속 (점프를 떼거나 떨어질 때)
        if (rb.linearVelocity.y < 0)
            rb.gravityScale = gravityScale * fallMultiplier;
        else
            rb.gravityScale = gravityScale;
    }

    // 에디터에서 바닥 감지 범위가 보이도록 그려주는 기믹
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}