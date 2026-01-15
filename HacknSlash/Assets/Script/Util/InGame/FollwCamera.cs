using UnityEngine;

public class FollwCamera : ISingleton<FollwCamera>
{
    [SerializeField]
    private Transform m_target = null;

    [SerializeField]
    [Tooltip("이동 속도에 영향을 주는 댐핑 계수입니다.")]
    private float m_damping = 2.0f;
    private float m_fixedZ;

    public bool isShaking = false;
    Vector3 _currnetPosition;

    private void Start()
    {
        m_fixedZ = transform.position.z;

        if (m_target == null)
        {
            m_target = Player.Instance.transform;
        }

        Player.Instance.onDeathShake += ActiveShake;
        Player.Instance.onDeathShake += InActiveShake;

    }

    private void FixedUpdate()
    {
        // player를 따라가자
        if (m_target == null)
        {
            if (Player.Instance != null)
            {
                m_target = Player.Instance.transform;
            }
            else
            {
                return;
            }
        }

        // 목표 위치 설정 (Z축은 카메라의 원래 위치 유지)
        Vector3 targetPos = new Vector3(m_target.position.x, m_target.position.y, m_fixedZ);

        // 거리 계산
        float distance = Vector3.Distance(transform.position, targetPos);

        // 거리가 매우 가까우면 위치를 고정하여 미세한 떨림 방지
        if (distance < 0.01f)
        {
            //transform.position = targetPos;
            return;
        }

        // 2차 함수 곡선 속도 댐핑:
        // 속도가 거리에 비례하는 일반적인 Lerp와 달리, t값에 거리를 한번 더 곱하여
        // 이동량이 거리의 제곱에 비례하도록(속도가 빨라지도록) 설정
        float t = distance * m_damping * Time.deltaTime;

        transform.position = Vector3.Lerp(transform.position, targetPos, t);
    }

    void Update()
    {

        if (isShaking)
        {
            Shake();
        }
        else
        {
            _currnetPosition = transform.position;
        }
    }

    private void Shake()
    {
        Vector3 temp = transform.position;
        temp.x = UnityEngine.Random.Range(0.99f, 1.01f) * _currnetPosition.x;
        temp.y = UnityEngine.Random.Range(0.99f, 1.01f) * _currnetPosition.y;
        transform.position = temp;
    }
    public void Shake(float value)
    {
        Vector3 temp = transform.position;
        temp.x = UnityEngine.Random.Range(0.99f* value, 1.01f * value) * _currnetPosition.x;
        temp.y = UnityEngine.Random.Range(0.99f * value, 1.01f * value) * _currnetPosition.y;
        transform.position = temp;
    }

    public void ActiveShake() { isShaking = true; }
    public void InActiveShake() { isShaking = false; }
}
