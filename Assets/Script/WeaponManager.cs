using UnityEngine;

public class WeaponManager : MonoBehaviour
{

    public Weapon[] allWeapons;
    public int currentWeaponIndex;
    public int SetCurrentWeaponIndex{set{ currentWeaponIndex = value; }}
    public Weapon CurrentWeapon { get; private set; }
    public static WeaponManager Instance { get; private set; }

    // public PlayerAimWeapon aimWeapon; 
    public GunShoot gunShoot; 
    public SpriteRenderer gunSpriteRenderer;
    private Player_controls controls;
    // private void Start()
    // {
        
    // }
    void Awake()
    {
        EquipWeapon(currentWeaponIndex);
       
        
    }
    // void Update()
    // {
    //     // controls.Combat.Weapon2.performed += ctx => NextWeapon(2);
    //     // controls.Combat.Weapon1.performed += ctx => EquipWeapon(Random.Range(0,1));
    //     EquipWeapon(currentWeaponIndex);
    // }
    public void EquipWeapon(int index)
    {
        if (index < 0 || index >= allWeapons.Length) return;

        CurrentWeapon = allWeapons[index];
        gunShoot.animationWeapon = CurrentWeapon.animationShootName;
        gunShoot.bulletPrefab = CurrentWeapon.bulletPrefab;
        gunShoot.fireRate = CurrentWeapon.fireRate;
        gunShoot.maxAmmo = CurrentWeapon.maxAmmo;
        gunShoot.totalAmmo = CurrentWeapon.totalAmmo;
        gunShoot.bulletSpeed = CurrentWeapon.bulletSpeed;
        gunShoot.currentAmmo = CurrentWeapon.maxAmmo;
        gunShoot.gunSprite = CurrentWeapon.weaponSprite;
        gunShoot.hasGunInfinityAmmo = CurrentWeapon.hasInfinityAmmo;
        gunShoot.gunDamage = CurrentWeapon.damage;
        // Debug.Log(CurrentWeapon.weaponSprite);

        if (CurrentWeapon.animatorController != null)
        {
            gunShoot.animator.runtimeAnimatorController = CurrentWeapon.animatorController;
        }
        // if (CurrentWeapon.weaponSprite != null && gunSpriteRenderer != null)
        // {
        //     gunSpriteRenderer.sprite = CurrentWeapon.weaponSprite;
        // }
    }

    public void UpgradeCurrentWeapon(float fireRatePercent = 1f, int extraAmmo = 0, float bulletSpeedMultiplier = 1f)
    {
        // Pastikan ada senjata aktif
        if (CurrentWeapon == null) return;
        // Upgrade nilai di ScriptableObject (opsional tergantung preferensi)
        // Jika tidak ingin ubah SO langsung, buat duplikat SO runtime.

        // Upgrade pada GunShoot langsung
        gunShoot.fireRate *= 1f - (fireRatePercent / 100f);
        gunShoot.bulletSpeed *= bulletSpeedMultiplier;
        Debug.Log(gunShoot.fireRate);

        // gunShoot.maxAmmo += extraAmmo;
        // gunShoot.currentAmmo = gunShoot.maxAmmo;

        // Jika kamu tampilkan totalAmmo (persediaan cadangan), bisa di-upgrade juga
        // gunShoot.totalAmmo += extraAmmo * 3; // misal cadangan bertambah 3x ammo
    }


    public void NextWeapon()
    {
        // Debug.Log("change weapon");
        currentWeaponIndex = (currentWeaponIndex + 1) % allWeapons.Length;
        EquipWeapon(currentWeaponIndex);
    }
}
