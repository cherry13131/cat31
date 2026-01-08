using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float gravityScale = 10f;
    public float speed = 5f;
    public float jumpForce = 10f;
    public float fallMultiplier = 2.5f;
    public bool canMove = true;

    private Rigidbody2D rb;
    private bool isGrounded;

    // ★ 추가: 애니메이터를 제어하기 위한 변수를 선언합니다.
    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityScale;
        rb.freezeRotation = true;

        // ★ 추가: 실제 캐릭터에 붙어있는 Animator 컴포넌트를 가져옵니다.
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // 수평(A,D), 수직(W,S) 입력값 받기
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // ★ 수정: anim 변수가 null이 아닐 때만 실행되도록 안전하게 짭니다.
        if (anim != null)
        {
            // 입력값이 하나라도 있으면(움직이면) true, 없으면 false
            if (h != 0 || v != 0)
            {
                anim.SetBool("isMoving", true);
            }
            else
            {
                anim.SetBool("isMoving", false);
            }
        }
        // --- 좌우 반전 코드 추가 시작 ---
        if (h > 0)
        {
            // 오른쪽 키를 누르면 원래 방향(false)
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (h < 0)
        {
            // 왼쪽 키를 누르면 좌우 반전(true)
            GetComponent<SpriteRenderer>().flipX = true;
        }
        // --- 좌우 반전 코드 추가 끝 ---
    }

    void FixedUpdate()
    {
        if (!canMove)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        float moveX = Input.GetAxisRaw("Horizontal");
        Vector2 targetVelocity = new Vector2(moveX * speed, rb.linearVelocity.y);

        if (isGrounded)
        {
            rb.linearVelocity = targetVelocity;
        }
        else
        {
            rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, targetVelocity, 0.15f);
        }

        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;
        }

        // 빠른 낙하 로직
        if (rb.linearVelocity.y < 0f)
        {
            rb.gravityScale = gravityScale * fallMultiplier;
        }
        else if (rb.linearVelocity.y > 0f)
        {
            rb.gravityScale = gravityScale * 1.5f;
        }
        else
        {
            rb.gravityScale = gravityScale;
        }
    }

    // (이하 충돌 체크 로직은 동일)
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts.Length > 0 && collision.contacts[0].normal.y > 0.5f)
            isGrounded = true;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.contacts.Length > 0)
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
}