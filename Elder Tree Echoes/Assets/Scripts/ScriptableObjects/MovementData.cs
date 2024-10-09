using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Movement Data", menuName = "ScriptableObject/MovmentData")]

public class MovementData : ScriptableObject
{
    public float maxSpeed;

    public float acceleration;
    public float accelAmount;

    public float deceleration;
    public float deccelAmount;

}
