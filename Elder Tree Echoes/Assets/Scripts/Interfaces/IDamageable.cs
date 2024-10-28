using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IDamageable
{
    float MaxHealth { get; set; }

    float CurrentHealth { get; set; }

    //Slider Healthbar { get; set; }

    void Damage(float amount);

    void Die();
}
