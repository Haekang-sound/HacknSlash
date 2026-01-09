using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 플레이어의 데이터를
/// 담당하는 클래스 Player
/// 
/// </summary>
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Damageable))]
public class UnitData : MonoBehaviour
{
    public Vector2 UnitDirection { get { return m_direction; } set { m_direction = value; } }
    // 바닥의 기울기 정보 (기본값은 평지)
    public Vector2 GroundNormal { get; set; } = Vector2.up;
    public bool OnGround
    {
        get
        {
            return isGround;
        }
        set
        {
            isGround = value;
        }
    }
    public bool OnSlope
    {
        get
        {
            return isSlope;
        }
        set
        {
            isSlope = value;
        }
    }
    public bool m_IsDeath
    {
        get
        {
            return isDeath;
        }
        set
        {
            isDeath = value;
        }
    }
    public Animator m_Animator { get; set; }
    public Rigidbody2D m_Rigidbody2D { get; set; }
    public Damageable m_damageable;

    [SerializeField] protected GameObject m_attacker;
    [SerializeField] protected Vector2 m_direction = Vector2.right;
    [SerializeField] protected bool isGround = false;
    [SerializeField] protected bool isSlope = false;
    [SerializeField] protected bool isDeath = false;

    protected void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_damageable = GetComponent<Damageable>();

        m_damageable.OnDeath -= OnDeath;
        m_damageable.OnDeath += OnDeath;
        m_damageable.OnHit -= OnHit;
        m_damageable.OnHit += OnHit;
        //CustomStart();
    }
    protected virtual void CustomStart() { }
    public virtual void FindTarget(){}
    public virtual void Shoot(){}
    public virtual void OnAttack(){}
    public virtual void OffAttack(){}
    public virtual void ReadDirection() { }
    public virtual void SetAttackerPos() { }
    public virtual void ActiveAttacker() 
    {
        SetAttackerPos();
        m_attacker.SetActive(true);
    }
    public virtual void InActiveAttacker() { m_attacker.SetActive(false); }
    protected virtual void OnDeath(DamageInfo _info){}
    protected virtual void OnHit(DamageInfo _info){}

}
