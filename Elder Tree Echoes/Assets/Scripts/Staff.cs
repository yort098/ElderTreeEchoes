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

    private ProjectileManager projManager;
    private BoxCollider2D whackRange;

    // A reference to the end part of the staff
    // (probably temporary until we get assets for it) 
    [SerializeField]
    private SpriteRenderer orbArea;

    [SerializeField]
    Sprite waterOrb, lightOrb;

    private void Awake()
    {
        projManager = GameObject.Find("ProjectileManager").GetComponent<ProjectileManager>();
        whackRange = GetComponent<BoxCollider2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        power = Power.Basic;
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
    }

    public void CyclePower(InputAction.CallbackContext context)
    {
        // Cycles between each weapon type
        if (context.performed) // Only happens when the button is initially pressed
        {
            if (context.ReadValue<Vector2>().y < 0)
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
                    break;
            }
            
        }
    }
}
