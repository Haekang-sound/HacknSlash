using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{

    [Header("Projectile Settings")]
    [SerializeField] private Projectile m_projectilePrefab;
    [SerializeField] private Transform m_firePoint;

    [Header("Stats")]
    [SerializeField] private float m_speed = 10f;
    [SerializeField] private float m_damage = 10f;
    [SerializeField] private float m_knockback = 5f;
    [SerializeField] private LayerMask m_targetLayer;
    [SerializeField] private int m_poolSize = 20; // 풀 사이즈 설정

    // 오브젝트 풀 선언
    private ObjectPool<Projectile> m_pool;

    private void Awake() // Start 대신 Awake에서 초기화 추천
    {
        // 전용 풀 생성 (부모는 Launcher 아래에 두거나 별도 관리)
        GameObject poolRoot = new GameObject($"{gameObject.name}_ProjectilePool");
        m_pool = new ObjectPool<Projectile>(m_projectilePrefab, m_poolSize, poolRoot.transform);
    }

    public void Fire(Vector2 direction)
    {
        if (m_projectilePrefab == null) return;
        if (m_firePoint == null) m_firePoint = transform;

        // 1. 풀에서 가져오기 (Instantiate 대체)
        Projectile proj = m_pool.Get();

        // 2. 위치 및 회전 설정 (Get()은 활성화만 해주므로 위치는 직접 잡아야 함)
        proj.transform.position = m_firePoint.position;
        proj.transform.rotation = Quaternion.identity;

        // 일단 초기화
        proj.Initialize(direction, m_speed, m_damage, m_knockback, m_targetLayer);
        proj.OnDestroyRequested += ReturnToPool;
    }

    private void ReturnToPool(Projectile proj)
    {
        m_pool.Return(proj); // 풀에 반납 (비활성화됨)
    }
}
