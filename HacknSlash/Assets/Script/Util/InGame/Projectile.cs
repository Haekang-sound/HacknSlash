using UnityEngine;

/// <summary>
/// 기본 발사체
/// </summary>
public class Projectile : MonoBehaviour
{
    [SerializeField] protected float m_lifeTime = 3.0f; // 최대 생존 시간
    [SerializeField] protected LayerMask m_targetLayer; // 충돌할 레이어
    public event System.Action<Projectile> OnDestroyRequested;

    [SerializeField] protected Vector2 m_direction;
    [SerializeField] protected float m_speed;
    [SerializeField] protected float m_damage;
    [SerializeField] protected float m_knockbackForce;
    [SerializeField] protected float m_timer;

    // 초기화 함수 (발사될 때 호출)
    public void Initialize(Vector2 dir, float speed, float damage, float knockback, LayerMask targetLayer)
    {
        m_direction = dir.normalized;
        m_speed = speed;
        m_damage = damage;
        m_knockbackForce = knockback;
        m_targetLayer = targetLayer;
        m_timer = 0f;

        // 회전 (화살 같은 경우 이동 방향으로 회전)
        // 오른쪽(1,0)이 0도 기준일 때
        float angle = Mathf.Atan2(m_direction.y, m_direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    protected void Update()
    {
        // 로컬 X축(오른쪽) 방향으로 이동 (회전했으므로)
        transform.Translate(Vector3.right * m_speed * Time.deltaTime);

        // 수명 체크
        m_timer += Time.deltaTime;
        if (m_timer >= m_lifeTime)
        {
            DestroyProjectile();
        }
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        // 1. 상대방이 데미지를 입을 수 있는 놈인가? (컴포넌트 가져오기)
        Damageable damageable = collision.GetComponent<Damageable>();
        if (damageable == null) return;

        if (damageable.GetLayerMask() == m_targetLayer)
        {
            // 무적이 아니면 때림
            if (!damageable.GetInvincible())
            {
                DamageInfo info = new DamageInfo(DamageType.Normal, m_damage, transform.position, m_direction * m_knockbackForce);
                damageable.TakeDamage(info);
                DestroyProjectile();
            }

            
        }
        else if (collision.CompareTag("Wall") || collision.CompareTag("Ground"))
        {
            DestroyProjectile();
        }

    }

    protected void DestroyProjectile()
    {
        OnDestroyRequested?.Invoke(this); 
    }
}
