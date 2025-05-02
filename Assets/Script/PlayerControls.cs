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
    [SerializeField] private PlayerAimWeapon playerAimWeapon;
    float angle;

    private void Awake()
    {
        playerControls = new Player_controls();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnEnable()
    {
        playerControls.Enable();
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
        // FIXME: bug ketika jalan kek kanan atas dan aiming, senjata berubah arah
        
        if (movement.x > 0 || (angle < 75 && angle > -75))
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
        
        else if (movement.x < 0 || ( angle > 100 || angle < -100 && angle > -180 ) )
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

    void FlipOnRotateMouse()
    {
       
    }

    void ChangeAnimationState(string newAnimation, bool forcePlay = false)
    {
        if (!forcePlay && currentAnimaton == newAnimation) return;

        animator.Play(newAnimation);
        currentAnimaton = newAnimation;
    }

}
