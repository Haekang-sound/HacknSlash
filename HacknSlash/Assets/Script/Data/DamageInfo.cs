using UnityEngine;

public enum DamageType
{
    Normal = 0,
}

public struct DamageInfo
{
    public DamageInfo(DamageType _damage, float _amount, Vector2 pos, Vector2 _knockback)
    {
        amount = _amount;
        hitPoint = pos;
        knockbackForce = _knockback;  // 넉백 벡터 
        damageType = _damage;   // 데미지 타입 
        attacker = null;
    }
    public float amount;            // 데미지 양                                
    public Attacker attacker;     // 공격한 오브젝트 
    public Vector2 hitPoint;        // 타격 위치 
    public Vector2 knockbackForce;  // 넉백 벡터 
    public DamageType damageType;   // 데미지 타입 
}
