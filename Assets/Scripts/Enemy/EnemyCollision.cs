using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private int playerDamage = 20;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Entered");
    
        var player = other.GetComponent<PlayerHealth>();
        var gameScore = FindAnyObjectByType<Score>();

        if(other.CompareTag("Projectile"))
        {
            DestroyEnemy(gameScore);
            Destroy(other.gameObject);
        }
        else if(player != null)
        {
            player.TakeDamage(playerDamage);
            DestroyEnemy(gameScore);
        }
    }

    private void DestroyEnemy(Score score)
    {
        if(explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }
        score?.AddScore();
        Destroy(gameObject);
    }
}