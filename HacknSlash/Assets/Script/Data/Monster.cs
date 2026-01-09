using System.Collections.Generic;
using UnityEngine;
public enum MonsterType
{
    Skeleton,
    FlyingEye
}
public class Monster : UnitData
{
    [SerializeField] private bool m_hasTarget = false;
    [SerializeField] private bool m_isInRange = false;
    [SerializeField] private List<WayPoint> m_wayPoints = null;
    [SerializeField] private ProjectileLauncher m_launcher = null;

    public Player m_target;
    int m_wayPointIndex = 0;
    public Vector2 m_targetPos;
    WayPoint m_currentWayPoint = null;
    public bool isLeft = false;
    public MonsterType m_type;


    public bool m_HasTarget { get { return m_hasTarget; } set { m_hasTarget = value; } }
    public bool m_IsInRange { get { return m_isInRange; } set { m_isInRange = value; } }

    // 몬스터가 죽었을 때 처리하기위함
    public event System.Action<Monster> onDeath;


    new private void Start()
    {
        base.Start();
        if (m_wayPoints != null)
        {
            SetTargetPos(m_wayPoints[m_wayPointIndex].transform.position);
            m_currentWayPoint = m_wayPoints[m_wayPointIndex];
        }
        m_launcher = GetComponent<ProjectileLauncher>();

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 충돌한 물체가 웨이포인트인지 확인
        if (collision.TryGetComponent<WayPoint>(out WayPoint hitPoint))
        {
            // 내가 목표로 하던 웨이포인트가 맞는지 확인
            if (hitPoint == m_currentWayPoint)
            {
                // 다음 인덱스 계산
                m_wayPointIndex = (m_wayPointIndex + 1) % m_wayPoints.Count;

                // 새 목표 설정
                m_currentWayPoint = m_wayPoints[m_wayPointIndex];
                SetTargetPos(m_currentWayPoint.transform.position);
            }
        }
    }
    public List<WayPoint> GetWayPoints() { return m_wayPoints; }

    public void SetWayPoints(List<WayPoint> _list)
    {
        m_wayPoints = _list;
    }
    public override void Shoot()
    {
        base.Shoot();
        m_launcher.Fire(UnitDirection);
    }
    public override void FindTarget()
    {
        base.FindTarget();
        m_HasTarget = true;
        m_target = Player.Instance;
        m_targetPos = m_target.transform.position;
    }
    override public void OnAttack()
    {
        m_isInRange = true;
    }
    override public void OffAttack()
    {
        m_isInRange = false;
    }

    override protected void OnDeath(DamageInfo _info)
    {
        // 일단 멀리날아가는게 좋겠다.
        m_Rigidbody2D.AddForce(_info.knockbackForce * 100, ForceMode2D.Impulse);
        onDeath?.Invoke(this);
    }
    public void OnDeathEvent()
    {
        onDeath?.Invoke(this);
    }

    override protected void OnHit(DamageInfo _info)
    {
        m_Rigidbody2D.AddForce(_info.knockbackForce * 10, ForceMode2D.Impulse);
    }

    public Vector2 GetTargetPos() { return m_wayPoints[m_wayPointIndex].transform.position; }

    /// <summary>
    /// 방향과 타겟을 설정
    /// </summary>
    /// <param name="target"></param>
    public void SetTargetPos(Vector3 target)
    {
        UnitDirection = (target - transform.position).normalized;
        m_targetPos = target;
    }

    override public void ReadDirection()
    {
        UnitDirection = new Vector2(m_targetPos.x - transform.position.x, m_targetPos.y - transform.position.y).normalized;
        if (UnitDirection.x < float.Epsilon) isLeft = true;
        else isLeft = false;

    }

    public void SetScaleForDirection()
    {
        ReadDirection();

        Vector3 temp = transform.localScale;
        // 왼쪽이면 -1, 아니면 1을 곱한 절대값
        float targetSign = isLeft ? -1f : 1f;

        // 현재 부호와 목표 부호가 다르면 적용
        if (Mathf.Sign(temp.x) != targetSign)
        {
            temp.x = Mathf.Abs(temp.x) * targetSign;
            transform.localScale = temp;
        }
    }

    /// <summary>
    /// 새로운 도착지점을 설정합니다.
    /// </summary>
    public void OnArriveWayPoint(WayPoint _point)
    {
        // 찾고 있던 녀석인지 확인
        if (_point != m_currentWayPoint) return;

        // waypoint의 인덱스를 올리고 
        ++m_wayPointIndex;
        if (m_wayPointIndex == m_wayPoints.Count) m_wayPointIndex = 0;

        // 새로 도착지점을 설정한다.
        m_currentWayPoint = m_wayPoints[m_wayPointIndex];
        SetTargetPos(m_currentWayPoint.transform.position);
    }


    public void CheckDistanceToTarget()
    {
        // 목표까지의 거리 제곱 (성능을 위해 sqrMagnitude 사용)
        float sqrDist = ((Vector3)m_targetPos - transform.position).sqrMagnitude;

        // 거리가 0.1f (제곱하면 0.01f) 보다 작으면 도착으로 간주
        if (sqrDist < 0.01f)
        {
            ArriveAtTarget();
        }
    }

    private void ArriveAtTarget()
    {
        m_wayPointIndex = (m_wayPointIndex + 1) % m_wayPoints.Count;
        m_currentWayPoint = m_wayPoints[m_wayPointIndex];
        SetTargetPos(m_currentWayPoint.transform.position);
    }
    public void UpdateTargetPosToPlayer()
    {
        if (m_target != null)
        {
            SetTargetPos(m_target.transform.position);
        }
    }

    override public void SetAttackerPos()
    {
        // 3. 플레이어 위치 (월드 좌표)
        Vector3 playerPos = Player.Instance.transform.position;

        // 4. 방향 벡터 계산 (목표 - 내위치)
        Vector3 pos = new Vector3(m_direction.x, m_direction.y, 0);
        m_attacker.transform.position = transform.position + pos;
    }

}
