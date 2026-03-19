using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 12f;
    public float gravityScale = 3f;
    public float fallMultiplier = 2.5f;

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

        // 👉 점프 (X 건드리지 않음)
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;
        }

        // 👉 방향 (항상 유지)
        if (moveInput > 0)
            sr.flipX = false;
        else if (moveInput < 0)
            sr.flipX = true;

        // 👉 애니메이션
        if (anim != null)
        {
            anim.SetBool("isMoving", moveInput != 0 && isGrounded);
            anim.SetBool("isJumping", !isGrounded);
        }
    }

    void FixedUpdate()
    {
        // 👉 이동 (지상에서만 방향 제어)
        if (isGrounded)
        {
            rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);
        }

        // 👉 낙하 가속
        if (rb.linearVelocity.y < 0)
            rb.gravityScale = gravityScale * fallMultiplier;
        else
            rb.gravityScale = gravityScale;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        CheckGround(collision);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        CheckGround(collision);
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }

    void CheckGround(Collision2D collision)
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
}