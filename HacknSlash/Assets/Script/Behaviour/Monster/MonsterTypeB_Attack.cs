using UnityEngine;

public class MonsterTypeB_Attack : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Monster monster = animator.GetComponent<Monster>();
        monster.SetScaleForDirection();
        if (monster.m_Rigidbody2D != null)
                monster.m_Rigidbody2D.linearVelocity = Vector2.zero;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Monster monster = animator.GetComponent<Monster>();
        if (monster == null) return;

        monster.UpdateTargetPosToPlayer();
        monster.SetScaleForDirection();

        // 공격 범위 안에 들어왔다면?
        if (monster.m_IsInRange)
        {
            animator.SetBool("IsInRange", true);

            if (monster.m_Rigidbody2D != null)
                //monster.m_Rigidbody2D.linearVelocity = Vector2.zero;

                return;
        }
        else
        {
            animator.SetBool("IsInRange", false);
        }

        //// 3. 물리 이동 (UnitData에 캐싱된 Rigidbody 사용)
        //if (monster.m_Rigidbody2D != null)
        //{
        //    monster.m_Rigidbody2D.AddForce(monster.UnitDirection * m_walkSpeed);

        //    // 속도 제한 로직
        //    Vector2 currentVel = monster.m_Rigidbody2D.linearVelocity;
        //    if (currentVel.magnitude > maxSpeed)
        //    {
        //        monster.m_Rigidbody2D.linearVelocity = currentVel.normalized * maxSpeed;
        //    }
        //}
    }
}
