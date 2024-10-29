using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class LightBeam : MonoBehaviour
{
    [SerializeField]
    private GameObject staffOrb;
    private SpriteRenderer spriteRend;

    private void Awake()
    {
        spriteRend = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        // Send beam to orb's position and make it invisible initially
        transform.position = staffOrb.transform.position;
        spriteRend.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = staffOrb.transform.position;
    }

    // Activate the light beam
    public void Shine()
    {
        // Show beam
        spriteRend.enabled = true;

        // Get directional vector between mouse and staff orb
        Vector2 mouse = Mouse.current.position.ReadValue();
        Vector3 startPos = new Vector2(transform.position.x, transform.position.y);
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouse);
        Vector2 direction = mouseWorldPosition - startPos;
        direction.Normalize();

        // Determine the angle between the mouse and orb, and use it to determine the beam's rotation/orientation
        float rotationAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, rotationAngle);
    }

    // Deactivate light beam
    public void StopShining()
    {
        spriteRend.enabled = false;
    }
}
