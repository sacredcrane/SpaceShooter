using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private PlayerHealth playerHealth;

    private void OnEnable()
    {
        playerHealth.OnHealthChanged.AddListener(UpdateHealth);  
        GameManager.Instance.OnScoreUpdated.AddListener(UpdateScore); 
        GameManager.Instance.OnLevelChanged.AddListener(UpdateLevel);    
    }

    private void OnDisable()
    {
        playerHealth.OnHealthChanged.RemoveListener(UpdateHealth);  
        GameManager.Instance.OnScoreUpdated.RemoveListener(UpdateScore);   
        GameManager.Instance.OnLevelChanged.RemoveListener(UpdateLevel);  
    }

    public void UpdateLevel(int level) 
    {
        levelText.text = $"Level: {level}";
    }

    public void UpdateHealth(int health)
    {
        healthText.text = $"{health}";
    }

    public void UpdateScore(int scr)
    {
        scoreText.text = $"{scr}";
    }

    // private void UpdateShieldUI(bool isActive)
    // {
    //     shieldCooldownImage.fillAmount = isActive ? 1 : 0;
    //     shieldCooldownImage.color = isActive ? Color.cyan : Color.gray;
    // }
}
