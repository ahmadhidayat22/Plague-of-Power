using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Unity.VisualScripting;

public class GunShoot : MonoBehaviour
{
    public Animator animator;
    public string animationWeapon;
    private Player_controls controls;
    public Transform ShootPoint;
    public GameObject ShootFlash;
    protected SpriteRenderer gunSpriteRenderer;
    public Sprite gunSprite;
    public bool hasGunInfinityAmmo;
    public int gunDamage;
    AudioManager audioManager;
    Vector2 direction;
    private float angle;

    public GameObject bulletPrefab;
    public float bulletSpeed;
    public float fireRate;
    private float nextFireTime = 0f;

    private CinemachineShake cinemachineShake;


    [Header("Ammo Settings")]
    public int maxAmmo;
    public int currentAmmo;
    public int totalAmmo;
    public float reloadTime = 2f;
    private bool isReloading = false;

    public string ammoText;
    public float GetGunAngle() => angle;

    [Header("recoil")]
    private Vector3 originalLocalPosition;
    public float recoilDistance = 0.08f;
    public float recoilDuration = 0.05f;
    public WeaponManager weaponManager;

    private void Awake()
    {

        // weaponManager = GetComponent<WeaponManager>();
        gunSpriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        controls = new Player_controls();
        // controls.Combat.Shoot.performed += ctx => OnShoot();
        controls.Combat.Reload.performed += ctx => TryReload();
        GameObject audioObj = GameObject.FindGameObjectWithTag("Audio");
        if (audioObj == null)
        {
            Debug.LogError("AudioManager object with tag 'Audio' not found!");
            return;
        }

        audioManager = audioObj.GetComponent<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogError("AudioManager component not found on the tagged object!");
        }



        originalLocalPosition = transform.parent.localPosition;
        gunSpriteRenderer.sprite = gunSprite;

    }


    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    private void Start()
    {
        currentAmmo = maxAmmo;

        ammoText = currentAmmo + " / " + totalAmmo;
        if (CinemachineShake.Instance != null)
        {
            cinemachineShake = CinemachineShake.Instance;
        }
        else
        {
            Debug.LogWarning("Cinemachine Shake instance not found!");
        }
        // ammoText = currentAmmo + " / " + totalAmmo;
    }
    public void setCurrentAmmo()
    {
        currentAmmo = maxAmmo;

    }

    private void Update()
    {
        gunSpriteRenderer.sprite = gunSprite;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // if (isReloading) return;
        if (controls.Combat.Shoot.IsPressed())
        {
            OnShoot();
        }

        ammoText = currentAmmo + " / " + totalAmmo;

        direction = mousePos - (Vector2)transform.position;
        float distanceToMouse = Vector2.Distance(transform.position, mousePos);

        if (distanceToMouse >= 1f)
        {
            angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            FaceMouse();
        }
    }



    void FaceMouse()
    {
        Vector3 scale = transform.root.localScale; // root = Player object

        if (scale.x < 0)
        {
            transform.right = new Vector2(-direction.x, -direction.y);
        }
        else
        {
            transform.right = direction;
        }
    }

    void OnShoot()
    {

        if (Time.time < nextFireTime || isReloading || ShootPoint == null || bulletPrefab == null || ShootFlash == null)
            return;

        if (currentAmmo <= 0)
        {
            Debug.Log("Out of ammo! Reload needed.");
            TryReload();
            return;
        }

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 shootDir = (mousePos - (Vector2)ShootPoint.position).normalized;

        GameObject bullet = Instantiate(bulletPrefab, ShootPoint.position, Quaternion.identity);
        if (bullet != null)
        {
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.damage = gunDamage;
            }
            Rigidbody2D rbBullet = bullet.GetComponent<Rigidbody2D>();
            float angle = Mathf.Atan2(shootDir.y, shootDir.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.Euler(0f, 0f, angle);
            audioManager.PlaySFX(audioManager.shoot);

            StartCoroutine(PlayRecoil());

            // transform.position = new Vector3(transform.position.x - 0.08f, transform.position.y, transform.position.z);

            if (rbBullet != null)
            {
                rbBullet.linearVelocity = shootDir * bulletSpeed;
            }
            currentAmmo--;
            weaponManager.weaponCurrentAmmos[weaponManager.currentWeaponIndex] = currentAmmo;

            StartCoroutine(FlashEffect());

            if (cinemachineShake != null)
            {
                cinemachineShake.ShakeCamera(2.5f, 0.1f);
            }

            nextFireTime = Time.time + fireRate;
        }
        transform.position = new Vector3(transform.position.x + 0.08f, transform.position.y, transform.position.z);

    }
    // TODO: Buat recoil berdasarkan angle senjata misal senjata mengarah kebawah maka arah recoil seharusnya keatas.

    private IEnumerator PlayRecoil()
    {
        transform.parent.localPosition = originalLocalPosition + new Vector3(-recoilDistance, 0, 0);

        // Vector3 scale = transform.root.localScale;
        // // Pindahkan senjata ke belakang (recoil)
        // Debug.Log(scale);
        // if (scale.x < 0)
        // {
        //     transform.parent.localPosition = originalLocalPosition + new Vector3(-recoilDistance, 0, 0);

        // }
        // else
        // {
        //     transform.parent.localPosition = originalLocalPosition + new Vector3(-recoilDistance, 0, 0);

        // }
        //Debug.Log(transform.localPosition);

        yield return new WaitForSeconds(recoilDuration);

        // Kembalikan ke posisi semula
        transform.parent.localPosition = originalLocalPosition;
    }
    private IEnumerator FlashEffect()
    {
        ShootFlash.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        ShootFlash.SetActive(false);
    }

    void TryReload()
    {
        if (!isReloading && currentAmmo < maxAmmo && totalAmmo > 0)
        {
            StartCoroutine(Reload());
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");
        // animator.SetTrigger("Reload"); // uncomment if you have reload animation
        audioManager.PlaySFX(audioManager.reload);

        yield return new WaitForSeconds(reloadTime);

        int ammoToReload = Mathf.Min(maxAmmo - currentAmmo, totalAmmo);
        currentAmmo += ammoToReload;
        if (!hasGunInfinityAmmo)
        {
            totalAmmo -= ammoToReload;
            weaponManager.weaponTotalAmmos[weaponManager.currentWeaponIndex] = totalAmmo;

        }

        Debug.Log("Reload complete: " + currentAmmo + " / " + totalAmmo);
        isReloading = false;
    }
}
