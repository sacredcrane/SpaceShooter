using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private int playerDamage = 20;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();   
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Entered");
    
        var player = other.GetComponent<PlayerHealth>();
        var projectile = other.GetComponent<Projectile>();
        if (audioSource != null) audioSource.Play();
        

        if(projectile != null)
        {
            Debug.Log("Got Projectile!");
            GameManager.Instance.AddScore();
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        if(player != null)
        {
            Debug.Log("Got player");
            player.TakeDamage(playerDamage);
            Destroy(gameObject);
        }
    }
}