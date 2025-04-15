using UnityEngine;
using UnityEngine.Events;

public class Boss : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHits = 10;
    private int currentHits = 0;

    [Header("Shooting Settings")]
    public float shootingInterval = 2f;     // Интервал между залпами
    public GameObject projectilePrefab;     // Префаб снаряда
    public float projectileSpeed = 10f;       // Скорость снаряда
    public float projectileLifeTime = 5f;     // Время жизни снаряда

    [Header("Invulnerability Settings")]
    [Tooltip("Время в секундах, в течение которого босс неуязвим после спауна")]
    public float invulnerabilityDuration = 3f;
    private float invulnerabilityTimer;

    // Событие, которое вызывается при уничтожении босса
    public UnityEvent OnBossDefeated;

    private float shootingTimer = 0f;

    private void Start()
    {
        shootingTimer = shootingInterval;
        invulnerabilityTimer = invulnerabilityDuration;
    }

    private void Update()
    {
        // Обновление таймера неуязвимости
        if(invulnerabilityTimer > 0f)
        {
            invulnerabilityTimer -= Time.deltaTime;
            // Здесь можно добавить визуальные эффекты неуязвимости, например, мерцание
        }

        shootingTimer -= Time.deltaTime;
        if (shootingTimer <= 0f)
        {
            ShootInAllDirections();
            shootingTimer = shootingInterval;
        }
    }

    private void ShootInAllDirections()
    {
        // Выпускаем снаряды в 6 равномерно распределённых направлениях (каждые 60 градусов)
        for (int i = 0; i < 6; i++)
        {
            float angle = i * 60f;
            Vector3 direction = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad),
                                            Mathf.Sin(angle * Mathf.Deg2Rad),
                                            0f);

            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = direction * projectileSpeed;
            }
            Destroy(projectile, projectileLifeTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Если ещё включена неуязвимость — игнорируем попадание
        if (invulnerabilityTimer > 0f)
            return;

        // Если объект имеет скрипт Projectile, считаем это попаданием по боссу
        if (other.GetComponent<Projectile>() != null)
        {
            currentHits++;
            Destroy(other.gameObject); // Уничтожаем пулю игрока

            if (currentHits >= maxHits)
            {
                BossDefeated();
            }
        }
    }

    private void BossDefeated()
    {
        OnBossDefeated?.Invoke();
        Destroy(gameObject);
    }
}
