using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float gravityScale = 10f;
    public float speed = 5f;
    public float jumpForce = 12f;
    public float fallMultiplier = 2.5f;
    public bool canMove = true;

    private Rigidbody2D rb;
    private Animator anim;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        rb.gravityScale = gravityScale;
        rb.freezeRotation = true;
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");

        // 👉 이동 애니메이션
        if (anim != null)
        {
            anim.SetBool("isMoving", h != 0);
            anim.SetBool("isJumping", !isGrounded);
        }

        // 👉 방향 전환
        if (h > 0)
            GetComponent<SpriteRenderer>().flipX = false;
        else if (h < 0)
            GetComponent<SpriteRenderer>().flipX = true;

        // 👉 점프 입력 (한 번만)
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && canMove)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;
        }
    }

    void FixedUpdate()
    {
        if (!canMove)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return;
        }

        float moveX = Input.GetAxisRaw("Horizontal");

        // 👉 좌우 이동
        if (isGrounded)
        {
            rb.linearVelocity = new Vector2(moveX * speed, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = Vector2.Lerp(
                rb.linearVelocity,
                new Vector2(moveX * speed, rb.linearVelocity.y),
                0.1f
            );
        }

        // 👉 낙하 가속 (핵심)
        if (rb.linearVelocity.y < 0)
        {
            rb.gravityScale = gravityScale * fallMultiplier;
        }
        else
        {
            rb.gravityScale = gravityScale;
        }
    }

    // 👉 바닥 체크
    void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                isGrounded = true;
                return;
            }
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                isGrounded = true;
                return;
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }
}