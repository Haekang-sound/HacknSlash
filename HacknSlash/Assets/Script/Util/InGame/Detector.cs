using UnityEngine;
using UnityEngine.Events;

public class Detector : MonoBehaviour
{
    [SerializeField] private float m_detectionRange = 10.0f;
    [SerializeField] private LayerMask m_targetLayer;
    [SerializeField] private Vector2 m_offset = Vector2.zero;
    [SerializeField] private Vector2 m_currentDirection = Vector2.right;
    [SerializeField] private UnitData m_owner = null;
    [SerializeField] private CircleCollider2D m_circleCollider2D = null;
    [SerializeField] private bool m_showDebugGizmo = true;
    public UnityEvent OnDetected;
    public UnityEvent OnAttack;
    public UnityEvent OffAttack;

    private void Start()
    {
        // 1. 내 오브젝트에서 찾거나, 없으면 부모에서 찾기
        if (m_owner == null)
        {
            m_owner = GetComponent<UnitData>();
            if (m_owner == null)
            {
                m_owner = GetComponentInParent<UnitData>();
            }
        }

        if (m_circleCollider2D == null) m_circleCollider2D = GetComponent<CircleCollider2D>();

        if (m_owner == null)
        {
            Debug.LogWarning($"{name}: UnitData를 찾을 수 없습니다! Detector가 작동하지 않습니다.");
        }

        OnDetected.AddListener(m_owner.FindTarget);
        OnAttack.AddListener(m_owner.OnAttack);
        OffAttack.AddListener(m_owner.OffAttack);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable target = collision.GetComponent<IDamageable>();
        if (target != null && m_targetLayer == target.GetLayerMask())
        {
            OnAttack?.Invoke();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        IDamageable target = collision.GetComponent<IDamageable>();
        if (target != null && m_targetLayer == target.GetLayerMask())
        {
            OffAttack?.Invoke();
        }
    }
    private void FixedUpdate()
    {
        if (m_owner == null) return;

        // 2. UnitData의 방향이 유효할 때만 갱신 (멈춰있을 땐 마지막 방향 유지)
        if (m_owner.UnitDirection.sqrMagnitude > 0.001f)
        {
            m_currentDirection = m_owner.UnitDirection;
        }

        DetectTarget(m_currentDirection);
    }

    private void DetectTarget(Vector2 dir)
    {
        // 방향이 없으면 리턴
        if (dir == Vector2.zero) return;

        Vector2 origin = (Vector2)transform.position + m_offset;

        // 3. 파라미터 dir 사용
        RaycastHit2D hit = Physics2D.Raycast(origin, dir, m_detectionRange, m_targetLayer);

        if (hit.collider != null)
        {
            OnDetected?.Invoke();
        }
    }

    private void OnDrawGizmos()
    {
        if (m_showDebugGizmo)
        {
            Gizmos.color = Color.red;
            Vector2 origin = (Vector2)transform.position + m_offset;
            Gizmos.DrawRay(origin, m_currentDirection * m_detectionRange);
        }
    }
}