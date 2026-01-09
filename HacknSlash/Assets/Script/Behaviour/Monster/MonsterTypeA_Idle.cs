using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 몬스터는 Idle 상태일때
/// 웨이포인트를 왔다갔다한다.
/// targetPos는 monster스크립트에서 정해주고 
/// 너는 이동만 하자
/// 
/// </summary>
public class MonsterTypeA_Idle : StateMachineBehaviour
{
    [SerializeField] private int m_walkSpeed = 3;
    [SerializeField] private float maxSpeed = 10f; // 원하는 최대 속도 값 설정
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<Monster>().SetScaleForDirection();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 1. 매 프레임 GetComponent를 줄이기 위해 Monster만 가져옴
        Monster monster = animator.GetComponent<Monster>();
        if (monster == null) return;

        // 2. 방향 및 스케일 업데이트
        monster.CheckDistanceToTarget();
        monster.SetScaleForDirection();
        if (monster.m_HasTarget)
        {
            animator.SetBool("HasTarget", true);
            // Chase로 넘어갈 거라면 여기서 이동을 멈추고 리턴하는 게 깔끔함
            monster.m_Rigidbody2D.linearVelocity = Vector2.zero;
            return;
        }

        // 3. 물리 이동 (UnitData에 캐싱된 Rigidbody 사용)
        if (monster.m_Rigidbody2D != null)
        {
            monster.m_Rigidbody2D.AddForce(monster.UnitDirection * m_walkSpeed);

            // 속도 제한 로직
            Vector2 currentVel = monster.m_Rigidbody2D.linearVelocity; 
            if (currentVel.magnitude > maxSpeed)
            {
                monster.m_Rigidbody2D.linearVelocity = currentVel.normalized * maxSpeed;
            }
        }
    }

}
