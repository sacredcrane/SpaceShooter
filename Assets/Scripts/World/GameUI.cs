using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Score score;

    private void OnEnable()
    {
        playerHealth.OnHealthChanged.AddListener(UpdateHealth);  
        score.OnScoreUpdated.AddListener(UpdateScore);     
    }

    private void OnDisable()
    {
        playerHealth.OnHealthChanged.RemoveListener(UpdateHealth);  
        score.OnScoreUpdated.RemoveListener(UpdateScore);    
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
