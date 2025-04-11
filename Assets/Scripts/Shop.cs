using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [Header("Weapon Upgrades")]
    [SerializeField] private Button upgradeButton;
    [SerializeField] private int upgradeCost;
    
    [Header("References")]
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private PlayerWeapon shootingSystem;

    private void Start()
    {
        shopPanel.SetActive(false);
        GameManager.Instance.OnShopOpened.AddListener(OpenShop);
        
        upgradeButton.onClick.AddListener(() => BuyUpgrade());
        GameManager.Instance.OnGameRestarted.AddListener(HideShop);
    }

    private void HideShop() 
    {
        shopPanel.SetActive(false);
    }

    private void OpenShop()
    {
        shopPanel.SetActive(true);
        UpdateButtons();
    }

    private void UpdateButtons()
    {
        bool canAfford = GameManager.Instance.CurrentScore >= upgradeCost;
        upgradeButton.interactable = canAfford;
    }

    public void BuyUpgrade()
    {
        if(GameManager.Instance.CurrentScore < upgradeCost) return;
        
        GameManager.Instance.AddScore(-upgradeCost);
        shootingSystem.UpgradeWeapon();
        UpdateButtons();
    }
}