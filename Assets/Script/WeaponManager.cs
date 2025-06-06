using UnityEngine;

public class WeaponManager : MonoBehaviour
{

    public Weapon[] allWeapons;
    public UpgradeWeaponData[] weaponUpgradeDatas;
    public int[] weaponLevels;
    public int currentWeaponIndex;
    public int SetCurrentWeaponIndex{set{ currentWeaponIndex = value; }}
    public Weapon CurrentWeapon { get; private set; }
    public static WeaponManager Instance { get; private set; }
    // public PlayerAimWeapon aimWeapon; 
    public GunShoot gunShoot; 
    public SpriteRenderer gunSpriteRenderer;
    private Player_controls controls;
      
    public int[] weaponCurrentAmmos;
    public int[] weaponTotalAmmos;

    // private void Start()
    // {

    // }
    void Awake()
    {
        weaponLevels = new int[allWeapons.Length];
        weaponCurrentAmmos = new int[allWeapons.Length];
        weaponTotalAmmos = new int[allWeapons.Length];

        for (int i = 0; i < weaponLevels.Length; i++)
        {
            weaponLevels[i] = 1; // default semua level senjata = 1
            weaponCurrentAmmos[i] = allWeapons[i].maxAmmo;
            weaponTotalAmmos[i] = allWeapons[i].totalAmmo;
        }

        EquipWeapon(currentWeaponIndex);
        
        
    }
    public void EquipWeapon(int index)
    {
        if (index < 0 || index >= allWeapons.Length) return;

        CurrentWeapon = allWeapons[index];
        int currentLevel = weaponLevels[index];
        if(CurrentWeapon.isShouldBuy && CurrentWeapon.isLock) return;

         // Cari UpgradeData sesuai nama senjata
        UpgradeWeaponData upgradeData = GetUpgradeData(CurrentWeapon.weaponName);
        var levelData = upgradeData?.GetUpgradeData(currentLevel);
        gunShoot.animationWeapon = CurrentWeapon.animationShootName;
        gunShoot.bulletPrefab = CurrentWeapon.bulletPrefab;
        gunShoot.fireRate = (float)(CurrentWeapon.fireRate * 1f - (levelData?.fireRatePercent / 100f));
        gunShoot.maxAmmo = CurrentWeapon.maxAmmo + (levelData?.extraMaxAmmo ?? 0);
        // gunShoot.totalAmmo = CurrentWeapon.totalAmmo ;
        gunShoot.bulletSpeed = CurrentWeapon.bulletSpeed * (levelData?.bulletSpeedMultiplier ?? 1f);
        // gunShoot.currentAmmo = CurrentWeapon.maxAmmo;
        // gunShoot.currentAmmo = gunShoot.maxAmmo;
        gunShoot.currentAmmo = weaponCurrentAmmos[index]; // ambil ammo terakhir yang tersisa
        gunShoot.totalAmmo = weaponTotalAmmos[index] ;

        gunShoot.gunSprite = CurrentWeapon.weaponSprite;
        gunShoot.hasGunInfinityAmmo = CurrentWeapon.hasInfinityAmmo;
        gunShoot.gunDamage = CurrentWeapon.damage + (levelData?.extraDamage ?? 0);
        gunShoot.shootSFX = CurrentWeapon.shootSFX;
        gunShoot.reloadSFX = CurrentWeapon.reloadSFX;

        // Debug.Log(CurrentWeapon.weaponSprite);

        if (CurrentWeapon.animatorController != null)
        {
            gunShoot.animator.runtimeAnimatorController = CurrentWeapon.animatorController;
        }
    }
    public void UpgradeWeapon(int index)
    {
        if (index < 0 || index >= weaponLevels.Length) return;
      
        weaponLevels[index]++;
        if (index == currentWeaponIndex)
        {
            EquipWeapon(index);
        }
    }
    public UpgradeWeaponData GetUpgradeData(string weaponName)
    {
        foreach (var data in weaponUpgradeDatas)
        {
            if (data.weaponName == weaponName)
                return data;
        }
        return null;
    }
    public void UpgradeCurrentWeapon(float fireRatePercent = 1f, int extraAmmo = 0, float bulletSpeedMultiplier = 1f)
    {
       
        if (CurrentWeapon == null) return;
        
        gunShoot.fireRate *= 1f - (fireRatePercent / 100f);
        gunShoot.bulletSpeed *= bulletSpeedMultiplier;
        // Debug.Log(gunShoot.fireRate);

        // gunShoot.maxAmmo += extraAmmo;
        // gunShoot.currentAmmo = gunShoot.maxAmmo;

        
        // gunShoot.totalAmmo += extraAmmo * 3; 
    }


    public void BuyWeapon(int index)
    {
        if (index < 0 || index >= weaponLevels.Length) return;
        CurrentWeapon = allWeapons[index];
        CurrentWeapon.isShouldBuy = false;

    }
    public void NextWeapon()
    {
        // Debug.Log("change weapon");
        for (int i = 1; i < allWeapons.Length; i++)
        {
            // currentWeaponIndex = (currentWeaponIndex + 1) % allWeapons.Length;
            int checkIndex = (currentWeaponIndex + i) % allWeapons.Length;

            if (!allWeapons[checkIndex].isLock && !allWeapons[checkIndex].isShouldBuy)
            {
                currentWeaponIndex = checkIndex;
                break;
            }
        }
        EquipWeapon(currentWeaponIndex);
    }
}
