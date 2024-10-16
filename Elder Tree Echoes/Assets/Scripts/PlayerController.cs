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

    private Vector2 direction = Vector3.zero;

    private Vector2 wallJumpDirection;

    #region STATE CHECKS

    private bool canMove;

    private bool isTouchingFront; // Whether the player is touching the wall in front of them
    private bool wallCling;

    private bool wallJumping;
    

    #endregion

    #region COLLISION CHECKS

    [SerializeField]
    Transform groundCheck;
    private Vector2 groundCheckSize = new Vector2(0.5f, 0.03f);

    // All floor/platforms able to be stood on
    [SerializeField]
    LayerMask groundLayer;

    [SerializeField]
    Transform frontWallCheck;
    private Vector2 wallCheckSize = new Vector2(0.03f, 0.5f);

    // All walls/barriers/obstructions
    [SerializeField]
    LayerMask wallLayer;
    #endregion

    #region TIMERS

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

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        
    }

    private void Start()
    {
        canMove = true;
    }

    /// <summary>
    /// Changes the player's direction
    /// </summary>
    public void OnMove(InputAction.CallbackContext context)
    {
        // Makes the player start moving based on which key is pressed
        // A = (-1, 0), D = (1, 0)
        direction = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// Makes the player jump/wall jump
    /// </summary>
    public void OnJump(InputAction.CallbackContext context)
    {
        if (IsGrounded() && context.performed) // Can only jump when touching the ground
        {
            body.AddForce(new Vector2(0, movementData.jumpForce), ForceMode2D.Impulse);
        }
        else if (wallCling && context.performed) // On the wall
        {
            wallJumping = true;

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

            // Applying wall jump
            body.AddForce(new Vector2(movementData.wallJumpForce.x * wallJumpDirection.x, movementData.wallJumpForce.y), ForceMode2D.Impulse);
            Flip();

            // Gets rid of force after wallJumpTime
            Invoke("DisableWallJump", movementData.wallJumpTime);
        }

    }

    void Update()
    {
        // Slightly increases gravity on descent
        if (body.velocity.y < 0) // falling
        {
            body.gravityScale = movementData.gravityScale * 1.5f;

            // Capping the fall speed
            body.velocity = new Vector2(body.velocity.x, Mathf.Max(body.velocity.y, -movementData.maxFallSpeed));

        }

        // Making sure the player "affixes" to the wall
        if (IsOnWall() && !wallJumping)
        {
            wallCling = true;
            
            // Removes sliding
            body.gravityScale = 0;
            body.velocity = new Vector2(body.velocity.x, 0);
        }
        else
        {
            body.gravityScale = movementData.gravityScale;
            wallCling = false;
        }


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

        
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            // The 
            float targetSpeed = movementData.maxSpeed * direction.x;

            // Slowly ramps to target speed using lerping when wall jumping
            // ----- prevents the player from getting back to the wall too quickly
            if (wallJumping)
            {
                targetSpeed = Mathf.Lerp(body.velocity.x, targetSpeed, movementData.lerpAmount);
            }

            float speedDiff = targetSpeed - body.velocity.x;

            // Accelerating/Deccelerating the player when they move
            float accelRate;
            accelRate = (Mathf.Abs(movementData.maxSpeed) > 0.01f) ? movementData.accelAmount : movementData.deccelAmount;

            float movement = speedDiff * accelRate;

            body.AddForce(movement * Vector2.right);
        }
    }

    /// <summary>
    /// Determines whether or not the player is touching the ground
    /// </summary>
    /// <returns></returns>
    private bool IsGrounded()
    {
        return Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0, groundLayer);
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
        wallJumping = false;
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


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);

        Gizmos.DrawWireCube(frontWallCheck.position, wallCheckSize);
    }
}
