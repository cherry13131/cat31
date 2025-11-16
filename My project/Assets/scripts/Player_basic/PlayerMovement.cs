using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float gravityScale = 10f;       // 기본 중력 세기 (Start에서 설정)
    public float speed = 5f;               // 좌우 이동 속도
    public float jumpForce = 10f;          // 점프 힘

    // **수정**: 공중에서 더 빨리 떨어지도록 중력 세기를 증가시킬 배수
    public float fallMultiplier = 2.5f;

    public bool canMove = true;            // GameManager 등 외부에서 이동/점프 제어

    private Rigidbody2D rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityScale;
        rb.freezeRotation = true; // 회전 방지
    }

    void Update()
    {
        // Update에서는 점프 입력 감지 (점프는 FixedUpdate에서 처리)
        // FixedUpdate에서도 Input.GetKeyDown()을 사용할 수 있지만, 
        // 일반적으로 Update에서 입력 감지 후 플래그 설정 후 FixedUpdate에서 물리 적용
    }

    void FixedUpdate()
    {
        if (!canMove)
        {
            rb.linearVelocity = Vector2.zero; // 제어 불가 시 멈춤
            return;
        }

        // 1. 좌우 이동 처리
        float moveX = Input.GetAxisRaw("Horizontal");
        Vector2 targetVelocity = new Vector2(moveX * speed, rb.linearVelocity.y);

        // 땅에서는 즉시 반응, 공중에서는 Lerp를 사용하여 부드럽게
        if (isGrounded)
        {
            rb.linearVelocity = targetVelocity;
        }
        else
        {
            // 공중 이동 반응성 약간 증가 (0.08f에서 0.15f로)
            rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, targetVelocity, 0.15f);
        }

        // 2. 점프 처리
        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            // AddForce 대신 velocity 직접 설정 (기존 코드 유지)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false; // 바닥 떠났다고 표시
        }

        // 3. **빠른 낙하 적용 (수정된 로직)** 🚀
        if (rb.linearVelocity.y < 0f) // 하강 중일 때
        {
            // 기본 중력(gravityScale)에 fallMultiplier를 곱하여 더 큰 중력 스케일 적용
            rb.gravityScale = gravityScale * fallMultiplier;
        }
        else if (rb.linearVelocity.y > 0f) // 상승 중일 때 (예: 점프 정점에서 더 빨리 떨어지도록)
        {
            // '점프 정점'에서만 약간의 가속을 원하면 이 부분을 추가하거나 수정
            // 예: 약간 더 높은 중력 스케일 적용 (fallMultiplier의 절반 정도)
            rb.gravityScale = gravityScale * 1.5f;
        }
        else // 땅에 있거나 velocity.y가 0일 때
        {
            // 기본 중력 스케일로 복원
            rb.gravityScale = gravityScale;
        }
    }

    // 바닥 감지 (기존 코드 유지)
    void OnCollisionEnter2D(Collision2D collision)
    {
        // 충돌 지점의 법선(Normal) y값이 0.5보다 크면(위쪽을 향하면) 땅으로 간주
        if (collision.contacts.Length > 0 && collision.contacts[0].normal.y > 0.5f)
            isGrounded = true;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // isGrounded = false; // 충돌에서 벗어날 때 바로 false로 설정 (이것이 적절한지는 게임 스타일에 따라 다름)
    }

    // **추가 권장**:
    // OnCollisionStay2D를 사용하여 isGrounded를 계속 갱신하는 것이 더 안정적일 수 있습니다.
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.contacts.Length > 0)
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y > 0.5f) // 위쪽 표면과 충돌하는 경우
                {
                    isGrounded = true;
                    return;
                }
            }
        }
    }
}