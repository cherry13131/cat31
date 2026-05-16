using UnityEngine;

public class MisaileAi : MonoBehaviour
{
    public float speed = 5f;
    public float rotateSpeed = 200f;

    private Rigidbody2D rb;
    private Transform target;
    private Collider2D col;

    private bool isDead = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        GameObject player =
            GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            target = player.transform;
        }

        Destroy(gameObject, 8f);
    }

    void FixedUpdate()
    {
        if (isDead) return;

        if (target == null)
        {
            Destroy(gameObject);
            return;
        }
        Vector2 targetPos =
            (Vector2)target.position + Vector2.up * 0.8f;

        Vector2 direction =
            (targetPos - rb.position).normalized;

        float rotateAmount =
            Vector3.Cross(direction, transform.up).z;

        rb.angularVelocity = -rotateAmount * rotateSpeed;
        rb.linearVelocity = transform.up * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead) return;

        if (other.CompareTag("Player"))
        {
            isDead = true;

            // 물리 정지
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.simulated = false;

            // 충돌 끄기
            col.enabled = false;

            Destroy(gameObject);
        }
    }
}