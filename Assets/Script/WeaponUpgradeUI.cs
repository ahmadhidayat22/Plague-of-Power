using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class WeaponUpgradeUI : MonoBehaviour
{
    public WeaponManager weaponManager;
    public Transform weaponListParent; 
    public GameObject weaponItemPrefab; 
    public GameObject weaponUpgradePanel;
    public CursorManager cursorManager;
    public TextMeshProUGUI coinText;

    public Player player;

    void Start()
    {
        player = GetComponent<Player>();

        CreateWeaponUI();
    }
    // TODO: mainkan animasi ketika panel upgrade dibuka / ditutup
    public void OpenUpgradeUI()
    {
        Time.timeScale = 0f;
        weaponUpgradePanel.SetActive(true);
        cursorManager.SetDefaultCursor();
    }

    public void CloseUpgradeUI()
    {
        Time.timeScale = 1f;
        weaponUpgradePanel.SetActive(false);
        cursorManager.setCustomCursor();
    }

    void CreateWeaponUI()
    {
        int coinPlayer = PlayerPrefs.GetInt("Coin");
        coinText.text = coinPlayer.ToString();
        for (int i = 0; i < weaponManager.allWeapons.Length; i++)
        {
            
            int weaponIndex = i;
            Weapon weapon = weaponManager.allWeapons[i];

            GameObject item = Instantiate(weaponItemPrefab, weaponListParent);
            item.SetActive(true);
            

            // Set Image
            Image weaponImage = item.transform.Find("Image").GetComponent<Image>();
            if (weapon.UIWeaponSprite != null)
                weaponImage.sprite = weapon.UIWeaponSprite;

            // Set Stats
            TextMeshProUGUI stat1 = item.transform.Find("Stats/Stats1").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI stat2 = item.transform.Find("Stats/Stats2").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI stat3 = item.transform.Find("Stats/Stats3").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI stat4 = item.transform.Find("Stats/Stats4").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI costUpgrade = item.transform.Find("btnUpgrade/CoinPanel/CoinText").GetComponent<TextMeshProUGUI>();

            UpgradeWeaponData upgradeData = weaponManager.GetUpgradeData(weapon.weaponName);
            int currentLevel = weaponManager.weaponLevels[weaponIndex];
            //Debug.Log(weaponManager.GetUpgradeData(weapon.weaponName));
            var levelData = upgradeData.GetUpgradeData(currentLevel);

            Button upgradeBtn = item.transform.Find("btnUpgrade").GetComponent<Button>();
            if (levelData != null)
            {
                stat1.text = $"{levelData.fireRatePercent}% +";
                stat2.text = $"{levelData.extraDamage} +";
                stat3.text = $"{levelData.extraMaxAmmo} +";
                stat4.text = $"{levelData.bulletSpeedMultiplier} +";
                costUpgrade.text = $"{levelData.costCoin}";
            }
           if (upgradeData.IsMaxLevel(currentLevel) || levelData.costCoin > coinPlayer)
            {
                upgradeBtn.interactable = false;
                
            }
            // Upgrade Button
            upgradeBtn.onClick.AddListener(() =>
            {
                coinPlayer -= levelData.costCoin;
                PlayerPrefs.SetInt("Coin", coinPlayer);
                player.GetCoin = coinPlayer;
                weaponManager.UpgradeWeapon(weaponIndex);
                RefreshUI(); // update tampilan
            });

            

        }
        
    }

    void RefreshUI()
    {
       
        foreach (Transform child in weaponListParent)
        {
            Destroy(child.gameObject);
        }
        CreateWeaponUI();
    }
}
