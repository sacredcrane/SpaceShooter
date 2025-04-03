using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class PlayerWeapon : MonoBehaviour
{
    [Header("Base Settings")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 0.2f;
    [SerializeField] private float projectileSpeed = 20f;

    [Header("Upgrade Settings")]
    [SerializeField] private float[] spreadAnglesPerLevel = { 0f }; // Углы для каждого уровня
    [SerializeField] private int maxUpgradeLevel = 5;

    private float nextFireTime;
    private int currentUpgradeLevel;
    private PlayerInput playerInput;
    private InputAction shootAction;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) Debug.Log("AudioSource not setted up");
        playerInput = GetComponent<PlayerInput>();
        shootAction = playerInput.actions["Attack"];
        currentUpgradeLevel = 0;
    }

    private void Update()
    {
        if (shootAction.ReadValue<float>() > 0.5f && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    public void UpgradeWeapon()
    {
        if (currentUpgradeLevel < maxUpgradeLevel)
        {
            currentUpgradeLevel++;
            UpdateFirePattern();
            Debug.Log(currentUpgradeLevel);
        }
    }

    private void UpdateFirePattern()
    {
        // Пример конфигурации апгрейдов
        spreadAnglesPerLevel = new float[][] {
            new float[] { 0f },                         // Уровень 1
            new float[] { -15f, 15f },                   // Уровень 2
            new float[] { -30f, 0f, 30f },               // Уровень 3 
            new float[] { -45f, -15f, 15f, 45f },        // Уровень 4
            new float[] { -60f, -30f, 0f, 30f, 60f },   // Уровень 5
            new float[] { -90f, -45f, 0f, 45f, 90f }     // Уровень 6
        }[currentUpgradeLevel];
    }

    private void Shoot()
    {
        audioSource.Play();
        foreach (float angle in spreadAnglesPerLevel)
        {
            Debug.Log(angle);                                                    
            Quaternion rotation = firePoint.rotation * Quaternion.Euler(angle, 0, 0);
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, rotation);
            
            if (projectile.TryGetComponent<Rigidbody>(out var rb))
            {
                rb.linearVelocity = projectile.transform.forward * projectileSpeed;
            }
        }
    }
}
