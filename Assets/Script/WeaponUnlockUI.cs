using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WeaponUnlockUI : MonoBehaviour
{
    public Weapon[] allWeapons;
    public Transform weaponListParent;
    public GameObject weaponItemPrefab;
    public TextMeshProUGUI gemsText;
    void Start()
    {
        CreateWeaponUI();
    }


    void CreateWeaponUI()
    {
        int coinPlayer = PlayerPrefs.GetInt("Coin");
        int gemsPlayer = PlayerPrefs.GetInt("Gems");
        for (int i = 0; i < allWeapons.Length; i++)
        {
            Weapon weapon = allWeapons[i];
            GameObject item = Instantiate(weaponItemPrefab, weaponListParent);
            item.SetActive(true);

            Image weaponImage = item.transform.Find("Image").GetComponent<Image>();
            if (weapon.UIWeaponSprite != null)
                weaponImage.sprite = weapon.UIWeaponSprite;


            // Set Stats
            TextMeshProUGUI stat1 = item.transform.Find("Stats/Stats1").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI stat2 = item.transform.Find("Stats/Stats2").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI stat3 = item.transform.Find("Stats/Stats3").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI stat4 = item.transform.Find("Stats/Stats4").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI costUpgrade = item.transform.Find("btnUnlock/costUnlock").GetComponent<TextMeshProUGUI>();
            Button unlockButton = item.transform.Find("btnUnlock").GetComponent<Button>();
            GameObject panel = item.transform.Find("Panel").gameObject;

            if (weapon.isLock)
            {
                unlockButton.gameObject.SetActive(true);
                panel.SetActive(true);
            }
            else
            {
                unlockButton.gameObject.SetActive(false);
                panel.SetActive(false);

            }

            stat1.text = $"{weapon.damage} ";
            stat2.text = $"{weapon.fireRate} "; // FIXME: nilai fire rate yang ditampilkan agar human readable
            stat3.text = $"{weapon.maxAmmo} ";
            stat4.text = $"{weapon.bulletSpeed} ";
            costUpgrade.text = $"{weapon.costUnlock}";

            unlockButton.onClick.AddListener(() =>
            {
                gemsPlayer -= weapon.costBuy;
                PlayerPrefs.SetInt("Gems", gemsPlayer);
                gemsText.text = gemsPlayer.ToString();
                weapon.isLock = false;
                // weaponManager.BuyWeapon(weaponIndex);
                RefreshUI();
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
