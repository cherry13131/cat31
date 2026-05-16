using UnityEngine;

public class MissileEnemyAI : MonoBehaviour
{
    [Header("미사일 설정")]
    public GameObject missilePrefab;
    public Transform firePoint;

    [Header("공격 설정")]
    public float attackCooldown = 2f;

    private Transform target;
    private float cooldownTimer;

    void Update()
    {
        if (target == null) return;

        cooldownTimer -= Time.deltaTime;

        // 플레이어 방향 바라보기
        Vector2 dir = target.position - transform.position;

        if (dir.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else
            transform.localScale = new Vector3(-1, 1, 1);

        // 공격
        if (cooldownTimer <= 0)
        {
            Shoot();
            cooldownTimer = attackCooldown;
        }
    }

    void Shoot()
    {
        Instantiate(
            missilePrefab,
            firePoint.position,
            firePoint.rotation
        );
    }

    // EnemySensor가 자동 호출
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    // EnemySensor가 자동 호출
    public void ClearTarget()
    {
        target = null;
    }
}