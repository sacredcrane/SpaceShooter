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
        audioSource.Play();

        if(other.CompareTag("Projectile"))
        {
            GameManager.Instance.AddScore();
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        else if(player != null)
        {
            player.TakeDamage(playerDamage);
            Destroy(gameObject);
        }
    }
}