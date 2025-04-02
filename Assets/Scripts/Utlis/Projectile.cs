using UnityEngine;
public class Projectile : MonoBehaviour
{
    [SerializeField] private float lifetime = 2f;
    [SerializeField] private int damage = 10;
    [SerializeField] private GameObject impactEffect;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // if (other.CompareTag("Enemy"))
        // {
        //     other.GetComponent<HealthSystem>().TakeDamage(damage);
        // }

        if (impactEffect != null)
        {
            Instantiate(impactEffect, transform.position, transform.rotation);
        }
        // Destroy(gameObject);
    }
}