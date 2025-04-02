using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] private float lifetime = 3f;
    
    private void OnEnable() => Invoke(nameof(Disable), lifetime);
    private void Disable() => gameObject.SetActive(false);
    private void OnDisable() => CancelInvoke();
}
