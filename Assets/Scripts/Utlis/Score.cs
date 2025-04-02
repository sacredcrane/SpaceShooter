using UnityEngine;
using UnityEngine.Events;

public class Score : MonoBehaviour
{
    [SerializeField] private int scorePerEnemy = 10;
    
    [Header("Events")]
    public UnityEvent<int> OnScoreUpdated;

    private int currentScore;

    public void AddScore()
    {
        currentScore += scorePerEnemy;
        OnScoreUpdated?.Invoke(currentScore);
    }
}
