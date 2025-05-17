using System.Collections;
using UnityEngine;

public class PlayerAimWeapon : MonoBehaviour
{
    public Animator animator;
    public string currentAnimaton;
    private Player_controls controls;
    public Transform Gun;
    public Transform ShootPoint;
    public GameObject ShootFlash;

    AudioManager audioManager;

    Vector2 direction;
    private float angle;
    // public Animator GunAnimator;
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;
    public float fireRate; // waktu antar tembakan (dalam detik)
    private float nextFireTime = 0f;

    private CinemachineShake cinemachineShake;

    [Header("Ammo Settings")]
    public int maxAmmo = 10;
    public int currentAmmo;
    public int totalAmmo = 30;
    public float reloadTime = 2f;
    private bool isReloading = false;

    public string ammoText;
    public float GetGunAngle() => angle;

    private void Awake()
    {
        animator = Gun.GetComponent<Animator>();
        controls = new Player_controls();
        controls.Combat.Shoot.performed += ctx => OnShoot();  // bind action
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

        controls.Combat.Reload.performed += ctx => TryReload(); // Tambahkan ini
        if (CinemachineShake.Instance != null)
        {
            cinemachineShake = CinemachineShake.Instance;
        }
        else
        {
            Debug.LogWarning("Cinemachine Shake instance not found!");
        }
    }
    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();


     private void Start()
    {
        currentAmmo = maxAmmo;
        ammoText = currentAmmo + " / " + totalAmmo;
    }
    private void Update()
    {
        // if (isReloading) return;
        ammoText = currentAmmo + " / " + totalAmmo;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        direction = mousePos - (Vector2)Gun.position;
        //   angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //   FaceMouse();
        float distanceToMouse = Vector2.Distance(Gun.position, mousePos);
        // Debug.Log("Distance to mouse: " + distanceToMouse);

        if (distanceToMouse >= 1f)
        {
            angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            FaceMouse();
        }

    }

    void FaceMouse()
    {
        Vector3 scale = transform.localScale;

        if (scale.x < 0)
        {
            // Player menghadap kiri → balik arah horizontal
            Gun.transform.right = new Vector2(-direction.x, -direction.y);
        }
        else
        {
            // Player menghadap kanan
            Gun.transform.right = direction;
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
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            float angle = Mathf.Atan2(shootDir.y, shootDir.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.Euler(0f, 0f, angle);
            animator.Play("Shoot");
            audioManager.PlaySFX(audioManager.shoot);

            if (rb != null)
            {
                rb.linearVelocity = shootDir * bulletSpeed;
            }
            currentAmmo--;
            StartCoroutine(FlashEffect());
            if (cinemachineShake != null)
            {
                cinemachineShake.ShakeCamera(3f, .1f);
            }
            nextFireTime = Time.time + fireRate;
            // animator.SetBool("isShoot", false);

        }
    }

    private IEnumerator FlashEffect()
    {
        ShootFlash.SetActive(true);
        yield return new WaitForSeconds(0.05f); // tampil selama 0.05 detik
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
        // animator.SetTrigger("Reload"); // jika kamu punya animasi reload
        audioManager.PlaySFX(audioManager.reload);

        yield return new WaitForSeconds(reloadTime);

        int ammoToReload = Mathf.Min(maxAmmo - currentAmmo, totalAmmo);
        currentAmmo += ammoToReload;
        totalAmmo -= ammoToReload;

        Debug.Log("Reload complete: " + currentAmmo + " / " + totalAmmo);
        isReloading = false;
    }

}