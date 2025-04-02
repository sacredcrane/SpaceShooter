using UnityEngine;

public class RandomSkyboxSelector : MonoBehaviour
{
    [Header("Skybox Materials")]
    [SerializeField] private Material[] _skyboxMaterials;

    private void Start()
    {
        if (_skyboxMaterials == null || _skyboxMaterials.Length == 0)
        {
            Debug.LogError("No skybox materials assigned!");
            return;
        }

        int randomIndex = Random.Range(0, _skyboxMaterials.Length);
        Material selectedMaterial = _skyboxMaterials[randomIndex];

        RenderSettings.skybox = selectedMaterial;

        DynamicGI.UpdateEnvironment();
    }
}