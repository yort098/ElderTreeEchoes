using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class LightBeam : MonoBehaviour
{
    // Start is called before the first frame update
    //[SerializeField]
    //GameObject lightBeam;
    //private Quaternion rotator;
    //private Vector3 direction;
    [SerializeField]
    private GameObject staffOrb;
    private SpriteRenderer spriteRend;

    private void Awake()
    {
        //rotator = new Quaternion();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        transform.position = staffOrb.transform.position;
        spriteRend.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = staffOrb.transform.position;
        //spriteRend.enabled = false;
    }

    public void Shine()
    {
        //spriteRend.enabled = true;
        Vector2 mouse = Mouse.current.position.ReadValue();
        Vector3 startPos = new Vector2(transform.position.x, transform.position.y);
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouse);
        Vector2 direction = mouseWorldPosition - startPos;
        direction.Normalize();

        //rotator.SetLookRotation(direction);
        //rotator.SetLookRotation(direction.normalized);
        float z = transform.eulerAngles.z;
        z += 150.0f * Time.deltaTime;
        //transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, z);

        // 57.5f is a magic number for some reason, I'm not sure why it's necessary to work
        float rotationAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, rotationAngle);
    }
}
