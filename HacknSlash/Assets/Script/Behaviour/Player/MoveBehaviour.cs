using UnityEngine;

public class MoveBehaviour : PlayerSMB
{
    //private float m_dierction = 0;

    //public float maxSpeed = 10f; // 원하는 최대 속도 값 설정
    //public float accelerationPower = 4f; // 가속 힘
    //public float decelerationSpeed = 50f; // 감속하는 힘의 크기

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_dierction = Player.Instance.UnitDirection.x;
        animator.SetFloat("DirectionValue", m_dierction);
        Player.Instance.ReadDirection();
        Player.Instance.m_InputReader.onAttack += SetAttackON;
        Player.Instance.m_InputReader.onMove += SetDirection;
        Player.Instance.m_InputReader.onMove += SetMoveON;
        Player.Instance.m_InputReader.offMove += SetMoveOff;
        Player.Instance.m_InputReader.onJump += SetJumpON;
        Player.Instance.m_InputReader.onCrouch += SetCrouchON;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Player.Instance.UnitDirection = Player.Instance.m_InputReader.moveComposite;
        animator.SetFloat("DirectionValue", m_dierction);
        animator.SetBool("IsMove", Player.Instance.m_InputReader.IsMovehPressed());
        if (Player.Instance.m_InputReader.IsMovehPressed())
        {
            Accelerate();
        }
        else
        {
            DeAccelerate();
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Player.Instance.m_InputReader.onAttack -= SetAttackON;
        Player.Instance.m_InputReader.onMove -= SetDirection;
        Player.Instance.m_InputReader.onMove -= SetMoveON;
        Player.Instance.m_InputReader.offMove -= SetMoveOff;
        Player.Instance.m_InputReader.onJump -= SetJumpON;
        Player.Instance.m_InputReader.onCrouch -= SetCrouchON;
    }

    //public void SetMoveOff()
    //{
    //    Player.Instance.m_Animator.SetBool("IsMove", false);
    //}
    //public void SetMoveOn()
    //{
    //    Player.Instance.m_Animator.SetBool("IsMove", true);
    //}

    //public void Accelerate()
    //{
    //    // 1. 힘을 가함
    //    if (Player.Instance.m_Animator.GetBool("IsGround"))
    //    {
    //        Vector3 groundNormal = new Vector3(Player.Instance.GroundNormal.x, Player.Instance.GroundNormal.y, 0);
    //        Vector2 currentDirection = Vector3.Cross(groundNormal, Vector3.forward).normalized;
    //        currentDirection *= Player.Instance.UnitDirection.x;
    //        Player.Instance.m_Rigidbody2D.AddForce(currentDirection * accelerationPower, ForceMode2D.Impulse);
    //    }
    //    else
    //    {
    //        Player.Instance.m_Rigidbody2D.AddForceX(Player.Instance.UnitDirection.x * accelerationPower, ForceMode2D.Impulse);
    //    }

    //    // 2. 현재 속도 벡터를 가져옴
    //    Vector2 currentVel = Player.Instance.m_Rigidbody2D.linearVelocity;

    //    // 3. X축 속도를 -maxSpeed 와 +maxSpeed 사이로 고정
    //    if (Player.Instance.OnGround)
    //    {
    //        // 땅에 있을 때는 전체 속도 크기를 제한 (경사면 Y축 가속 방지)     
    //        if (currentVel.magnitude > maxSpeed)
    //        {
    //            currentVel = currentVel.normalized * maxSpeed;
    //        }
    //    }
    //    else
    //    {
    //        // 공중에서는 X축 속도만 제한 (낙하 속도 유지)                     
    //        currentVel.x = Mathf.Clamp(currentVel.x, -maxSpeed, maxSpeed);
    //    }
    //    // 4. 보정된 속도를 다시 적용
    //    Player.Instance.m_Rigidbody2D.linearVelocity = currentVel;
    //}

    //public void DeAccelerate()
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
    //public void SetJumpON()
    //{
    //    Player.Instance.m_Animator.SetBool("IsJumping", true);
    //    Player.Instance.m_Animator.SetBool("IsGround", false);
    //}

    //public void SetDirection()
    //{
    //    m_dierction = Player.Instance.UnitDirection.x;
    //}
    //public void SetCrouchON()
    //{
    //    Player.Instance.m_Animator.SetBool("IsCrouch", true);
    //}
    //public void SetAttackON()
    //{
    //    Player.Instance.m_Animator.SetTrigger("OnAttack");
    //}
}
