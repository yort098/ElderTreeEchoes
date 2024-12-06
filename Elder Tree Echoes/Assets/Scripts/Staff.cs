using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Staff : MonoBehaviour
{
    private Power power;

    [SerializeField]
    LayerMask plantLayer;

    [SerializeField]
    LayerMask enemyLayer;

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
        power = Power.Water;
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
        }

        // Shine the beam and deplete staff's light energy if there's enough
        if (shineLight && PlayerAbilities.Instance.LightEnergy >= 0.15f)
        {
            lightBeam.IsShining = true;
            PlayerAbilities.Instance.DepleteEnergy(ProjectileType.Light, 0.15f);
        }
        // Make the beam invisible when not in use
        else if (lightBeam)
        {
            lightBeam.IsShining = false;
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
                    power = Power.Water;
                }

            }
            else
            {
                power--;

                // Loops back around when the number gets out of range
                if (power < Power.Water)
                {
                    power = Power.Light;
                }
            }

            mouseCycles = 0;
        }
    }

    public void OnSwitchPower(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (Keyboard.current.digit1Key.IsPressed() && PlayerAbilities.Instance.IsPowerUnlocked(Power.Water))
            {
                power = Power.Water;
            }
            else if (Keyboard.current.digit2Key.IsPressed() && PlayerAbilities.Instance.IsPowerUnlocked(Power.Light))
            {
                power = Power.Light;
            }
        }
       
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            switch (power)
            {
                case Power.Water:
                    // Whack

                    if (PlayerAbilities.Instance.WaterEnergy >= 30.0f)
                    {
                        PlayerAbilities.Instance.DepleteEnergy(ProjectileType.Water, 30.0f);
                        if (GameManager.Instance.Enemies.Length > 0)
                        {
                            //foreach (GameObject e in GameManager.Instance.Enemies)
                            //{
                            Collider2D col;
                            if (col = Physics2D.OverlapCircle(whackCheck.position, 1.0f, enemyLayer))
                            {
                                col.GetComponent<EnemyScript>().Damage(5);
                            }
                            //if (whackCheck.IsTouching(e.GetComponent<BoxCollider2D>()))
                            //{
                            //    e.GetComponent<EnemyScript>().Damage(5);
                            //}
                            //}
                            //colPlatform = Physics2D.OverlapCircle(groundCheck.position, 0.05f, platformLayer);
                        }
                    }
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
                case Power.Water:
                    // Grow

                    projManager.GenerateWaterShot(orbArea.transform.position, Mouse.current.position.ReadValue());

                    break;

                case Power.Light:
                    // Revitalize

                    // Beam will continuously shine on mouse press hold until released
                    shineLight = context.control.IsPressed();
                    lightBeam = Instantiate(lightBeamPref).GetComponent<LightBeam>();

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
