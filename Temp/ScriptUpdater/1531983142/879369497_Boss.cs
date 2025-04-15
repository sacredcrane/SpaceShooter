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

    private float shootingTimer = 0f;

    // Событие, которое вызывается при уничтожении босса
    public UnityEvent OnBossDefeated;

    private void Start()
    {
        shootingTimer = shootingInterval;
    }

    private void Update()
    {
        shootingTimer -= Time.deltaTime;
        if (shootingTimer <= 0f)
        {
            ShootInAllDirections();
            shootingTimer = shootingInterval;
        }
    }

    private void ShootInAllDirections()
    {
        // Запускаем снаряды в 6 равномерно распределённых направлениях (каждые 60 градусов)
        for (int i = 0; i < 6; i++)
        {
            float angle = i * 60f;
            // Вычисляем вектор направления по окружности
            Vector3 direction = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad),
                                            Mathf.Sin(angle * Mathf.Deg2Rad),
                                            0f);
            // Создаём снаряд в позиции босса
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            // Применяем скорость к Rigidbody снаряда (убедитесь, что в префабе снаряда есть компонент Rigidbody)
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = direction * projectileSpeed;
            }
            // Уничтожаем снаряд через заданное время, чтобы не засорять сцену
            Destroy(projectile, projectileLifeTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Если объект имеет скрипт Projectile, считаем это попаданием по боссу.
        if (other.GetComponent<Projectile>() != null)
        {
            currentHits++;
            // Уничтожаем пулю (если требуется, можно добавить эффект попадания)
            Destroy(other.gameObject);

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
