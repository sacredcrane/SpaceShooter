using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Level Settings")]
    [SerializeField] private int baseEnemies = 20;
    [SerializeField] private int enemiesPerLevel = 5;

    [Header("Boss Settings")]
    [SerializeField] private GameObject bossPrefab;      // Префаб босса
    [SerializeField] private Transform bossSpawnPoint;   // Точка появления босса
    
    [Header("Events")]
    public UnityEvent<int> OnLevelChanged;
    public UnityEvent OnShopOpened;
    public UnityEvent OnGameRestarted;
    public UnityEvent<int> OnScoreUpdated;

    [Header("Skybox Materials")]
    [SerializeField] private Material[] _skyboxMaterials;

    public int CurrentScore;

    

    private int _currentLevel = 0;
    private bool _isGamePaused;
    private bool _isDead;

    public void AddScore(int score = 10)
    {
        CurrentScore += score;
        OnScoreUpdated?.Invoke(CurrentScore);

    }

    private void ChooseSkybox() 
    {
        if (_skyboxMaterials == null || _skyboxMaterials.Length == 0)
        {
            Debug.LogError("No skybox materials assigned!");
            return;
        }

        int randomIndex = Random.Range(0, _skyboxMaterials.Length);
        Material selectedMaterial = _skyboxMaterials[randomIndex];

        RenderSettings.skybox = selectedMaterial;

        DynamicGI.UpdateEnvironment();
    }

    private void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);
        FindAnyObjectByType<PlayerHealth>().OnDeath.AddListener(LevelCompleted);
        RestartLevel();
    }

    public void LevelCompleted(bool death)
    {
        _isDead = death;
        if (!death)
        {
            // Уровень пройден успешно – запускаем босса вместо моментального завершения уровня.
            GameObject boss = Instantiate(bossPrefab, bossSpawnPoint.position, Quaternion.identity);
            Boss bossScript = boss.GetComponent<Boss>();
            if (bossScript != null)
            {
                bossScript.OnBossDefeated.AddListener(HandleBossDefeated);
            }
        }
        else
        {
            Time.timeScale = 0;
            _isGamePaused = true;
            OnShopOpened?.Invoke();
        }
    }

    private void HandleBossDefeated()
    {
        // После уничтожения босса завершаем уровень.
        Time.timeScale = 0;
        _isGamePaused = true;
        OnShopOpened?.Invoke();
    }

    public void RestartLevel()
    {
        if (!_isDead) _currentLevel++;
        Time.timeScale = 1;
        _isGamePaused = false;
        ChooseSkybox();
        OnLevelChanged?.Invoke(_currentLevel);
        OnGameRestarted?.Invoke();
    }

    public int GetEnemiesCount()
    {
        return baseEnemies + (_currentLevel - 1) * enemiesPerLevel;
    }
}