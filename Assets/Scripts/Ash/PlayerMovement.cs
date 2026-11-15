using System;
using System.Collections;
using System.Numerics;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //Miscelleaneous variables
    private Rigidbody2D rb;
    private Animator animator;
    float ghostSpawnTimer;

    //Variables for the dash tap system
    float dashPressedTimer;
    float dashPressedLimit = 0.2f;
    bool dashPressedLock = false;
    float dashLength = 0;

    //Variable for the jump tap system
    float jumpPressedTimer;
    float jumpPressedLimit = 0.1f;
    bool jumpPressedLock = false;

    //Variables for the grace mechanic
    float graceTimer;

    //Variables for the jump pad mechanic
    bool jumpPadLock;

    //please let it end please please
    //public UnityEngine.Vector2 targetPos;

    [Header("PlayerInput")]
    [SerializeField] private float horizontalInput;
    [SerializeField] private bool jumpPressed;
    [SerializeField] private bool jumpHeld;
    [SerializeField] private bool dashPressed;
    [SerializeField] private bool dashHeld;
    [SerializeField] private float lastXInput;
    [Header("PlayerVariables")]
    //Moving horizontally
    public float acceleration = 2;
    public float deceleration = 0.8f;
    public float maxXSpeed = 8;
    public float walkSpeed = 8;
    public float sprintSpeed = 10;
    public float turnaroundFactor = 0.5f;
    //Jumping
    public float maxYSpeed = 8;
    public float jumpHeight = 100;
    public float wallJumpStrength;
    public float graceTime = 0.2f;
    public float gravity = 30;
    public float normalGravity;
    //Dashing
    public float dashSpeed = 400;
    public float dashTime = 0.2f;
    public float dashDelay = 0.5f;
    public float dashCooldownAfterJump;
    //Misc
    public static UnityEngine.Vector2 spawnPos;
    public float ledgePopHeight;

    [Header("EffectVariables")]
    public float ghostSpawnInterval;
    [Header("Flags")]
    public bool isGrounded = false;
    public bool hasJump = false;
    public int wallDirection;
    public bool wallJumpLocked = false;
    public bool onWall = false;
    public bool graceActive = false;
    public bool hasDash = false;
    public bool isDashing = false;
    public bool isSprinting = false;
    [Header("External Stuff")]
    public GameObject afterImage;
    public GameObject dashAfterImage;
    public Transform afterImageContainer;
    public GameObject hitbox;
    public GameObject wallHitbox;
    public GameObject groundHitbox;
    public LayerMask groundMask;
    


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spawnPos = new UnityEngine.Vector2(-33.59f, -3.61f);
    }

    void FixedUpdate()
    {
        HandleMovement();
        HandleJump();
        HandleDash();
        HitBoxFixer();
        ghostSpawnTimer += Time.fixedDeltaTime;
        if (ghostSpawnTimer > ghostSpawnInterval)
        {
            if (maxXSpeed > walkSpeed)
            {
                SpawnAfterImage(false);
                ghostSpawnTimer = 0;
            }
        }
    }

    void HandleMovement()
    {
        horizontalInput = InputManager.instance.horizontalInput;
        if (!isGrounded && Mathf.Sign(horizontalInput) == Mathf.Sign(wallDirection) && onWall) { horizontalInput = 0; }
        UnityEngine.Vector2 moveDirection = new(horizontalInput, 0);
        UnityEngine.Vector2 velocity = moveDirection * acceleration;
        //Accelerates towards the right direction or decelerates if no key is held
        if (velocity != UnityEngine.Vector2.zero)
        {
            if (Mathf.Sign(horizontalInput) != Mathf.Sign(rb.linearVelocityX) && isGrounded)
            {
                UnityEngine.Vector2 currentVelocity = rb.linearVelocity;
                UnityEngine.Vector2 counterForce = currentVelocity * turnaroundFactor * -1;
                rb.linearVelocity += counterForce;
            }
            rb.linearVelocity += velocity;
        }
        else
        {
            rb.linearVelocityX *= deceleration;
        }
        //Movement cap
        if (Math.Abs(rb.linearVelocityX) > maxXSpeed)
        {
            rb.linearVelocity = new UnityEngine.Vector2(Math.Sign(rb.linearVelocityX) * maxXSpeed, rb.linearVelocityY);
        }

        if (horizontalInput != 0) { lastXInput = horizontalInput; }



        animator.SetFloat("Xspeed", math.abs(rb.linearVelocityX));
        animator.SetFloat("Yspeed", rb.linearVelocityY);
        animator.SetFloat("Xvelocity", velocity.x);
        animator.SetBool("jumpPressed", jumpPressed);
        animator.SetBool("hasJump", hasJump);
        animator.SetBool("isGrounded", isGrounded);
    }

    void HandleJump()
    {

        #region JumpHeld and JumpPressed system
        jumpHeld = InputManager.instance.jumpInput;
        if (!jumpPressedLock) { jumpPressed = jumpHeld; }
        if (jumpPressed == true)
        {
            jumpPressedTimer += Time.fixedDeltaTime;
            if (jumpPressedTimer > jumpPressedLimit)
            {
                jumpPressed = false;
                jumpPressedLock = true;
                jumpPressedTimer = 0;
            }
        }
        if (jumpHeld == false) { jumpPressedLock = false; }
        
        #endregion

        #region Grace Mechanic
        if (isGrounded) { graceActive = true; }
        if (!isGrounded)
        {
            graceTimer += Time.fixedDeltaTime;
            if (graceTimer > graceTime)
            {
                graceActive = false;
                graceTimer = 0;
            }
        }
        #endregion

        //Jump
        if (jumpPressed && hasJump && graceActive)
        {
            rb.linearVelocity += UnityEngine.Vector2.up * jumpHeight;
            hasJump = false;
            StartCoroutine(UseAfterJumpCoroutine());
        }

        if (!isGrounded)
        {
            rb.linearVelocity += UnityEngine.Vector2.down * gravity;
        }

        //Walljump
        // if (onWall && jumpPressed && !wallJumpLocked)
        // {
        //     Debug.Log("walljumped ");
        //     horizontalInput = InputManager.instance.horizontalInput;
        //     UnityEngine.Vector2 wallJumpDirection = new UnityEngine.Vector2(0, 1);
        //     UnityEngine.Vector2 wallJumpVector = wallJumpDirection * wallJumpStrength;
        //     rb.linearVelocity += wallJumpVector;
        //     StartCoroutine(UseWallJumpCoroutine());
        // }
        

        if (rb.linearVelocityY > maxYSpeed)
        {
            rb.linearVelocity = new UnityEngine.Vector2(rb.linearVelocityX, maxYSpeed);
        }

        if (!isDashing) { gravity = normalGravity; }
    }

    void HandleDash()
    {
        #region DashPressed and dashHeld input system
        dashHeld = InputManager.instance.dashHeld;
        if (!dashPressedLock) { dashPressed = dashHeld; }
        if (dashPressed == true)
        {
            dashPressedTimer += Time.fixedDeltaTime;
            if (dashPressedTimer > dashPressedLimit)
            {
                dashPressed = false;
                dashPressedLock = true;
                dashPressedTimer = 0;
            }
        }
        if (dashHeld == false) { dashPressedLock = false; }
        #endregion


        if (dashHeld) { maxXSpeed = sprintSpeed; isSprinting = true; }
        else { maxXSpeed = walkSpeed; isSprinting = false; }
        if (dashPressed && hasDash && !isDashing)
        {
            UnityEngine.Vector2 moveDirection = new(lastXInput, 0);
            StartCoroutine(UseDashCoroutine(moveDirection));
        }
    }

    public void SetGrounded(bool grounded)
    {
        if (grounded)
        {
            hasJump = true;
            hasDash = true;
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    public void SetWalled(bool walled, int direction)
    {
        onWall = walled;
        wallDirection = direction;
    }

    public void ledgePop()
    {
        if (!isGrounded && !onWall)
        {
            rb.position = rb.position + new UnityEngine.Vector2(lastXInput, 1) * ledgePopHeight;
        }
    }

    public void ToSpawn()
    {
        transform.position = spawnPos;
    }

    public void JumpPad(float jumpPadStrength)
    {
        rb.linearVelocity += UnityEngine.Vector2.up * jumpPadStrength;
    }

    private IEnumerator UseDashCoroutine(UnityEngine.Vector2 direction)
    {
        hasDash = false;
        gravity = 0;
        rb.linearVelocity = UnityEngine.Vector2.zero;
        isDashing = true;

        UnityEngine.Vector2 startPos = transform.position;
        UnityEngine.Vector2 targetPos = startPos + direction * dashSpeed * dashTime;

        while (dashLength < dashTime)
        {
            dashLength += Time.fixedDeltaTime;
            float t = dashLength / dashTime;
            UnityEngine.Vector2 newPos = UnityEngine.Vector2.Lerp(startPos, targetPos, t);
            rb.MovePosition(newPos);
            SpawnAfterImage(true);
            yield return new WaitForFixedUpdate();
        }
        dashLength = 0;
        gravity = normalGravity;
        yield return new WaitForSeconds(dashDelay);
        isDashing = false;
        if (isGrounded) { hasDash = true; }
    }

    private IEnumerator UseAfterJumpCoroutine()
    {
        hasDash = false;
        yield return new WaitForSeconds(dashCooldownAfterJump);
        hasDash = true;
    }

    private IEnumerator UseWallJumpCoroutine()
    {
        wallJumpLocked = true;
        yield return new WaitForSeconds(0.5f);
        wallJumpLocked = false;
    }


    void SpawnAfterImage(bool isBlue)
    {
        GameObject afterImageToUse;
        if (isBlue) { afterImageToUse = dashAfterImage; } else { afterImageToUse = afterImage; }

        GameObject ghost = Instantiate(afterImageToUse, transform.position, transform.rotation);
        ghost.transform.SetParent(afterImageContainer);
        SpriteRenderer ASR = GetComponent<SpriteRenderer>();
        SpriteRenderer GSR = ghost.GetComponent<SpriteRenderer>();
        if (ASR && GSR)
        {
            GSR.sprite = ASR.sprite;
        }
    }

    void HitBoxFixer()
    {
        CapsuleCollider2D hitboxCollider = hitbox.GetComponent<CapsuleCollider2D>();
        BoxCollider2D groundCollider = groundHitbox.GetComponent<BoxCollider2D>();
        BoxCollider2D wallCollider = wallHitbox.GetComponent<BoxCollider2D>();
        if (lastXInput == 1)
        {
            hitboxCollider.offset = new UnityEngine.Vector2(0.04824504f, -0.04495452f);
            wallCollider.offset = new UnityEngine.Vector2(0.1709992f, -0.0006413758f);
            groundCollider.offset = new UnityEngine.Vector2(0.08258928f, -0.2739546f);
        }
        else
        {
            hitboxCollider.offset = new UnityEngine.Vector2(-0.08f, -0.04495452f);
            wallCollider.offset = new UnityEngine.Vector2(-0.155f, -0.0006413758f);
            groundCollider.offset = new UnityEngine.Vector2(-0.06f, -0.2739546f);
        }
    }
}
