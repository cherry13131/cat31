using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float gravityScale = 10f;
    public float speed = 5f;
    public float jumpForce = 10f;
    private Rigidbody2D rb;
    private bool isGrounded;

    public bool canMove = true;

    // Update에서 점프 입력을 감지해서 FixedUpdate에서 적용
    private bool jumpRequested = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityScale;
        rb.linearDamping = 0f; // 2D에서는 linearDamping 대신 drag
        // 필요하면 회전 고정
        rb.freezeRotation = true;
    }

    void Update()
    {
        if (!canMove) return;

        // 점프는 한 번만 감지
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            jumpRequested = true;
        }
    }

    void FixedUpdate()
    {
        if (!canMove) return;

        float horizontalMove = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(horizontalMove * speed, rb.linearVelocity.y); // linearVelocity -> velocity

        if (jumpRequested && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); // 즉시 위로 속도 설정
            isGrounded = false;
            jumpRequested = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // 바닥인지 판별 (법선의 y가 충분히 크면 바닥)
        if (collision.contacts.Length > 0 && collision.contacts[0].normal.y > 0.5f)
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // 접촉이 끊기면 착지 해제
        isGrounded = false;
    }
}
