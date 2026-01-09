

using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour, IDamageable
{
    [Header("Health Settings")]
    [SerializeField] private float m_maxHealth = 100f;
    [SerializeField] private float m_currentHealth;
    [SerializeField] private LayerMask m_layer;

    [Header("State")]
    public bool m_isInvincible = false; // 무적 상태 

    // 외부 시스템(UI, 이펙트 매니저)이 구독할 이벤트
    public event System.Action<DamageInfo> OnHit;
    public event System.Action<DamageInfo> OnDeath;

    private void Awake()
    {
        m_currentHealth = m_maxHealth;
    }
    private void Start()
    {
        GameManager.Instance.OnResetGame -= Heal;
        GameManager.Instance.OnResetGame += Heal;
    }
    private void OnEnable()
    {
        m_currentHealth = m_maxHealth;

    }
    public void Heal()
    {
        m_currentHealth = m_maxHealth;

    }
    public bool GetInvincible() { return  m_isInvincible; }
    public void TakeDamage(DamageInfo _info)
    {
        // 이미 죽었거나 무적이면 무시
        if (m_currentHealth <= 0 || m_isInvincible) return;

        // 체력 감소
        m_currentHealth -= _info.amount;

        if (m_currentHealth <= 0)
        {
            m_currentHealth = 0;
            // 죽음 처리
            OnDeath?.Invoke(_info);
            Debug.Log($"{gameObject.name} 사망!");
        }
        else
        {
            // 피격 처리 
            OnHit?.Invoke(_info);
            Debug.Log($"{gameObject.name} 피격! 남은 체력: {m_currentHealth}");
        }
    }

    public LayerMask GetLayerMask()
    {
        return m_layer;
    }
}
