using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Level Settings")]
    [SerializeField] private int baseEnemies = 20;
    [SerializeField] private int enemiesPerLevel = 5;
    
    [Header("Events")]
    public UnityEvent<int> OnLevelChanged;
    public UnityEvent OnShopOpened;
    public UnityEvent OnGameRestarted;

    private int _currentLevel = 1;
    private bool _isGamePaused;

    private void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void LevelCompleted()
    {
        Time.timeScale = 0;
        _isGamePaused = true;
        OnShopOpened?.Invoke();
    }

    public void RestartLevel()
    {
        _currentLevel++;
        Time.timeScale = 1;
        _isGamePaused = false;
        OnLevelChanged?.Invoke(_currentLevel);
        OnGameRestarted?.Invoke();
    }

    public int GetEnemiesCount()
    {
        return baseEnemies + (_currentLevel - 1) * enemiesPerLevel;
    }
}