using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Movement Data", menuName = "ScriptableObject/MovmentData")]

public class PlayerMovementData : ScriptableObject
{
    // Speed
    public float maxSpeed;

    public float runAcceleration;
    public float runAccelAmount;
    public float airAccel;

    public float runDecceleration;
    public float runDeccelAmount;
    public float airDeccel;

    public float lerpAmount;

    // Jump/Falling
    public float jumpForce;
    public float jumpHeight;
    public float jumpTimeToApex;

    public float gravityStrength;
    public float gravityScale;
    public float maxFallSpeed;

    // Wall Jump
    public Vector2 wallJumpForce;
    public float wallJumpTime;

    public float slideSpeed;
    public float slideAccel;

    // Extra Schmovement
    public float coyoteTime;
    public float stickTime;

    private void OnValidate()
    {
        //Calculate gravity strength using the formula (gravity = 2 * jumpHeight / timeToJumpApex^2) 
        gravityStrength = -(2 * jumpHeight) / (jumpTimeToApex * jumpTimeToApex);

        //Calculate the rigidbody's gravity scale (ie: gravity strength relative to unity's gravity value, see project settings/Physics2D)
        gravityScale = gravityStrength / Physics2D.gravity.y;

        //Calculate are run acceleration & deceleration forces using formula: amount = ((1 / Time.fixedDeltaTime) * acceleration) / runMaxSpeed
        runAccelAmount = (50 * runAcceleration) / maxSpeed;
        runDeccelAmount = (50 * runDecceleration) / maxSpeed;

        //Calculate jumpForce using the formula (initialJumpVelocity = gravity * timeToJumpApex)
        jumpForce = Mathf.Abs(gravityStrength) * jumpTimeToApex;

        #region Variable Ranges
        runAcceleration = Mathf.Clamp(runAcceleration, 0.01f, maxSpeed);
        runDecceleration = Mathf.Clamp(runDecceleration, 0.01f, maxSpeed);
        #endregion
    }
}


