using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float gravityScale = 10f;
    public float speed = 5f;
    public float jumpForce = 10f;
    private Rigidbody2D rb;
    private bool isGrounded;
    

    public bool canMove = false; // ó������ �� ������

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!canMove) return;

        float horizontalMove = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(horizontalMove * speed, rb.linearVelocity.y);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > -10f)
        {
            isGrounded = true;
        }

        if (!isGrounded)
        {
            rb.linearVelocity += Vector2.down * gravityScale * Time.fixedDeltaTime;
        }
    }
}
