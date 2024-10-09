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
    [SerializeField]
    MovementData movementData;

    [SerializeField]
    float jumpForce = 3f;

    private Vector2 direction = Vector3.zero;

    private bool wallCling;
    private float wallJumpDirection;
    private bool isWallJumping;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter = 0f;

    private bool canMove;

    // Represents all collidable platforms the player
    // can use/land on
    [SerializeField]
    LayerMask groundLayer;

    [SerializeField]
    LayerMask wallLayer;

    // Using rigid bodies for now, can change to a more robust system later
    private Rigidbody2D body; 
    private bool isFacingRight = true;

    public bool IsFacingRight
    {
        get { return isFacingRight; }
    }

    public bool CanMove
    {
        get { return canMove; }
        set { canMove = value; }
    }

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
    /// Makes the player move
    /// </summary>
    public void OnMove(InputAction.CallbackContext context)
    {
        // Makes the player start moving based on which key is pressed
        // A = (-1, 0), D = (1, 0)
        direction = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// Makes the player jump
    /// </summary>
    public void OnJump(InputAction.CallbackContext context)
    {
        if (IsGrounded() && context.performed) // Can only jump when touching the ground
        {
            body.AddForce(new Vector2(0, jumpForce));
        }
        else if(wallCling && context.performed)
        {
            wallJumpingCounter = wallJumpingTime;
            isWallJumping = true;
            wallJumpDirection = -transform.localScale.x;
            body.AddForce(new Vector2(wallJumpDirection * 200, jumpForce));
            Flip();
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        if (wallJumpingCounter < 0f)
        {
            isWallJumping = false;
        }
        if (!isWallJumping && canMove)
        {
            float targetSpeed = movementData.maxSpeed * direction.x;
            float speedDiff = targetSpeed - body.velocity.x;
            
            float accelRate;
            accelRate = (Mathf.Abs(movementData.maxSpeed) > 0.01f) ? movementData.accelAmount : movementData.deccelAmount;
            float movement = speedDiff * accelRate;
            Debug.Log(movement);

            body.AddForce(movement * Vector2.right);
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
        WallCling();
        wallJumpingCounter -= Time.deltaTime;
    }

    /// <summary>
    /// Determines whether or not the player is touching the ground
    /// </summary>
    /// <returns></returns>
    private bool IsGrounded()
    {
        //  The pivot of the empty game player object should be at the BOTTOM of the player sprite
        //  or else this will not work as intended
        return Physics2D.OverlapCircle(transform.position, 0.05f, groundLayer);
    }

    /// <summary>
    /// Checks whether or not the player is on the wall
    /// </summary>
    /// <returns></returns>
    private bool IsOnWall()
    {
        return Physics2D.OverlapCircle(transform.position, 0.6f, wallLayer);
    }

    private void WallCling()
    {
        if(IsOnWall() && !IsGrounded())
        {
            wallCling = true;
            body.velocity = new Vector2(body.velocity.x, Mathf.Clamp(body.velocity.y, 0f, float.MaxValue));
        }
        else
        {
            wallCling = false;
        }
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
}
