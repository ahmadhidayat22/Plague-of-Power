using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GUIPlayerUpdater : MonoBehaviour
{
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private TMP_Text gemsText;
    [SerializeField] private Player player; // referensi ke script Player
    [SerializeField] private GunShoot playerWeapon;
    [SerializeField] private TMP_Text ammoText;
    [SerializeField] private TMP_Text medkitText;
    [SerializeField] private TMP_Text HealthText;

    [SerializeField] private GameObject healImage;
    [SerializeField] private GameObject weaponImage;
    [SerializeField] private GameObject changeWeaponPanel;
    public GunShoot gunShoot;
    private float cooldownTime = 5f;
    private float cooldownTime2 = 2f;
    private float cooldownTimer = 0f;
    private float cooldownTimer2 = 0f;
    private bool isCooldownHeal = false;
    private bool isCooldownChangeWeapon = false;
    private WeaponManager weaponManager;
    [SerializeField] private GameObject weaponIconSkill;

    void Start()
    {
        weaponManager = GameObject.FindAnyObjectByType<WeaponManager>();
        healImage.GetComponent<Image>();
        weaponImage.GetComponent<Image>();
        changeWeaponPanel.SetActive(false);
        
    }
    private void Update()
    {
        int countsAllWeaponIsBuy = 0;
        for (int i = 0; i < weaponManager.allWeapons.Length; i++)
        {
            // Debug.Log(weaponManager.allWeapons[i].isShouldBuy);
            if (!weaponManager.allWeapons[i].isShouldBuy)
            {
                // changeWeaponPanel.SetActive(false);
                countsAllWeaponIsBuy++;

            }

        }

        if (countsAllWeaponIsBuy > 1)
        {
            changeWeaponPanel.SetActive(true);
        }

        // Debug.Log(gunShoot.gunSprite);
        if (player != null)
        {
            // coinText.text =  PlayerPrefs.GetInt("Coin").ToString();
            coinText.text = player.GetCoin.ToString();
            gemsText.text = player.GetGems.ToString();
            ammoText.text = playerWeapon.ammoText.ToString();
            medkitText.text = player.GetMedkit.ToString();
            HealthText.text = player.getCurrentHealth.ToString() + " / " + player.getMaxHealth.ToString();
        }

        if (isCooldownHeal)
        {
            cooldownTimer -= Time.deltaTime;
            healImage.GetComponent<Image>().fillAmount = cooldownTimer / cooldownTime;

            if (cooldownTimer <= 0f)
            {
                isCooldownHeal = false;
                healImage.GetComponent<Image>().fillAmount = 0f;
            }
        }
        if (isCooldownChangeWeapon)
        {
            cooldownTimer2 -= Time.deltaTime;
            weaponImage.GetComponent<Image>().fillAmount = cooldownTimer2 / cooldownTime2;

            if (cooldownTimer2 <= 0f)
            {
                isCooldownChangeWeapon = false;
                weaponImage.GetComponent<Image>().fillAmount = 0f;
            }
        }
        displayNextChangeWeapon();
    }
    public Weapon nextWeapon { get; private set; }
    void displayNextChangeWeapon()
    {
        int nextIndex = weaponManager.currentWeaponIndex + 1;
        if (nextIndex >= weaponManager.allWeapons.Length)
        {
            nextIndex = 0;
        }
        nextWeapon = weaponManager.allWeapons[nextIndex];
        // Debug.Log(nextWeapon);
        weaponIconSkill.GetComponent<Image>().sprite = nextWeapon.UIWeaponSprite;
    }

    public void useHeal(float CDTime)
    {
        if (!isCooldownHeal)
        {
            // Aktifkan skill
            cooldownTime = CDTime;
            cooldownTimer = cooldownTime;
            isCooldownHeal = true;
            healImage.GetComponent<Image>().fillAmount = 1f;
        }
    }

    public void useWeapon(float CDTime)
    {
        if (!isCooldownChangeWeapon)
        {
            // Aktifkan skill
            cooldownTime = CDTime;
            cooldownTimer2 = cooldownTime;
            isCooldownChangeWeapon = true;
            weaponImage.GetComponent<Image>().fillAmount = 1f;
        }
    }
}
