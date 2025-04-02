using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private float shieldDuration = 3f;
    [SerializeField] private GameObject shield;

    [Header("Events")]
    public UnityEvent<int> OnHealthChanged;
    public UnityEvent<bool> OnShieldStateChanged;

    private int currentHealth;
    private bool isShieldActive;

    private void Start()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth);
    }

    public void TakeDamage(int damage)
    {
        if(isShieldActive) return;

        currentHealth = Mathf.Max(currentHealth - damage, 0);
        OnHealthChanged?.Invoke(currentHealth);

        if(currentHealth <= 0) HandleDeath();
        else ActivateShield();
    }

    private void ActivateShield()
    {
        if(shield == null) return;
        
        isShieldActive = true;
        shield.SetActive(isShieldActive);
        OnShieldStateChanged?.Invoke(true);
        Invoke(nameof(DeactivateShield), shieldDuration);
    }

    private void DeactivateShield()
    {
        isShieldActive = false;
        shield.SetActive(isShieldActive);
        OnShieldStateChanged?.Invoke(false);
    }

    private void HandleDeath()
    {
        Debug.Log("Game Over!");
        Time.timeScale = 0;
    }
}