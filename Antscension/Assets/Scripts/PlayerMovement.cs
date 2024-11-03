using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 8f;
    [SerializeField] float climbSpeed = 5f;
    // This is a bad implementation since if the scale of player is changed
    // we need to change this value os well
    int scale = 5;

    Vector2 moveInput;
    // The velocity property determines the movement
    Rigidbody2D playerRigidBody;
    Animator playerAnimator;
    CapsuleCollider2D playerCapsuleCollider;
    float gravityScaleAtStart;

    void Start()
    {
        // Initialization of variables
        playerRigidBody = GetComponent<Rigidbody2D>();       
        playerAnimator = GetComponent<Animator>();
        playerCapsuleCollider = GetComponent<CapsuleCollider2D>();
        gravityScaleAtStart = playerRigidBody.gravityScale;
    }

    void Update()
    {
       Run(); 
       FlipSprite();
       ClimbLadder();
    }

    void OnMove(InputValue value) 
    {
        // Get inputs from PlayerInput component
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value) 
    {
        // Prevents the player from jumping in the air
        if(!playerCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))  { return; }
        if(value.isPressed)
        {
            playerRigidBody.linearVelocity += new Vector2(0f, jumpSpeed);
        }
    }

    void Run() 
    {
        // Run based on the runSpeed, keep y the same 
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, playerRigidBody.linearVelocity.y);
        playerRigidBody.linearVelocity = playerVelocity;

        // Change animation to running if the player moves
        bool playerHasHorizontalSpeed = Mathf.Abs(playerRigidBody.linearVelocity.x) > Mathf.Epsilon; 
        playerAnimator.SetBool("IsRunning", playerHasHorizontalSpeed);
    }

    void FlipSprite() 
    {
        // If the player moves
        bool playerHasHorizontalSpeed = Mathf.Abs(playerRigidBody.linearVelocity.x) > Mathf.Epsilon; 

        if (playerHasHorizontalSpeed) 
        {
            transform.localScale = new Vector2(Mathf.Sign(playerRigidBody.linearVelocity.x) * scale, scale);
        }
    }

    void ClimbLadder()
    {
        if (!playerCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            playerRigidBody.gravityScale = gravityScaleAtStart;
            playerAnimator.SetBool("IsClimbing", false);
            return;
        }

        Vector2 climbVelocity = new Vector2(playerRigidBody.linearVelocity.x, moveInput.y * climbSpeed);
        playerRigidBody.linearVelocity = climbVelocity;
        playerRigidBody.gravityScale = 0f;

        bool playerHasVerticalSpeed = Mathf.Abs(playerRigidBody.linearVelocity.y) > Mathf.Epsilon;
        playerAnimator.SetBool("IsClimbing", playerHasVerticalSpeed);
    }
}
