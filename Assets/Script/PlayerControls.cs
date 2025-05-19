using UnityEngine;
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
    private float healAmountFromMedkit = 2f;
    private float healCooldown = 5f;
    private float nextHealTime = 0f;

    public int GetCoin { get { return coinCounter; } set { coinCounter = value; } }
    public int GetGems { get { return gemsCounter; }}
    public int GetMedkit { get { return medkitCounter; }}
    public float getCurrentHealth { get { return currentHealth; }}
    public float getMaxHealth { get { return maxHealth; }}

    public float setMaxhealth { set { maxHealth = value; } }
    

    public GameObject GameOverMenu;
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
        playerControls.Combat.ChangeWeapon1.performed += ctx => weaponManager.NextWeapon();
        playerControls.Combat.upgrade.performed += ctx => weaponManager.UpgradeCurrentWeapon(5f);

        coinCounter = PlayerPrefs.HasKey("Coin") ? PlayerPrefs.GetInt("Coin") : 0;
        gemsCounter = PlayerPrefs.HasKey("Gems") ? PlayerPrefs.GetInt("Gems") : 0;

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
        // Debug.Log("X:"+ movement.x);
        // Debug.Log("Y:"+ movement.y);

        if (movement.x != 0 || movement.y != 0)
        {
            if (isFacingDown)
            {
                ChangeAnimationState(PLAYER_WALK);

            }
            else
            {
                ChangeAnimationState(PLAYER_WALK_BACK);

            }
        }
        else
        {
            if (isFacingDown)
            {
                ChangeAnimationState(PLAYER_IDLE);

            }
            else
            {
                ChangeAnimationState(PLAYER_IDLE_BACK);
            }
        }
    }

    void FlipSprite()
    {
        bool isAimingRight = angle > -75 && angle < 75;
        // Debug.Log("Movement x : "+movement.x+ "; angle :" + angle);
        if (isAimingRight )
        {
            if(angle > 45 )
            {
                // ChangeAnimationState(PLAYER_IDLE_BACK);
                isFacingDown =false;
            }else{
                // ChangeAnimationState(PLAYER_IDLE);
                isFacingDown =true;
            }
            transform.localScale = new Vector3(1, 1, 1);
            gunHolder.localPosition = new Vector3(Mathf.Abs(gunHolder.localPosition.x), gunHolder.localPosition.y, 0);
        }
        
        else if (angle > 100 || (angle < -100 && angle > -180 ) )
        {
            if(angle > 120 )
            {
                // ChangeAnimationState(PLAYER_IDLE_BACK);
                isFacingDown =false;
            }
            else{
                // ChangeAnimationState(PLAYER_IDLE);
                isFacingDown =true;
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
        if( Time.time < nextHealTime )
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
        medkitCounter --;
        nextHealTime = Time.time + healCooldown;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log("I'm hit! HP: " + currentHealth);
        healthBar.UpdateHealthBar(currentHealth, maxHealth);
        if(currentHealth <= 0 )
        {
            Die();
        }
    }
    void Die()
    {
        // simpan state jumlah coin dan gems
        PlayerPrefs.SetInt("Coin", coinCounter);
        PlayerPrefs.SetInt("Gems", gemsCounter);
        // Tambahkan animasi atau efek di sini jika perlu
        GameOverMenu.SetActive(true);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    { 
        if(collision.CompareTag("Coin") && collision.gameObject.activeSelf )
        {
            // collision.gameObject.SetActive(false);
            Destroy(collision.gameObject);
            coinCounter += Random.Range(1, 4);
        }
        if(collision.CompareTag("Gems") && collision.gameObject.activeSelf )
        {
            // collision.gameObject.SetActive(false);
            Destroy(collision.gameObject);

            gemsCounter += Random.Range(1, 3);
        }
        if ( collision.CompareTag("Medkit") && collision.gameObject.activeSelf)
        {
            Destroy(collision.gameObject);
           
            // collision.gameObject.SetActive(false);
            medkitCounter += 1;
        }
    }


}
