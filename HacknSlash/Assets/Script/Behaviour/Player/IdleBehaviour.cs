using UnityEngine;

public class IdleBehaviour : PlayerSMB
{
    //public float decelerationSpeed = 50f; // 감속하는 힘의 크기

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetFloat("DirectionValue", Player.Instance.UnitDirection.x);

        Player.Instance.m_InputReader.onAttack += SetAttackON;
        Player.Instance.m_InputReader.onMove += SetMoveON;
        Player.Instance.m_InputReader.onJump += SetJumpON;
        Player.Instance.m_InputReader.onCrouch += SetCrouchON;

        if (Player.Instance.m_InputReader.IsCrouchPressed()) SetCrouchON();

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetFloat("DirectionValue", Player.Instance.UnitDirection.x);
        if (Player.Instance.OnSlope) Player.Instance.m_Rigidbody2D.gravityScale = 0;
        else Player.Instance.m_Rigidbody2D.gravityScale = 1;
        if (Player.Instance.m_InputReader.IsMovehPressed()) animator.SetBool("IsMove", true);
        DeAccelerate();
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Player.Instance.m_Rigidbody2D.gravityScale = 1;
        Player.Instance.m_InputReader.onAttack -= SetAttackON;
        Player.Instance.m_InputReader.onMove -= SetMoveON;
        Player.Instance.m_InputReader.onJump -= SetJumpON;
        Player.Instance.m_InputReader.onCrouch -= SetCrouchON;

    }

    //public void SetMoveON()
    //{
    //    Player.Instance.m_Animator.SetBool("IsMove", true);
    //}

    //public void SetJumpON()
    //{
    //    Player.Instance.m_Animator.SetBool("IsJumping", true);
    //    Player.Instance.m_Animator.SetBool("IsGround", false);
    //}

    //public void SetCrouchON()
    //{
    //    Player.Instance.m_Animator.SetBool("IsCrouch", true);
    //}


    //public void SetAttackON()
    //{
    //    Player.Instance.m_Animator.SetTrigger("OnAttack");
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
}
