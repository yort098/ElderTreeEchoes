using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Movement Data", menuName = "ScriptableObject/MovmentData")]

public class PlayerMovementData : ScriptableObject
{
    // Speed
    public float maxSpeed;

    public float acceleration;
    public float accelAmount;

    public float deceleration;
    public float deccelAmount;

    public float lerpAmount;

    // Jump/Falling
    public float jumpForce;
    public float gravityScale;
    public float maxFallSpeed;

    // Wall Jump
    public Vector2 wallJumpForce;
    public float wallJumpTime;

    // Extra Schmovement
    public float coyoteTime;

    

}
