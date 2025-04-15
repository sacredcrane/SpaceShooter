using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float bulletSpeed = 10f;
    public float lifeTime = 5f;
    public int damage = 10;

    private void Start()
    {
        // Автоматически удаляем пулю через lifeTime секунд
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        // Двигаем пулю вперед относительно ее локальной оси
        transform.Translate(Vector3.up * bulletSpeed * Time.deltaTime, Space.Self);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Если пуля сталкивается с игроком, наносим урон
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
        // Можно добавить дополнительные условия столкновений (например, уничтожать при попадании в препятствие)
    }
}
