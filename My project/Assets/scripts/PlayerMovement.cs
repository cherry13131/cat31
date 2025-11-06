using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float gravityScale = 10f;
    public float speed = 5f;
    public float jumpForce = 10f;
    private Rigidbody2D rb;
    private bool isGrounded;

    public bool canMove = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityScale;
        rb.linearDamping = 0; // 미끄러짐 방지
    }

    void FixedUpdate()
    {
        if (!canMove) return;

        // ⛔ Input.GetAxis -> ✅ Input.GetAxisRaw
        float horizontalMove = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(horizontalMove * speed, rb.linearVelocity.y);

        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // 바닥 접촉 확인
        if (collision.contacts[0].normal.y > 0.5f)
        {
            isGrounded = true;
        }
    }
}
