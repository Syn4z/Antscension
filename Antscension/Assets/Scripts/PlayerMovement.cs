using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 8f;
    // This is a bad implementation since if the scale of player is changed
    // we need to change this value os well
    int scale = 5;

    Vector2 moveInput;
    // The velocity property determines the movement
    Rigidbody2D playerRigidBody;
    Animator playerAnimator;
    CapsuleCollider2D playerCapsuleCollider;

    void Start()
    {
        // Initialization of variables
        playerRigidBody = GetComponent<Rigidbody2D>();       
        playerAnimator = GetComponent<Animator>();
        playerCapsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    void Update()
    {
       Run(); 
       FlipSprite();
    }

    void OnMove(InputValue value) 
    {
        // Get inputs from PlayerInput component
        moveInput = value.Get<Vector2>();
        Debug.Log(moveInput);
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
}
