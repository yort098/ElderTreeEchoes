using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SearchService;
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

    private PlayerController player;
    private LightShotManager lightShotManager;
    private BoxCollider2D whackRange;
    private GameObject enemy;

    // A reference to the end part of the staff
    // (probably temporary until we get assets for it) 
    [SerializeField]
    private SpriteRenderer orb;

    // Start is called before the first frame update
    void Start()
    {
        power = Power.Basic;
        lightShotManager = GameObject.Find("LightShotManager").GetComponent<LightShotManager>();
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        whackRange = GetComponentInParent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Visually representing the different powers
        // by changing the color of the orb
        switch (power)
        {
            case Power.Water:
                orb.color = Color.blue; // Blue for water
                break;

            case Power.Light:
                orb.color = Color.yellow; // Yellow for light
                break;

            default:
                orb.color = Color.white; // White for basic
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
                    enemy = GameObject.Find("Enemy");
                    if (whackRange.IsTouching(enemy.GetComponent<BoxCollider2D>()))
                    {
                        Destroy(enemy);
                    }
                    break;

                case Power.Water:
                    // Uproot
                    break;

                case Power.Light:
                    // Light shot
                    lightShotManager.GenerateLightAttack(orb.transform.position, player.IsFacingRight);
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
                    Collider2D plant;
                    if (plant = Physics2D.OverlapCircle(orb.transform.position, 0.5f, plantLayer))
                    {
                        Debug.Log(plant.name);
                        plant.GetComponent<Plant>().Grow();
                        plant.GetComponent<SpriteRenderer>().color = Color.green;
                    }

                    break;

                case Power.Light:
                    // Revitalize
                    break;
            }
            
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(orb.transform.position, 0.5f);
    }
}
