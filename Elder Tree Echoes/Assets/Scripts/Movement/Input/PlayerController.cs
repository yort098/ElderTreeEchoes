using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    // ScriptableObject containing all data relating to player's movement
    [SerializeField]
    PlayerMovementData movementData;

    // The player's rigid body
    private Rigidbody2D body;

    private RopeMovement ropeMovement;

    private Vector2 direction = Vector3.zero;

    private Vector2 wallJumpDirection;

    private Coroutine ropeSlide;
    private Coroutine wallSlide;

    public bool isDoor;
    public GameObject currentDoor;

    #region STATE CHECKS

    private bool canMove;

    private bool isTouchingFront; // Whether the player is touching the wall in front of them
    private bool wallCling;

    private bool isWallJumping;

    // Player Animator
    public Animator animator;
    

    #endregion

    #region COLLISION CHECKS

    [SerializeField]
    Transform groundCheck;
    private Vector2 groundCheckSize = new Vector2(0.5f, 0.03f);

    // All floor/platforms able to be stood on
    [SerializeField]
    LayerMask groundLayer;
    [SerializeField]
    LayerMask platformLayer;

    [SerializeField]
    Transform frontWallCheck;
    private Vector2 wallCheckSize = new Vector2(0.03f, 1f);

    // All walls/barriers/obstructions
    [SerializeField]
    LayerMask wallLayer;
    #endregion

    #region TIMERS

    private float coyoteTimeCounter = 0;
    private float stickTimeCounter = 0;

    #endregion

    #region STATE MACHINE

    public PlayerStateMachine StateMachine { get; set; }

    public PlayerIdleState IdleState { get; set; }

    public PlayerRunState RunState { get; set; }

    public PlayerJumpState JumpState { get; set; }

    public PlayerWallJumpState WallJumpState { get; set; }

    public PlayerClimbState ClimbState { get; set; }
    #endregion

    private bool isFacingRight = true;

    /// <summary>
    /// Whether or not the player is facing right
    /// </summary>
    public bool IsFacingRight
    {
        get { return isFacingRight; }
    }

    /// <summary>
    /// Whether or not the player can use directional inputs
    /// </summary>
    public bool CanMove
    {
        get { return canMove; }
        set { canMove = value; }
    }

    /// <summary>
    /// The current input direction being pressed by the player
    /// </summary>
    public Vector2 Direction
    {
        get { return direction; }
        set { direction = value; }
    }

    public PlayerMovementData MovementData { get { return movementData; } }

    private void Awake()
    {
        StateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdleState(this, StateMachine);
        RunState = new PlayerRunState(this, StateMachine);
        JumpState = new PlayerJumpState(this, StateMachine);
        WallJumpState = new PlayerWallJumpState(this, StateMachine);
        ClimbState = new PlayerClimbState(this, StateMachine);

        body = GetComponent<Rigidbody2D>();
        ropeMovement = GetComponent<RopeMovement>();
    }

    private void Start()
    {
        SetGravityScale(MovementData.gravityScale);
        canMove = true;
        StateMachine.Initialize(IdleState);

        stickTimeCounter = movementData.stickTime;
        //Debug.Log("this better work: "+ stickTimeCounter);
    }

    /// <summary>
    /// Changes the player's direction
    /// </summary>
    public void OnMove(InputAction.CallbackContext context)
    {
        // Makes the player start moving based on which key is pressed
        // A = (-1, 0), D = (1, 0)
        direction = context.ReadValue<Vector2>();

        if (IsOnWall())
        {
            // Start or restart the wall-sticking coroutine when on the wall
            if (wallSlide != null)
            {
                StopCoroutine(wallSlide);
            }

            stickTimeCounter = MovementData.stickTime;
            wallSlide = StartCoroutine(UnstickFromWall());
        }

        if (isDoor && direction.y > 0)
        {
            currentDoor.GetComponent<TeleportTrigger>().enabled = true;
            currentDoor.GetComponent<TeleportTrigger>().Activate();
        }



        // Lateral movement along the rope
        if (direction.x != 0 && ropeMovement.attached && context.performed)
        {
            Vector3 lateralPush = new Vector3(direction.x, 0, 0) * ropeMovement.pushForce;
            body.AddRelativeForce(lateralPush);
        }

        /* Debug.Log("attatched: " + ropeMovement.attached);
         Debug.Log("direction.y: " + direction.y);
         Debug.Log("performed: " + context.performed);
         if (direction.y == -1 && ropeMovement.attached && context.performed)
         {
             Debug.Log("down");
             ropeMovement.Slide(-1);
         }

         if (direction.y == 1 && ropeMovement.attached && context.performed)
         {
             Debug.Log("up");
             ropeMovement.Slide(1);
         }*/

        // Check for vertical movement input
        if (context.phase == InputActionPhase.Started && Mathf.Abs(direction.y) > 0 && ropeMovement.attached)
        {
            // Start continuous slide coroutine if holding W or S
            if (ropeSlide == null)
            {
                ropeSlide = StartCoroutine(ContinuousSlide((int)direction.y));
            }
        }
        else if (context.phase == InputActionPhase.Canceled && ropeSlide != null)
        {
            // Stop continuous slide coroutine when input is released
            StopCoroutine(ropeSlide);
            ropeSlide = null;
        }
    }

    private IEnumerator ContinuousSlide(int direction)
    {
        while (true)
        {
            ropeMovement.Slide(direction);
            yield return new WaitForSeconds(0.1f); // Adjust this interval for smoother or faster climbing
        }
    }

    /// <summary>
    /// Makes the player jump/wall jump
    /// </summary>
    public void OnJump(InputAction.CallbackContext context)
    {
        // Set the animator to play the jumping/landing character animation
        animator.SetBool("IsJumping", true); 

        StateMachine.ChangeState(JumpState);

        //Debug.Log("is facing right: " + isFacingRight);
        //Debug.Log("jumped!");
        if (coyoteTimeCounter > 0f && context.performed && !IsOnWall()) // Can only jump when touching the ground
        {
            body.AddForce(new Vector2(0, movementData.jumpForce), ForceMode2D.Impulse);
        }
        else if (IsOnWall() && context.performed && !isWallJumping) // On the wall
        {
            wallCling = false;
            body.gravityScale = movementData.gravityScale;
            stickTimeCounter = MovementData.stickTime;

            

            // The player can jump off the wall without
            // having to jump into it
            if (isFacingRight)
            {
                wallJumpDirection = Vector2.left;
            }
            else
            {
                wallJumpDirection = Vector2.right;
            }

            //Debug.Log("is Wall jumping: " + isWallJumping);
            // Applying wall jump
            body.velocity = Vector2.zero;
            Debug.Log("wall jump force: (" + movementData.wallJumpForce.x * wallJumpDirection.x + ", " + movementData.wallJumpForce.y + ")");
            body.AddForce(new Vector2(movementData.wallJumpForce.x * wallJumpDirection.x, movementData.wallJumpForce.y), ForceMode2D.Impulse);
            //Flip();

            isWallJumping = true;

            // Gets rid of force after wallJumpTime
            Invoke("DisableWallJump", movementData.wallJumpTime);
        }

        //Debug.Log(ropeMovement.attatched);
        if (context.performed && ropeMovement.attached)
        {
            ropeMovement.Detatch();
            body.AddForce(new Vector3(direction.x, 1, 0) * ropeMovement.pushForce, ForceMode2D.Impulse);
        }

        if (context.canceled)
        {
            if (body.velocity.y > 0 && !isWallJumping)
            {
                body.velocity = new Vector2(body.velocity.x, body.velocity.y / 2);
            }
            coyoteTimeCounter = 0;
            //stickTimeCounter = 0;
        }

    }

    /*public void OnWallCling(InputAction.CallbackContext context)
    {
        if (context.performed && IsOnWall() && !isWallJumping)
        {
            wallCling = true;

            // Removes sliding
            body.gravityScale = 0;
            body.velocity = new Vector2(body.velocity.x, 0);
        }

        if (context.canceled)
        {
            body.gravityScale = movementData.gravityScale;
            wallCling = false;
        }
    }*/

    void Update()
    {
        // Set up parameter to switch btw idle & running animation
        animator.SetFloat("Speed", Mathf.Abs(body.velocity.x));

        StateMachine.CurrentState.FrameUpdate();
        // Slightly increases gravity on descent
        if (body.velocity.y < 0) // falling
        {
            body.gravityScale = movementData.gravityScale * 1.5f;

            // Capping the fall speed
            body.velocity = new Vector2(body.velocity.x, Mathf.Max(body.velocity.y, -movementData.maxFallSpeed));

        }
        
        if (IsGrounded())
        {
            // Disable character jumping/falling & animate normally
            animator.SetBool("IsJumping", false); 

            //Debug.Log(coyoteTimeCounter);
            coyoteTimeCounter = movementData.coyoteTime;
        }
        

        coyoteTimeCounter -= Time.deltaTime;
        //stickTimeCounter -= Time.deltaTime;

    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();

        if (stickTimeCounter > 0 && IsOnWall())
        {
            canMove = false;

            Slide();
        }
        else if (stickTimeCounter < 0)
        {
            canMove = true;
        }
    }

    /// <summary>
    /// Determines whether or not the player is touching the ground
    /// </summary>
    /// <returns></returns>
    public bool IsGrounded()
    {
        // Check if on the ground
        Collider2D colground;
        if (colground = Physics2D.OverlapCircle(groundCheck.position, 0.05f, groundLayer))
        {
            return true;
        }

        // Check if on light platform
        Collider2D colPlatform;
        colPlatform = Physics2D.OverlapCircle(groundCheck.position, 0.05f, platformLayer);
        if (colPlatform && colPlatform.gameObject.GetComponent<LeafPlatform>().IsGrown)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Checks whether or not the player is on the wall
    /// </summary>
    /// <returns></returns>
    private bool IsOnWall()
    {
        return !IsGrounded() && Physics2D.OverlapBox(frontWallCheck.position, wallCheckSize, 0, wallLayer);
    }

    /// <summary>
    /// Removes wall jumping force
    /// </summary>
    private void DisableWallJump()
    {
        isWallJumping = false;
    }

    public void SetGravityScale(float value)
    {
        body.gravityScale = value;
    }

    /// <summary>
    /// Changes the current direction the player is facing
    /// </summary>
    private void Flip()
    {
        isFacingRight = !isFacingRight;

        Vector3 localScale = transform.localScale;
        localScale.x *= -1; // Reverses the sprite on the x-axis (horizontal)
        transform.localScale = localScale;
    }

    public void HandleMovement()
    {
        if (canMove)
        {
            // Changing the direction the character is facing
            // based on the direction the player is moving
            if (!isFacingRight && direction.x > 0f)
            {
                Flip();
            }
            else if (isFacingRight && direction.x < 0f)
            {
                Flip();
            }

            // The 
            float targetSpeed = 0;
            if (direction.x > 0)
            {
                targetSpeed = movementData.maxSpeed;
            }
            else if (direction.x < 0)
            {
                targetSpeed = -movementData.maxSpeed;
            }


            // Slowly ramps to target speed using lerping when wall jumping
            // ----- prevents the player from getting back to the wall too quickly
            if (isWallJumping)
            {
                targetSpeed = Mathf.Lerp(body.velocity.x, targetSpeed, movementData.lerpAmount);
            }
            else
            {
                targetSpeed = Mathf.Lerp(body.velocity.x, targetSpeed, 1);
            }

            float speedDiff = targetSpeed - body.velocity.x;

            // Accelerating/Deccelerating the player when they move
            float accelRate;

            if (IsGrounded())
            {
                accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? movementData.runAccelAmount : movementData.runDeccelAmount;
            }
            else
            {
                accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? MovementData.runAccelAmount * MovementData.airAccel : MovementData.runDeccelAmount * MovementData.airDeccel;
            }
                

            float movement = speedDiff * accelRate;

            body.AddForce(movement * Vector2.right, ForceMode2D.Force);
        }
    }

    private void Slide()
    {
        body.velocity = new Vector2(body.velocity.x, Mathf.Clamp(body.velocity.y, MovementData.slideSpeed, float.MaxValue));
    }

    public void HandleJump()
    {


    }

    public IEnumerator UnstickFromWall()
    {
        while (isFacingRight && direction.x < 0f || !isFacingRight && direction.x > 0f)
        {
            stickTimeCounter -= Time.deltaTime;
            
            if (stickTimeCounter <= 0)
            {
                break;
            }

            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(0.2f);
        stickTimeCounter = MovementData.stickTime;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);

        Gizmos.DrawWireCube(frontWallCheck.position, wallCheckSize);
    }
}
