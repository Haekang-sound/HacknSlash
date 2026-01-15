using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.AdaptivePerformance.Provider.AdaptivePerformanceSubsystemDescriptor;

public class Attacker : MonoBehaviour
{
    // 이번 공격에서 이미 때린 놈들 목록
    private List<IDamageable> hitTargets = new List<IDamageable>();

    [SerializeField] private LayerMask m_targetLayer; // 적절한 타겟만 처리하자
    [SerializeField] private float m_damage;
    [SerializeField] private GameObject m_attakcker;
    private DamageInfo _info = new DamageInfo();
    private void Start()
    {
        // 데미지정보 세팅
        _info.amount = m_damage;
        _info.attacker = this;
        
    }
    private void OnEnable()
    {
        hitTargets.Clear();
    }

    /// <summary>
    /// 데미지 정보를 전달하는 구간
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable target = collision.GetComponent<IDamageable>();
        if (target != null && m_targetLayer == target.GetLayerMask())
        {
            // 이미 때린 놈이면 무시!
            if (hitTargets.Contains(target)) return;

            // 안 때린 놈이면 목록에 추가하고 데미지 줌
            hitTargets.Add(target);

            _info.knockbackForce = collision.transform.position - transform.position;
            target.TakeDamage(_info);
        }
    }
    private void OnDrawGizmos()
    {
        // 콜라이더가 켜져 있을 때만 그리기
        if (GetComponent<Collider2D>().enabled)
        {
            Gizmos.color = Color.red; // 빨간색으로 표시

            // BoxCollider2D인 경우
            BoxCollider2D box = GetComponent<BoxCollider2D>();
            if (box != null)
            {
                // 회전과 위치를 고려해서 그리기
                Gizmos.matrix = transform.localToWorldMatrix;
                Gizmos.DrawWireCube(box.offset, box.size);
            }

        }
    }
}
