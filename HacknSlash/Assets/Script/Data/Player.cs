using UnityEngine;

/// <summary>
/// 플레이어의 데이터를
/// 담당하는 클래스 Player
/// 
/// </summary>
public class Player : UnitData
{
    public FollwCamera cam;
    protected static Player instance;
    public static Player Instance
    {
        get
        {
            if (instance == null)
            {
                // 씬에 있는 T 타입을 찾아본다.
                instance = FindFirstObjectByType<Player>();

                // 그래도 없으면? (선택 사항: 에러를 띄우거나 새로 만들거나)
                if (instance == null)
                {
                    Debug.LogError($"씬에 {typeof(Player)} 컴포넌트가 없습니다!");
                }
            }
            return instance;
        }
    }
    [SerializeField] protected ProjectileLauncher m_launcher = null;
    public InputReader m_InputReader { get; set; }
    public System.Action onDeathShake;
    public System.Action offDeathShake;

    new protected void Start()
    {
        base.Start();

        // 필요한 컴포넌트들을 받아온다.
        m_InputReader = GetComponent<InputReader>();
        m_launcher = GetComponent<ProjectileLauncher>();

        m_InputReader.onMove += ReadDirection;
        m_InputReader.onMagic += Shoot;
    }
  
    private void FixedUpdate()
    {
    }
    // 키입력을 통해 플레이어의 방향을 읽어옵니다.
    override public void ReadDirection()
    {
        UnitDirection = m_InputReader.moveComposite;
    }
    public override void Shoot()
    {
        FollwCamera.Instance.isShaking = true;
        FollwCamera.Instance.Shake(1.1f);
        base.Shoot();
        UnitDirection = CalculateMouseDirection();
        m_launcher.Fire(UnitDirection);
        FollwCamera.Instance.isShaking = false;

    }
    public Vector2 CalculateMouseDirection()
    {
        //// 1. 마우스 위치 (스크린 좌표) 가져오기
        Vector2 screenMousePos = Player.Instance.m_InputReader.mousePos;

        // 2. 스크린 좌표 -> 월드 좌표로 변환
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(new Vector3(screenMousePos.x, screenMousePos.y, -Camera.main.transform.position.z));
        // 참고: 2D에서 Z축 문제 방지를 위해 카메라와의 거리(-z)를 넣거나, 변환 후 z를 0으로 맞춥니다.
        worldMousePos.z = 0;

        // 3. 플레이어 위치 (월드 좌표)
        Vector3 playerPos = transform.position;

        // 4. 방향 벡터 계산 (목표 - 내위치)
        Vector2 direction = (worldMousePos - playerPos).normalized; // 정규화하여 방향만 추출
        
        return direction;
    }
    override public void SetAttackerPos()
    {
        //// 1. 마우스 위치 (스크린 좌표) 가져오기
        Vector2 screenMousePos = Player.Instance.m_InputReader.mousePos;

        // 2. 스크린 좌표 -> 월드 좌표로 변환
        // (메인 카메라가 필요합니다. 2D 게임이라면 Z값은 무시하거나 카메라의 nearClipPlane 등으로 맞춤)
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(new Vector3(screenMousePos.x, screenMousePos.y, -Camera.main.transform.position.z));
        // 참고: 2D에서 Z축 문제 방지를 위해 카메라와의 거리(-z)를 넣거나, 변환 후 z를 0으로 맞춥니다.
        worldMousePos.z = 0;

        // 3. 플레이어 위치 (월드 좌표)
        Vector3 playerPos = Player.Instance.transform.position;

        // 4. 방향 벡터 계산 (목표 - 내위치)
        Vector2 direction = (worldMousePos - playerPos).normalized; // 정규화하여 방향만 추출
        Vector3 pos = new Vector3(direction.x, direction.y, 0);
        m_attacker.transform.position = transform.position + pos;
    }
    override public void ActiveAttacker()
    {
        SetAttackerPos();
        m_attacker.SetActive(true);
    }
    override public void InActiveAttacker() { m_attacker.SetActive(false); }
    override protected void OnDeath(DamageInfo _info)
    {
        GameManager.Instance.m_isGameOver = true;
        m_Animator.SetTrigger("OnDeath");
    }
    override protected void OnHit(DamageInfo _info)
    {
    }

    public void ShakeON()
    {
        onDeathShake?.Invoke();
    }
    public void ShakeOff()
    {
        offDeathShake?.Invoke();
    }
}
