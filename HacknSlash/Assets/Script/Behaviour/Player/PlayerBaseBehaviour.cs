using UnityEngine;

public class PlayerSMB : StateMachineBehaviour
{
    [SerializeField] protected float m_dierction = 0;
    [SerializeField] protected float maxSpeed = 10f; // 원하는 최대 속도 값 설정
    [SerializeField] protected float accelerationPower = 4f; // 가속 힘
    [SerializeField] protected float decelerationSpeed = 50f; // 감속하는 힘의 크기
    [SerializeField] protected float dashPower = 1f; // 대쉬 힘
    [SerializeField] protected float jumpPower = 20f; // 점프 힘

    public void SetDirection()
    {
        m_dierction = Player.Instance.UnitDirection.x;
    }
    public void SetMoveON()
    {
        SetDirection();
        Player.Instance.m_Animator.SetBool("IsMove", true);
    }
    public void SetMoveOff()
    {
        Player.Instance.m_Animator.SetBool("IsMove", false);
    }
    public void SetJumpON()
    {
        Player.Instance.m_Animator.SetBool("IsJumping", true);
        Player.Instance.m_Animator.SetBool("IsGround", false);
    }

    public void SetCrouchOff()
    {
        Player.Instance.m_Animator.SetBool("IsCrouch", false);
    }
    public void SetAttackON()
    {
        Player.Instance.m_Animator.SetTrigger("OnAttack");
    }


    public void SetJumpOff()
    {
        Player.Instance.m_Animator.SetBool("IsJumping", false);
    }
    public void OnLanded()
    {
        Player.Instance.m_Animator.SetBool("IsGround", true);
    }
    public void SetCrouchON()
    {
        Player.Instance.m_Animator.SetBool("IsCrouch", true);
    }

    public void DeAccelerate()
    {
        Vector2 currentVel = Player.Instance.m_Rigidbody2D.linearVelocity;

        // 속도가 거의 0이면 아예 0으로 만듦 (연산 낭비 및 미세 떨림 방지)
        if (currentVel.sqrMagnitude < 0.1f)
        {
            Player.Instance.m_Rigidbody2D.linearVelocity = Vector2.zero;
            return;
        }

        // 감속 로직:
        // 현재 속도에 비례해서 감속력을 키웁니다. (빠를수록 더 강하게 제동)
        // frictionFactor가 클수록 빡빡하게 멈춥니다.
        // 기존 decelerationSpeed는 "최소 감속량"으로 사용

        float frictionFactor = 5.0f; // 제동 계수 (조절 필요)
        float dynamicDecel = decelerationSpeed + (Mathf.Abs(currentVel.x) * frictionFactor);

        if (Player.Instance.OnGround)
        {
            // 땅: MoveTowards로 0을 향해 감속
            Vector2 newVel = Vector2.MoveTowards(currentVel, Vector2.zero, dynamicDecel * Time.deltaTime);
            Player.Instance.m_Rigidbody2D.linearVelocity = newVel;
        }
        else
        {
            // 공중: 공기 저항 (보통 땅보다는 적게)
            float airResistance = dynamicDecel * 0.5f; // 공중은 절반만 적용
            float newVelX = Mathf.MoveTowards(currentVel.x, 0, airResistance * Time.deltaTime);
            Player.Instance.m_Rigidbody2D.linearVelocity = new Vector2(newVelX, currentVel.y);
        }
    }
    //{
    //    Vector2 currentVel = Player.Instance.m_Rigidbody2D.linearVelocity;

    //    // 속도가 0이 아닐 때만(0.001f 오차 범위) 로직 수행
    //    if (currentVel.sqrMagnitude > float.Epsilon)
    //    {
    //        if (Player.Instance.OnGround)
    //        {
    //            // 땅에 있을 때는 X, Y 모두 감속 (경사면에서 튀어오름 방지)                                       
    //            Vector2 newVel = Vector2.MoveTowards(currentVel, Vector2.zero, decelerationSpeed * Time.deltaTime);
    //            Player.Instance.m_Rigidbody2D.linearVelocity = newVel;
    //        }
    //        else
    //        {
    //            // 공중에서는 X축만 제어 (공중 조작감 유지)                                                       
    //            float newVelX = Mathf.MoveTowards(currentVel.x, 0, decelerationSpeed * Time.deltaTime);
    //            Player.Instance.m_Rigidbody2D.linearVelocity = new Vector2(newVelX, currentVel.y);
    //        }
    //    }
    //}


    public void Accelerate()
    {
        // 1. 목표 속도 계산
        float targetSpeedX = Player.Instance.UnitDirection.x * maxSpeed;

        // 2. 현재 속도 가져오기
        Vector2 currentVel = Player.Instance.m_Rigidbody2D.linearVelocity;

        // 3. 폭발적 가속 (Explosive Acceleration) 로직
        // 목표 속도와의 차이 비율 (0 ~ 1 이상)
        // 차이가 클수록(출발 시) 가속도가 폭발적으로 증가
        float diff = Mathf.Abs(targetSpeedX - currentVel.x);
        float diffRatio = Mathf.Clamp01(diff / maxSpeed);

        // 가속 보정 계수:
        // 출발 시(diffRatio=1) -> accelerationPower의 3배 적용
        // 최고 속도 근처(diffRatio=0) -> accelerationPower의 1배 적용
        float explosiveFactor = 1f + (diffRatio * diffRatio * 4f);

        float finalAccel = accelerationPower * explosiveFactor;

        // MoveTowards로 부드럽게(그러나 빠르게) 목표 속도 접근
        float newSpeedX = Mathf.MoveTowards(currentVel.x, targetSpeedX, finalAccel * Time.deltaTime);

        // 4. 적용
        if (Player.Instance.OnGround)
        {
            // 경사면 처리
            Vector3 groundNormal = new Vector3(Player.Instance.GroundNormal.x, Player.Instance.GroundNormal.y, 0);
            Vector2 slopeDir = Vector3.Cross(groundNormal, Vector3.forward).normalized;

            // slopeDir은 "오른쪽"을 향하는 접선 벡터
            // newSpeedX 부호를 사용하여 방향 결정 (slopeDir 자체는 항상 오른쪽 위/아래)
            // Player.Instance.UnitDirection.x를 곱하면 안 됨 (newSpeedX에 이미 부호가 있음)

            // slopeDir은 항상 오른쪽 기준이므로, newSpeedX가 음수면 왼쪽으로 감
            Player.Instance.m_Rigidbody2D.linearVelocity = slopeDir * newSpeedX;
        }
        else
        {
            // 공중: X축만 변경, Y축 유지
            Player.Instance.m_Rigidbody2D.linearVelocity = new Vector2(newSpeedX, currentVel.y);
        }
    }
}

