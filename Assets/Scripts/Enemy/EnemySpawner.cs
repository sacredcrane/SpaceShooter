using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [Header("Основные настройки")]
    [SerializeField] private int totalEnemies = 20;
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private int enemiesPerGroup = 3;
    [SerializeField] private float initialDelay = 2f; 
    [SerializeField] private float spawnAreaOffset = 1f;
    [SerializeField] private float EnemyLifetime = 5f;
    private Camera mainCamera;
    private float minX, maxX, minZ, maxZ;

    private int _enemiesSpawned;


    private void Init()
    {
        StopAllCoroutines();
        totalEnemies = GameManager.Instance.GetEnemiesCount();
        StartCoroutine(SpawnRoutine());
    }

    private void Start()
    {
        GameManager.Instance.OnGameRestarted.AddListener(Init);
        
        mainCamera = Camera.main;
        CalculateScreenBounds();
        Init();
    }

    private IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(initialDelay);
        
        int remainingEnemies = totalEnemies;
        
        while(remainingEnemies > 0)
        {
            int enemiesToSpawn = Mathf.Min(enemiesPerGroup, remainingEnemies);
            
            for(int i = 0; i < enemiesToSpawn; i++)
            {
                SpawnSingleEnemy();
                remainingEnemies--;
                yield return new WaitForSeconds(0.3f);
            }
            
            yield return new WaitForSeconds(spawnInterval);
        }

        GameManager.Instance.LevelCompleted();
    }

    private void SpawnSingleEnemy()
    {
        if(enemyPrefabs.Length == 0)
        {
            Debug.LogWarning("Нет префабов врагов!");
            return;
        }

        GameObject randomPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        
        Vector3 spawnPosition = new Vector3(
            Random.Range(minX, maxX),
            transform.position.y,
            0
        );

        var enemy = Instantiate(randomPrefab, spawnPosition, Quaternion.identity);
        Destroy(enemy, EnemyLifetime);

    }

    private void CalculateScreenBounds()
    {
        if(mainCamera.orthographic)
        {
            float vertExtent = mainCamera.orthographicSize;
            float horzExtent = vertExtent * mainCamera.aspect;
            
            minX = -horzExtent + spawnAreaOffset;
            maxX = horzExtent - spawnAreaOffset;
            minZ = -vertExtent + spawnAreaOffset;
            maxZ = vertExtent - spawnAreaOffset;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(
            (maxX - minX),
            0.1f,
            (maxZ - minZ)
        ));
    }
}