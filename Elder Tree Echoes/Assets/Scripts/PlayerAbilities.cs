using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Represents each power that the staff can weild
/// </summary>
public enum Power
{
    Basic,
    Water,
    Light

}
public class PlayerAbilities : MonoBehaviour
{
    public static PlayerAbilities Instance { get; private set; }

    [SerializeField] private bool waterPowerUnlocked = false;
    [SerializeField] private bool lightPowerUnlocked = false;
   
    [HideInInspector] public float WaterEnergy { get; set; }
    [HideInInspector] public float LightEnergy { get; set; }

    [SerializeField] private float waterRegenRate, lightRegenRate;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        WaterEnergy = 0;
        LightEnergy = 0;
    }

    private void Update()
    {
        if (waterPowerUnlocked)
        {
            if (WaterEnergy < 100)
            {
                WaterEnergy += waterRegenRate * Time.deltaTime;
            }
        }

        if (lightPowerUnlocked)
        {
            if (LightEnergy < 100)
            {
                LightEnergy += lightRegenRate * Time.deltaTime;
            }
        }
    }

    public void UnlockPower(Power power)
    {
        switch (power)
        {
            case Power.Water:
                waterPowerUnlocked = true;
                Debug.Log("Water power unlocked!");
                break;

            case Power.Light:
                lightPowerUnlocked = true;
                Debug.Log("Light power unlocked!");
                break;

        }
    }

    public bool IsPowerUnlocked(Power power)
    {
        return power switch
        {
            Power.Water => waterPowerUnlocked,
            Power.Light => lightPowerUnlocked,
            _ => false
        };
    }
}
