using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 게임 규칙을 총괄하는 게임매니저
/// 
/// </summary>
public class GameManager : ISingleton<GameManager>
{
    public event System.Action OnGameOver;
    public event System.Action OnResetGame;

    [SerializeField] Text m_wave;
    [SerializeField] Text m_monster;
    [SerializeField] Text m_result;
    [SerializeField] Text m_time;
    [SerializeField] GameObject m_pannel;

    public bool m_isGameOver = false;
    public bool m_isGameClear = false;
    public bool m_isGamePaused = false;
    public bool m_isResetGame = false;
    public int m_goalCount = 0;
    private void Update()
    {
        int wave = MonsterManager.Instance.m_waveCount;
        int monster = MonsterManager.Instance.ActiveMonsters.Count;
        float time = MonsterManager.Instance.m_waveTimer;
        m_wave.text = $"{wave}/{m_goalCount}";
        m_monster.text = $"{monster}";
        m_time.text = $"{time:F2}";
        if (m_isGameClear)
        {
            GameCleaer();
            return;
        }

        if (m_isGameOver)
        {
            GameOver();
            return;
        }

        if (m_isResetGame)
        {
            ResetGame();
            return;
        }
    }

    public void GameCleaer()
    {
        m_result.text = "Game Cleared !";
        m_pannel.SetActive(true);
        Debug.Log("Game Cleared !");
    }
    public void GameOver()
    {
        m_result.text = "Game Over !";
        m_pannel.SetActive(true);
        Debug.Log("Game Over !");
        OnGameOver?.Invoke();
    }

    public void ResetGame() 
    {
        m_isGameOver = false;
        m_isGameClear = false;
        m_isGamePaused = false;
        m_isResetGame = false;
        MonsterManager.Instance.ActiveMonsters.Clear();
        MonsterManager.Instance.m_waveTimer = 0f;
        MonsterManager.Instance.m_waveCount = 0;
        OnResetGame?.Invoke();
        Player.Instance.m_Animator.SetTrigger("Respawn");
        Player.Instance.m_Animator.ResetTrigger("Respawn");
        m_pannel.SetActive(false);

    }
    public void NextWave() { }
    public void UpdateMonsterCount(int _currentMonsterCount) { }
}