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
    const string PLAYER_WALK= "Player_walk";
    const string PLAYER_WALK_BACK = "Player_walk_backwards";

    bool isFacingDown = true;

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

    }

    private void FixedUpdate()
    {
        Move();
    }


    private void PlayerInput()
    {
        movement = playerControls.Movement.Move.ReadValue<Vector2>();
        // FIXME: Fix this
        if(movement.y > 0)
        {
            isFacingDown = false;
        }else if(movement.y < 0){
            isFacingDown = true;
        }

    }

    private void Move()
    {
        rb.MovePosition(rb.position + movement * (MoveSpeed * Time.fixedDeltaTime));
        // Debug.Log("X:"+ movement.x);
        // Debug.Log("Y:"+ movement.y);
        
        
        // FIXME: Fix this
        

        if (movement.x != 0 || movement.y != 0)
        {
            if(isFacingDown )
            {
                ChangeAnimationState(PLAYER_WALK);

            }else{
                ChangeAnimationState(PLAYER_WALK_BACK);

            }
        }
        else{
            if(isFacingDown )
            {
            ChangeAnimationState(PLAYER_IDLE);

            }else{
                ChangeAnimationState(PLAYER_IDLE_BACK);
            }
        }
    }

    void FlipSprite()
    {
        if (movement.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (movement.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }
    void ChangeAnimationState(string newAnimation, bool forcePlay = false)
    {
        if (!forcePlay && currentAnimaton == newAnimation) return;

        animator.Play(newAnimation);
        currentAnimaton = newAnimation;
    }

}
