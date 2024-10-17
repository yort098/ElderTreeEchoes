using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/EnemyAttributes")]
public class EnemyAttributes : ScriptableObject
{
    // Data to be used with each different type of enemy
    public float speed = 2f;
    // Disable for time being so enemies don't share HP
    //public float health = 10;
    public float damage = 5f;
}
