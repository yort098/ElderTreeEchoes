using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Represents each power that the staff can weild
/// </summary>
enum Power
{
    Basic,
    Water,
    Light
}

public class Staff : MonoBehaviour
{
    private Power power;

    [SerializeField]
    LayerMask plantLayer;

    [SerializeField]
    float clicksToCycle = 3.5f;

    [SerializeField]
    float cycleCooldown;

    const float mouseCycleSpeed = 120;
    float cycleTimer;
    float mouseCycles;
    


    private ProjectileManager projManager;
    //private LightBeam lightBeam;
    [SerializeField] Transform whackCheck;
    private Vector2 whackCheckSize = new Vector2(1.5f, 2);

    // A reference to the end part of the staff
    // (probably temporary until we get assets for it) 
    [SerializeField]
    private SpriteRenderer orbArea;

    [SerializeField]
    Sprite waterOrb, lightOrb;

    [SerializeField] GameObject lightBeamPref;
    private LightBeam lightBeam;

    // Whether or not the light beam should be active
    bool shineLight = false;

    private void Awake()
    {
        projManager = GameObject.Find("ProjectileManager").GetComponent<ProjectileManager>();
        //lightBeam = GameObject.Find("LightBeam").GetComponent<LightBeam>();
    }
    // Start is called before the first frame update
    void Start()
    {
        power = Power.Basic;
        cycleTimer = cycleCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        // Visually representing the different powers
        // by changing the color of the orb
        switch (power)
        {
            case Power.Water:
                orbArea.sprite = waterOrb; // Blue for water
                break;

            case Power.Light:
                orbArea.sprite = lightOrb; // Yellow for light
                break;

            default:
                orbArea.sprite = null; // White for basic
                break;
        }

        // Shine the beam and deplete staff's light energy if there's enough
        if (shineLight && GameManager.Instance.LightEnergy >= 0.15f)
        {
            lightBeam.Shine();
            GameManager.Instance.DepleteEnergy(ProjectileType.Light, 0.05f);
        }
        // Make the beam invisible when not in use
        else if (lightBeam)
        {
            lightBeam.StopShining();
        }

        if (cycleTimer <= 0)
        {
            mouseCycles = 0;
        }

        cycleTimer -= Time.deltaTime;
    }

    public void CyclePower(InputAction.CallbackContext context)
    {
        cycleTimer = cycleCooldown;

        if (context.performed)
        {
            mouseCycles += mouseCycleSpeed;
        }

        if (mouseCycles >= clicksToCycle * mouseCycleSpeed)
        {
            // Cycles between each weapon type // Only happens when the button is initially pressed
            if (context.ReadValue<float>() < 0)
            {
                power++;

                // Loops back around when the number gets out of range
                if (power > Power.Light)
                {
                    power = Power.Basic;
                }

            }
            else
            {
                power--;

                // Loops back around when the number gets out of range
                if (power < Power.Basic)
                {
                    power = Power.Light;
                }
            }

            mouseCycles = 0;
        }
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            switch (power)
            {
                case Power.Basic:
                    // Whack

                    // Temporarily disable until staff animations are ready
                    //if (GameManager.Instance.Enemies.Length > 0)
                    //{
                    //    foreach (GameObject e in GameManager.Instance.Enemies)
                    //    {
                    //        if (whackRange.IsTouching(e.GetComponent<BoxCollider2D>()))
                    //        {
                    //            e.GetComponent<EnemyScript>().TakeDamage(5);
                    //        }
                    //    }
                        
                    //}
                    break;

                case Power.Water:
                    // Uproot
                    break;

                case Power.Light:
                    // Light shot

                    // Get mouse input
                    Vector2 mouse = Mouse.current.position.ReadValue();

                    projManager.GenerateLightAttack(orbArea.transform.position, mouse);

                    break;
            }
        }
    }

    public void SecondaryAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            switch (power)
            {
                case Power.Basic:
                    // Do something
                    break;

                case Power.Water:
                    // Grow

                    projManager.GenerateWaterShot(orbArea.transform.position, Mouse.current.position.ReadValue());

                    break;

                case Power.Light:
                    // Revitalize

                    // Beam will continuously shine on mouse press hold until released
                    shineLight = context.control.IsPressed();
                    lightBeam = Instantiate(lightBeamPref, orbArea.transform).GetComponent<LightBeam>();

                    break;
            }
            
        }

        // Stop shining upon mouse release
        if (context.canceled && power == Power.Light)
        {
            shineLight = context.control.IsPressed();
            Destroy(lightBeam.gameObject);
        }
    }
}
