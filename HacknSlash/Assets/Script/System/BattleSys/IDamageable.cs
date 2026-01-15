using UnityEngine;

/// <summary>
/// Damageable의 가장 기본적인 형태인
/// IDamageable
/// 
/// </summary>
public interface IDamageable
{
    public void TakeDamage(DamageInfo _info);
    public LayerMask GetLayerMask();
    public bool GetInvincible();
}
