using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float horizontalSpeed = 2f;
    
    private Camera mainCamera;
    private float minX;
    private float maxX;
    private Vector3 movementDirection;

    private void Start()
    {
        mainCamera = Camera.main;
        CalculateScreenBounds();
        movementDirection = new Vector3(
            Random.Range(-horizontalSpeed, horizontalSpeed), 
            -moveSpeed, 
            0
        );
    }

    private void CalculateScreenBounds()
    {
        float vertExtent = mainCamera.orthographicSize;
        float horzExtent = vertExtent * mainCamera.aspect;
        minX = -horzExtent + 1f;
        maxX = horzExtent - 1f;
    }

    private void Update()
    {
        transform.Translate(movementDirection * Time.deltaTime, Space.World);
        
        // Случайное изменение направления
        if(Random.value < 0.02f)
        {
            movementDirection.x = Random.Range(-horizontalSpeed, horizontalSpeed);
        }
        
        // Ограничение движения по X
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        transform.position = pos;
    }
}