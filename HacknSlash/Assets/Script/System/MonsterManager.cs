using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 몬스터 생성을 관리하는 관리자 
/// 
/// </summary>
public class MonsterManager : ISingleton<MonsterManager>
{
    [Header("Settings")]
    [SerializeField] private Monster m_monsterPrefabA;
    [SerializeField] private Monster m_monsterPrefabB;
    [SerializeField] private int m_poolSizeA = 40;
    [SerializeField] private int m_poolSizeB = 10;
    [SerializeField] private float m_spawnInterval = 1.0f;
    [SerializeField] private int m_waveSize = 10;
    [SerializeField] private float m_waveDuration = 60f;
    [SerializeField] private ObjectPool<Monster> m_monsterPool;
    [SerializeField] private List<WayPoint> m_wayPointsA;
    [SerializeField] private List<WayPoint> m_wayPointsB;
    [SerializeField] private Transform m_spawnA;
    [SerializeField] private Transform m_spawnB;


    // 웨이브 상태 변수
    private int m_spawnedCount = 0;
    private bool m_isSpawning = true;
    private bool m_waveWaiting = false; // 클리어 대기 상태
    public float m_waveTimer = 0f; // 현재 웨이브 경과 시간
    public int m_waveCount = 0;

    public List<Monster> ActiveMonsters { get; private set; } = new List<Monster>();
    public float RemainingTime => Mathf.Max(0, m_waveDuration - m_waveTimer); // 남은 시간 (UI 표시용)

    private float m_spawnTimer;

    private void Awake()
    {
        GameObject poolRoot = new GameObject("EnemyPool_Root");
        m_monsterPool = new ObjectPool<Monster>(m_monsterPrefabA, m_poolSizeA, poolRoot.transform);
        m_monsterPool.AddPool(m_monsterPrefabB, m_poolSizeB);

        foreach (var monster in m_monsterPool.GetPool())
        {
            if (MonsterType.Skeleton == monster.m_type) monster.SetWayPoints(m_wayPointsA);
            if (MonsterType.FlyingEye == monster.m_type) monster.SetWayPoints(m_wayPointsB);
        }
    }

    private void Update()
    {
        // 게임오버이거나 클리어면 중단
        if (GameManager.Instance.m_isGameOver || GameManager.Instance.m_isGameClear) return;

        // 웨이브 타이머 진행 (대기 상태가 아닐 때만)
        if (!m_waveWaiting)
        {
            m_waveTimer += Time.deltaTime;

            // 시간 초과 체크
            if (m_waveTimer >= m_waveDuration)
            {
                // [수정 포인트] 마지막 웨이브인지 확인
                if (m_waveCount >= GameManager.Instance.m_goalCount)
                {
                    // 마지막 웨이브인데 시간 끝남 -> 몬스터가 남아있으면 게임오버
                    if (ActiveMonsters.Count > 0)
                    {
                        GameManager.Instance.m_isGameOver = true;
                    }
                    else
                    {
                        // 몬스터가 없으면 클리어 (혹은 아래 로직에서 처리됨)
                        GameManager.Instance.m_isGameClear = true;
                    }
                    return;
                }
                else
                {
                    // 마지막 웨이브가 아니면 강제 다음 웨이브
                    Debug.Log("Time's Up! Force starting next wave.");
                    StartNextWave();
                }
                return;
            }
        }

        // 1. 스폰 로직
        if (m_isSpawning)
        {
            HandleSpawning();
        }
        // 2. 웨이브 클리어 체크 (스폰 끝남 + 적 없음 + 대기중 아님)
        else if (ActiveMonsters.Count == 0 && !m_waveWaiting)
        {
            // [수정 포인트] 마지막 웨이브를 클리어했다면?
            if (m_waveCount >= GameManager.Instance.m_goalCount)
            {
                GameManager.Instance.m_isGameClear = true;
                return;
            }

            // 일반 웨이브 클리어 -> 다음 웨이브 대기
            m_waveWaiting = true;
            Debug.Log("Wave Cleared Early! Next wave coming soon...");
            Invoke(nameof(StartNextWave), 3.0f); // 3초 휴식 후 다음 웨이브
        }

    }
    public void HandeMonsterDeath(Monster deadMonster)
    {
        deadMonster.m_HasTarget = false;
        deadMonster.m_IsInRange = false;

        // 1. 이벤트 구독 해제 
        deadMonster.onDeath -= HandeMonsterDeath;

        // 2. 리스트에서 제거
        ActiveMonsters.Remove(deadMonster);

        // 3. 풀에 반납
        m_monsterPool.Return(deadMonster);

    }

    private void HandleSpawning()
    {
        m_spawnTimer += Time.deltaTime;
        if (m_spawnTimer >= m_spawnInterval)
        {
            m_spawnTimer = 0;
            SpawnMonster();

            m_spawnedCount++;
            if (m_spawnedCount >= m_waveSize)
            {
                m_isSpawning = false;
            }
        }
    }

    private void StartWave()
    {
        m_spawnedCount = 0;
        m_isSpawning = true;
        m_waveWaiting = false;
        m_waveTimer = 0f; // 타이머 초기화
    }

    private void StartNextWave()
    {
        // 중복 호출 방지
        CancelInvoke(nameof(StartNextWave));
        ++m_waveCount;
        // 목표 웨이브를 넘었다면 더 이상 진행하지 않음
        if (m_waveCount > GameManager.Instance.m_goalCount)
        {
            return;
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.NextWave();
        }
        StartWave();
    }

    private void SpawnMonster()
    {
        Monster monster = m_monsterPool.Get();
        monster.onDeath -= HandeMonsterDeath;
        monster.onDeath += HandeMonsterDeath;

        // 스폰 위치 랜덤 오프셋 적용 (반경 2.0f 내)
        Vector3 randomOffset = (Vector3)UnityEngine.Random.insideUnitCircle * 2.0f;

        if (MonsterType.Skeleton == monster.m_type) monster.SetWayPoints(m_wayPointsA);
        if (MonsterType.FlyingEye == monster.m_type) monster.SetWayPoints(m_wayPointsB);

        // 스폰위치를 지정할것
        if (MonsterType.Skeleton == monster.m_type)
        {
            monster.transform.position = m_spawnA.position;
        }
        else if (MonsterType.FlyingEye == monster.m_type)
        {
            monster.transform.position = m_spawnB.position + randomOffset;
        }

        ActiveMonsters.Add(monster);

        // 적 숫자 갱신
        if (GameManager.Instance != null)
            GameManager.Instance.UpdateMonsterCount(ActiveMonsters.Count);
    }

    public void DespawnEnemy(Monster _monster, int listIndex)
    {
        ActiveMonsters.RemoveAt(listIndex);
        m_monsterPool.Return(_monster);

        // 적 숫자 갱신
        if (GameManager.Instance != null)
            GameManager.Instance.UpdateMonsterCount(ActiveMonsters.Count);
    }

}