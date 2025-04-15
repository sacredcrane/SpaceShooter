using UnityEngine;

public class SpaceshipEnemy : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 3f;           // скорость движения к игроку

    [Header("Shooting Settings")]
    public float shootingInterval = 2f;    // интервал между выстрелами
    public GameObject bulletPrefab;        // префаб пули
    public Transform bulletSpawnPoint;     // точка появления пули
    public float bulletSpeed = 10f;        // скорость пули

    private Transform target;              // цель (игрок)
    private float shootingTimer;

    private void Start()
    {
        // Находим игрока по тегу; убедитесь, что у игрока установлен тег "Player"
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
        }
        shootingTimer = shootingInterval;
    }

    private void Update()
    {
        if (target != null)
        {
            // Вычисляем направление к цели
            Vector3 direction = (target.position - transform.position).normalized;
            
            // Передвигаем звездолет к игроку
            transform.position += direction * moveSpeed * Time.deltaTime;

            // Вычисляем угол для поворота (с учётом, что «вперёд» у вас может быть смещён; отладьте при необходимости)
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            // Если ваш спрайт направлен вверх, то можно сделать -90 градусов, иначе подберите смещение
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
        }

        // Таймер стрельбы
        shootingTimer -= Time.deltaTime;
        if (shootingTimer <= 0f)
        {
            Shoot();
            shootingTimer = shootingInterval;
        }
    }

    private void Shoot()
    {
        if (bulletPrefab != null && bulletSpawnPoint != null)
        {
            // Создаем пулю в позиции bulletSpawnPoint с текущим поворотом
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            
            // Если у пули есть компонент Rigidbody, придаем ей скорость
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // bulletSpawnPoint.up указывает направление "вперёд" для пули (важно, чтобы ось Y была направлена туда, куда должен лететь снаряд)
                rb.linearVelocity = bulletSpawnPoint.up * bulletSpeed;
            }
        }
    }
}
