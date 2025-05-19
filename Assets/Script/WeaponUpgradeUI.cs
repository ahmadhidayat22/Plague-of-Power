using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class WeaponUpgradeUI : MonoBehaviour
{
    public WeaponManager weaponManager;
    public Transform weaponListParent; // Drag "Overflow Scroll" here
    public GameObject weaponItemPrefab; // Prefab UI dari 1 item (Weapon1, Weapon2, dst.)
    public GameObject weaponUpgradePanel;

    void Start()
    {
        CreateWeaponUI();
    }
    // TODO: mainkan animasi ketika panel upgrade dibuka / ditutup
    public void OpenUpgradeUI()
    {
        Time.timeScale = 0f;
        weaponUpgradePanel.SetActive(true);
    }
    
    public void CloseUpgradeUI()
    {
        Time.timeScale = 1f;
        weaponUpgradePanel.SetActive(false);
    }

    void CreateWeaponUI()
    {
        
        for (int i = 0; i < weaponManager.allWeapons.Length; i++)
        {
            int weaponIndex = i;
            Weapon weapon = weaponManager.allWeapons[i];

            GameObject item = Instantiate(weaponItemPrefab, weaponListParent);
            // Atur posisi dengan RectTransform
           
            // Set Image
            Image weaponImage = item.transform.Find("Image").GetComponent<Image>();
            if (weapon.UIWeaponSprite != null)
                weaponImage.sprite = weapon.UIWeaponSprite;

            // Set Stats
            TextMeshProUGUI stat1 = item.transform.Find("Stats/Stats1").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI stat2 = item.transform.Find("Stats/Stats2").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI stat3 = item.transform.Find("Stats/Stats3").GetComponent<TextMeshProUGUI>();

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
            }
           if (upgradeData.IsMaxLevel(currentLevel))
            {
                upgradeBtn.interactable = false;
                
            }
            // Upgrade Button
            upgradeBtn.onClick.AddListener(() =>
            {
                weaponManager.UpgradeWeapon(weaponIndex);
                RefreshUI(); // update tampilan
            });

            // Sesuaikan tinggi kontainer scroll view berdasarkan jumlah item

        }
        //  RectTransform contentRT = weaponListParent.GetComponent<RectTransform>();
        // float totalHeight = weaponManager.allWeapons.Length * (itemHeight + spacing);
        // contentRT.sizeDelta = new Vector2(contentRT.sizeDelta.x, totalHeight);
    }

    void RefreshUI()
    {
        // Bersihkan & reload (sederhana)
        foreach (Transform child in weaponListParent)
        {
            Destroy(child.gameObject);
        }
        CreateWeaponUI();
    }
}
