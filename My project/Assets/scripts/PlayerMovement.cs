using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float gravityScale = 10f;
    public float speed = 5f;
    public float jumpForce = 10f;
    private Rigidbody2D rb;
    private bool isGrounded;

    public bool canMove = true;
    private bool jumpRequested = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityScale;
        rb.linearDamping = 0f; // linearDamping 대신 drag
        rb.freezeRotation = true;
    }

    void Update()
    {
        if (!canMove) return;

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            jumpRequested = true;
        }
    }

    void FixedUpdate()
    {
        if (!canMove) return;

        float horizontalMove = Input.GetAxisRaw("Horizontal");

        if (isGrounded)
        {
            // 땅에서는 즉시 방향 반응
            rb.linearVelocity = new Vector2(horizontalMove * speed, rb.linearVelocity.y);
        }
        else
        {
            // 공중에서는 살짝 관성 (Lerp로 부드럽게 변화)
            Vector2 targetVelocity = new Vector2(horizontalMove * speed, rb.linearVelocity.y);
            rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, targetVelocity, 0.08f);
        }

        if (jumpRequested && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;
            jumpRequested = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts.Length > 0 && collision.contacts[0].normal.y > 0.5f)
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }
}
