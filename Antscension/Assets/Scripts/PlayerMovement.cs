using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    // Dashing variables
    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 6f;   // how fast the player will dash (speed)
    private float dashingTime = 0.15f; // how long the player will dash (distance)
    private float dashCooldown = 1f;   // how long the player will wait to dash again (time)
    private bool isInPoisonZone = false;
    private Coroutine poisonCoroutine;

    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 8f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] Vector2 deathKick = new Vector2(10f, 10f);
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun;
    [SerializeField] int maxJumpsInAir = 1; // the numbers of jumps the player can do in the air
    [SerializeField] TrailRenderer tr;
    [SerializeField] float interactRadius = 1f;

    // This is a bad implementation since if the scale of player is changed
    // we need to change this value os well
    int scale = 5;

    Vector2 moveInput;
    // The velocity property determines the movement
    Rigidbody2D playerRigidBody;
    Animator playerAnimator;
    CapsuleCollider2D playerBodyCollider;
    BoxCollider2D playerFeetCollider;
    float gravityScaleAtStart;

    bool isAlive = true;
    int jumpCount = 0;

    void Start()
    {
        // Initialization of variables
        playerRigidBody = GetComponent<Rigidbody2D>();       
        playerAnimator = GetComponent<Animator>();
        playerBodyCollider = GetComponent<CapsuleCollider2D>();
        playerFeetCollider = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = playerRigidBody.gravityScale;
    }

    void Update()
    {
        if (!isAlive) { return; }
        if (isDashing) { return; }
        Run(); 
        FlipSprite();
        ClimbLadder();
        HandlePoisonZone();
        Die();

         // Reset jump count when the player is on the ground
        if (playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            jumpCount = 0;
        }

        if (jumpCount == maxJumpsInAir)
        {
            Debug.Log("Resetting jumps");
            StartCoroutine(WaitAndResetJumps());
        }
    }

    void OnClaws(InputValue value) 
    {
        if (!isAlive) { return; }

        bool isPressed = value.isPressed;

        if (isPressed)
        {
            playerAnimator.SetTrigger("Attack");
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, interactRadius);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Enemy"))
                {
                    Destroy(hitCollider.gameObject);
                    break;
                }
            }
        }
    }

    void ResetAttackTrigger()
    {
        playerAnimator.ResetTrigger("Attack");
    }

    void OnAttack(InputValue value) 
    {
        // Get inputs from PlayerInput component
        if (!isAlive) { return; }
        Instantiate(bullet, gun.position, transform.rotation);
    }

    void OnMove(InputValue value) 
    {
        // Get inputs from PlayerInput component
        moveInput = value.Get<Vector2>();

        if (!isAlive) { return; }
    }

    void OnJump(InputValue value) 
    {
        // Prevents the player from jumping in the air
        if (!isAlive) { return; }
        if(jumpCount < maxJumpsInAir && value.isPressed)
        {
            playerRigidBody.linearVelocity += new Vector2(0f, jumpSpeed);
            jumpCount++;
            FindObjectOfType<GameSession>().DecrementJumps();
        }
    }

    public void UpdateMaxJumps(int extraJumps)
    {
        maxJumpsInAir = 1 + extraJumps;
    }

    private IEnumerator WaitAndResetJumps()
    {
        yield return new WaitForSeconds(0.75f); // Wait for 0.75 seconds
        FindObjectOfType<GameSession>().ResetJumps();
    }

    void OnDash(InputValue value)
    {
        if (!isAlive) { return; }
        if (canDash && value.isPressed)
        {
            FindObjectOfType<GameSession>().DecrementDash();
            StartCoroutine(Dash());
            StartCoroutine(WaitAndResetDash());
        }
    }

    private IEnumerator WaitAndResetDash()
    {
        yield return new WaitForSeconds(0.75f); // Wait for 0.75 seconds
        FindObjectOfType<GameSession>().ResetDash();
    }

    public void UpgradeDash()
    {
        dashingTime += 0.3f;
    }

    // Dash logic
    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = playerRigidBody.gravityScale;
        playerRigidBody.gravityScale = 0f;
        playerRigidBody.linearVelocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        tr.emitting = true;
        // Wait for the dashing time
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        playerRigidBody.gravityScale = originalGravity;
        isDashing = false;
        // Wait for the cooldown time
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
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
        if (!playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
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

    void Die()
    {
        if (playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards")))
        {
            isAlive = false;
            playerAnimator.SetTrigger("Dying");
            playerRigidBody.linearVelocity = deathKick;
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }

    void HandlePoisonZone()
    {
        if (playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Poison")))
        {
            if (!isInPoisonZone)
            {
                isInPoisonZone = true;
                poisonCoroutine = StartCoroutine(PoisonZoneTimer());
            }
        }
        else
        {
            if (isInPoisonZone)
            {
                isInPoisonZone = false;
                if (poisonCoroutine != null)
                {
                    StopCoroutine(poisonCoroutine);
                    poisonCoroutine = null;
                }
            }
        }
    }

    private IEnumerator PoisonZoneTimer()
    {
        yield return new WaitForSeconds(2f); // Time the player can stay in the poison zone
        if (isInPoisonZone) // Double-check the player is still in the zone
        {
            isAlive = false;
            playerAnimator.SetTrigger("Dying");
            playerRigidBody.linearVelocity = deathKick;
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }
}
