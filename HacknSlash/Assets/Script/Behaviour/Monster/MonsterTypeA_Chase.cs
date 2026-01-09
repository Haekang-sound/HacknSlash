using UnityEngine;

public class MonsterTypeA_Chase : StateMachineBehaviour
{
    [SerializeField] private int m_walkSpeed = 3;
    [SerializeField] private float maxSpeed = 10f; // 원하는 최대 속도 값 설정
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<Monster>().SetScaleForDirection();
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
            return;
        }
        else
        {
            animator.SetBool("IsInRange", false);
        }

        // 3. 물리 이동 (UnitData에 캐싱된 Rigidbody 사용)
        if (monster.m_Rigidbody2D != null)
        {
            monster.m_Rigidbody2D.AddForce(monster.UnitDirection * m_walkSpeed );

            // 속도 제한 로직
            Vector2 currentVel = monster.m_Rigidbody2D.linearVelocity;
            if (currentVel.magnitude > maxSpeed)
            {
                monster.m_Rigidbody2D.linearVelocity = currentVel.normalized * maxSpeed;
            }
        }
    }

}
