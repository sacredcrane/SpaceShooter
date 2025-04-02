using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float rotationSpeed = 200f;
    [SerializeField] private float maxTilt = 15f;

    [Header("Physics")]
    [SerializeField] private float drag = 3f;
    [SerializeField] private float angularDrag = 4f;

    [Header("Boundaries")]
    [SerializeField] private Vector2 mapSize = new Vector2(50f, 50f);
    [SerializeField] private float boundaryOffset = 1f;

    private Rigidbody rb;
    private Vector2 inputVector;
    private Quaternion initialRotation;
    private Camera mainCamera;

    // Input System
    private PlayerInput playerInput;
    private InputAction moveAction;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        initialRotation = transform.rotation;
        mainCamera = Camera.main;
        
        ConfigurePhysics();
        
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
    }

    private void ConfigurePhysics()
    {
        rb.linearDamping = drag;
        rb.angularDamping = angularDrag;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | 
                        RigidbodyConstraints.FreezeRotationZ;
    }

    private void Update()
    {
        inputVector = moveAction.ReadValue<Vector2>();
        ApplyVisualRotation();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        ClampPositionToBoundaries();
        StabilizeShip();
    }

    private void ApplyMovement()
    {
        Vector3 moveDirection = new Vector3(inputVector.x, inputVector.y, 0);
        rb.AddForce(moveDirection * moveSpeed, ForceMode.Acceleration);
    }

    private void ClampPositionToBoundaries()
    {
        Vector3 clampedPosition = transform.position;
        
        if (mainCamera.orthographic)
        {
            float vertExtent = mainCamera.orthographicSize;
            float horzExtent = vertExtent * mainCamera.aspect;
            
            clampedPosition.x = Mathf.Clamp(clampedPosition.x, 
                -horzExtent + boundaryOffset, 
                horzExtent - boundaryOffset);
            
            clampedPosition.y = Mathf.Clamp(clampedPosition.y, 
                -vertExtent + boundaryOffset, 
                vertExtent - boundaryOffset);
        }
        else 
        {
            clampedPosition.x = Mathf.Clamp(clampedPosition.x, 
                -mapSize.x + boundaryOffset, 
                mapSize.x - boundaryOffset);
            
            clampedPosition.y = Mathf.Clamp(clampedPosition.y, 
                -mapSize.y + boundaryOffset, 
                mapSize.y - boundaryOffset);
        }

        rb.position = clampedPosition;
    }

    private void ApplyVisualRotation()
    {
        if (inputVector.magnitude > 0.1f)
        {
            Vector3 tilt = new Vector3(
                initialRotation.eulerAngles.x + (inputVector.y * maxTilt),
                initialRotation.eulerAngles.y + (inputVector.x * maxTilt),
                initialRotation.eulerAngles.z
            );
            
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                Quaternion.Euler(tilt),
                rotationSpeed * Time.deltaTime
            );
        }
        else
        {
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                initialRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }

    private void StabilizeShip()
    {
        Vector3 angularVelocity = rb.angularVelocity;
        angularVelocity.x *= 0.95f;
        angularVelocity.z *= 0.95f;
        rb.angularVelocity = angularVelocity;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (mainCamera != null && mainCamera.orthographic)
        {
            float vertExtent = mainCamera.orthographicSize;
            float horzExtent = vertExtent * mainCamera.aspect;
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(
                horzExtent * 2 - boundaryOffset * 2,
                0.1f,
                vertExtent * 2 - boundaryOffset * 2
            ));
        }
        else
        {
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(
                mapSize.x * 2 - boundaryOffset * 2,
                0.1f,
                mapSize.y * 2 - boundaryOffset * 2
            ));
        }
    }
}