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

    Vector2 direction;
    private float angle;
    // public Animator GunAnimator;
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;
    public float fireRate; // waktu antar tembakan (dalam detik)
    private float nextFireTime = 0f;

    private CinemachineShake cinemachineShake;

    public float GetGunAngle()
    {
        return angle;
    }


    private void Awake()
    {
        animator = Gun.GetComponent<Animator>();
        controls = new Player_controls();
        controls.Combat.Shoot.performed += ctx => OnShoot();  // bind action
        if (CinemachineShake.Instance != null)
        {
            cinemachineShake = CinemachineShake.Instance;
        }
        else
        {
            Debug.LogWarning("Cinemachine Shake instance not found!");
        }
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void Update()
    {
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
        if (Time.time < nextFireTime || ShootPoint == null || bulletPrefab == null || ShootFlash == null)
            return;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 shootDir = (mousePos - (Vector2)ShootPoint.position).normalized;

        GameObject bullet = Instantiate(bulletPrefab, ShootPoint.position, Quaternion.identity);
        if (bullet != null)
        {
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            float angle = Mathf.Atan2(shootDir.y, shootDir.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.Euler(0f, 0f, angle);
            animator.Play("Shoot");
            if (rb != null)
            {
                rb.linearVelocity = shootDir * bulletSpeed;
            }
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

}