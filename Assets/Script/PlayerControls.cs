using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public class Player : MonoBehaviour
{
    [SerializeField] float MoveSpeed = 1f;

    private Player_controls playerControls;
    private Vector2 movement;
    private Rigidbody2D rb;

    public string currentAnimaton;
    private Animator animator;

    const string PLAYER_IDLE = "Player_idle";
    const string PLAYER_IDLE_BACK = "Player_idle_backwards";
    const string PLAYER_WALK = "Player_walk";
    const string PLAYER_WALK_BACK = "Player_walk_backwards";

    bool isFacingDown = true;
    [SerializeField] private Transform gunHolder;
    // [SerializeField] private PlayerAimWeapon playerAimWeapon;
    private WeaponManager weaponManager;
    [SerializeField] private GunShoot playerAimWeapon;
    float angle;

    [Header("Health Player")]
    private float maxHealth = 100f;
    private float currentHealth;
    [SerializeField] FloatingHealthBar healthBar;
    Collider2D _lastPointTouched;
    private int coinCounter = 0;
    private int gemsCounter = 0;
    public int medkitCounter = 0;
    private float healAmountFromMedkit = 30f;
    public float healCooldown = 5f;
    private float nextHealTime = 0f;
    [SerializeField] private GameObject healEffectPrefab; // Drag prefab FX Heal ke sini lewat Inspector


    public int GetCoin { get { return coinCounter; } set { coinCounter = value; } }
    public int GetGems { get { return gemsCounter; } set { gemsCounter = value; } }
    public int GetMedkit { get { return medkitCounter; } }
    public float getCurrentHealth { get { return currentHealth; } }
    public float getMaxHealth { get { return maxHealth; } }

    public float setMaxhealth { set { maxHealth = value; } }
    public InGameUI inGameUI;

    [SerializeField] private GameOverManager gameOverManager;
    public GameObject GameOverMenu;
    public GUIPlayerUpdater gUIPlayerUpdater;
    private float nextChangeWeapon = 0f;
    private float changeWeaponCooldown = 2f;
    public GameObject weaponIconSkill;
    AudioManager audioManager;

    private void Awake()
    {
        weaponManager = GameObject.FindAnyObjectByType<WeaponManager>();
        healthBar = GetComponentInChildren<FloatingHealthBar>();
        playerControls = new Player_controls();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        healthBar.UpdateHealthBar(currentHealth, maxHealth);
        playerControls.Combat.Heal.performed += ctx => Heal(healAmountFromMedkit);
        playerControls.Combat.ChangeWeapon1.performed += ctx => changeWeapon();
        playerControls.Combat.upgrade.performed += ctx => weaponManager.UpgradeCurrentWeapon(5f);
        playerControls.Other.PauseMenu.performed += ctx => inGameUI.Action("Pause");
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

        coinCounter = PlayerPrefs.HasKey("Coin") ? PlayerPrefs.GetInt("Coin") : 0;
        gemsCounter = PlayerPrefs.HasKey("Gems") ? PlayerPrefs.GetInt("Gems") : 0;
        PlayerPrefs.SetInt("Coin", coinCounter);
        PlayerPrefs.SetInt("Gems", gemsCounter);
    }
    void changeWeapon()
    {
        if (Time.time < nextChangeWeapon)
            return;
        GUIPlayerUpdater.FindAnyObjectByType<GUIPlayerUpdater>().useWeapon(changeWeaponCooldown);
        weaponManager.NextWeapon();
        // weaponIconSkill.GetComponent<Image>().sprite = playerAimWeapon.gunSprite;
        nextChangeWeapon = Time.time + changeWeaponCooldown;
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerInput();
        FlipSprite();
        angle = playerAimWeapon.GetGunAngle();
        // Debug.Log(angle);
    }

    private void FixedUpdate()
    {
        Move();
    }


    private void PlayerInput()
    {
        movement = playerControls.Movement.Move.ReadValue<Vector2>();

        if (movement.y > 0)
        {
            isFacingDown = false;
        }
        else if (movement.y < 0)
        {
            isFacingDown = true;
        }

    }

    private void Move()
    {
        rb.MovePosition(rb.position + movement * (MoveSpeed * Time.fixedDeltaTime));

        bool isMoving = movement.x != 0 || movement.y != 0;

        if (isMoving)
        {
            // Putar langkah hanya jika belum diputar
            if (audioManager.GetSFXSource.clip != audioManager.walk || !audioManager.GetSFXSource.isPlaying)
            {
                audioManager.GetSFXSource.clip = audioManager.walk;
                audioManager.GetSFXSource.loop = true;
                audioManager.GetSFXSource.Play();
            }

            if (isFacingDown)
                ChangeAnimationState(PLAYER_WALK);
            else
                ChangeAnimationState(PLAYER_WALK_BACK);
        }
        else
        {
            // Hentikan langkah jika tidak bergerak
            if (audioManager.GetSFXSource.clip == audioManager.walk && audioManager.GetSFXSource.isPlaying)
            {
                audioManager.GetSFXSource.Stop();
                audioManager.GetSFXSource.clip = null; // kosongkan agar tidak nyangkut
            }

            if (isFacingDown)
                ChangeAnimationState(PLAYER_IDLE);
            else
                ChangeAnimationState(PLAYER_IDLE_BACK);
        }
    }


    void FlipSprite()
    {
        bool isAimingRight = angle > -75 && angle < 75;
        // Debug.Log("Movement x : "+movement.x+ "; angle :" + angle);
        if (isAimingRight)
        {
            if (angle > 45)
            {
                // ChangeAnimationState(PLAYER_IDLE_BACK);
                isFacingDown = false;
            }
            else
            {
                // ChangeAnimationState(PLAYER_IDLE);
                isFacingDown = true;
            }
            transform.localScale = new Vector3(1, 1, 1);
            gunHolder.localPosition = new Vector3(Mathf.Abs(gunHolder.localPosition.x), gunHolder.localPosition.y, 0);
        }

        else if (angle > 100 || (angle < -100 && angle > -180))
        {
            if (angle > 120)
            {
                // ChangeAnimationState(PLAYER_IDLE_BACK);
                isFacingDown = false;
            }
            else
            {
                // ChangeAnimationState(PLAYER_IDLE);
                isFacingDown = true;
            }
            transform.localScale = new Vector3(-1, 1, 1);
            gunHolder.localPosition = new Vector3(Mathf.Abs(gunHolder.localPosition.x), gunHolder.localPosition.y, 0);
        }
    }

    void ChangeAnimationState(string newAnimation, bool forcePlay = false)
    {
        if (!forcePlay && currentAnimaton == newAnimation) return;

        animator.Play(newAnimation);
        currentAnimaton = newAnimation;
    }

    public void Heal(float amount)
    {
        if (Time.time < nextHealTime)
            return;


        if (amount < 0)
        {
            throw new System.ArgumentOutOfRangeException("Cannot have negative healing");
        }

        bool wouldBeOverMaxHealth = currentHealth + amount > maxHealth;

        if (wouldBeOverMaxHealth)
        {
            currentHealth = maxHealth;
        }
        else
        {
            currentHealth += amount;
        }
        healthBar.UpdateHealthBar(currentHealth, maxHealth);
        GUIPlayerUpdater.FindAnyObjectByType<GUIPlayerUpdater>().useHeal(healCooldown);
        ShowHealEffect(); // Panggil FX Heal
        medkitCounter--;
        nextHealTime = Time.time + healCooldown;
    }
    private void ShowHealEffect()
    {
        if (healEffectPrefab != null)
        {
            Vector3 playerpos = new Vector3(transform.position.x , transform.position.y + .8f);
            GameObject fx = Instantiate(healEffectPrefab, playerpos, Quaternion.identity);
            fx.transform.SetParent(transform); // Opsional: biar ikut posisi player
            audioManager.PlaySFX(audioManager.heal);
            Destroy(fx, 1f); // Hancurkan efek setelah 2 detik (atau sesuai durasi animasi)
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log("I'm hit! HP: " + currentHealth);
        healthBar.UpdateHealthBar(currentHealth, maxHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        PlayerPrefs.SetInt("Coin", coinCounter);
        PlayerPrefs.SetInt("Gems", gemsCounter);

        float finalScore = gUIPlayerUpdater.GetSurvivalTime();
        gameOverManager.ShowGameOver(finalScore);

        GameOverMenu.SetActive(true);
        Destroy(gameObject);
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin") && collision.gameObject.activeSelf)
        {
            // collision.gameObject.SetActive(false);
            Destroy(collision.gameObject);
            coinCounter += Random.Range(4, 10);
        }
        if (collision.CompareTag("Gems") && collision.gameObject.activeSelf)
        {
            // collision.gameObject.SetActive(false);
            Destroy(collision.gameObject);
            gemsCounter += Random.Range(4, 10);
        }
        if (collision.CompareTag("Medkit") && collision.gameObject.activeSelf)
        {
            Destroy(collision.gameObject);

            // collision.gameObject.SetActive(false);
            medkitCounter += 1;
        }
    }


}
