using System.Collections;
using System.Collections.Generic;
using UnityEditor.MPE;
using UnityEngine;
using UnityEngine.UIElements;

// Enum representing the projectile type of this object
enum ProjectileType
{
    Light,
    Water
}

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private ProjectileType projectileType;
    
    [SerializeField]
    float projSpeed;

    private Vector2 location;
    private Vector2 direction;
    private Rigidbody2D body;
    private int damage = 1;

    [SerializeField]
    float upTime = 3;
    private bool active;
    private float activeCounter;

    public Vector2 Direction
    {
        get { return direction; }
        set { direction = value; }
    }

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        active = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        location = transform.position;
        direction = Vector2.right;
    }

    // Update is called once per frame
    void Update()
    {
        location += body.velocity * Time.deltaTime;
        transform.position = location;

        if (active)
        {
            activeCounter += Time.deltaTime;

            if (activeCounter >= upTime)
            {
                active = false;
                Destroy(gameObject);
            }

            if (Physics2D.OverlapCircle(transform.position, 0.05f, LayerMask.GetMask("Floor", "Walls")))
            {
                Destroy(gameObject);
            }

            Collider2D col;
            if (col = Physics2D.OverlapCircle(transform.position, 0.05f, LayerMask.GetMask("Interactable")))
            {
                // Making plants grow with water
                if (col.GetComponent<Plant>() && projectileType == ProjectileType.Water)
                {
                    col.GetComponent<Plant>().Grow();
                    col.GetComponent<SpriteRenderer>().color = Color.green;
                }
               

                Destroy(gameObject);
            }

            if (col = Physics2D.OverlapCircle(transform.position, 0.05f, LayerMask.GetMask("Enemy")))
            {
                 // Damaging enemies with light
                if (col.GetComponent<EnemyScript>() && projectileType == ProjectileType.Light)
                {
                    col.GetComponent<EnemyScript>().TakeDamage(damage);
                }

                if (col.GetComponent<SwoopingEnemy>() && projectileType == ProjectileType.Light)
                {
                    col.GetComponent<SwoopingEnemy>().TakeDamage(damage);
                }

                Destroy(gameObject);
            }

        }

    }

    public void Fire()
    {
        active = true;
        // Water
        if (projectileType == ProjectileType.Water)
        {
            body.velocity = new Vector2(projSpeed * direction.x, projSpeed * direction.y);
        }
        // Light shot
        else if (projectileType == ProjectileType.Light)
        {
            body.velocity = new Vector2(projSpeed * direction.x, projSpeed * direction.y);
        }
    }

    // Likely won't be needed eventually
    public void setDirection(bool facingRight)
    {
        if (facingRight)
        {
            direction = Vector2.right;
        }
        else
        {
            direction = Vector2.left;
        }
    }
    
    // Get the direction of the projectile shot based on the mouse position
    public void setDirectionMouse(Vector2 mouseScreenLocation)
    {
        Vector2 startPos = new Vector2(transform.position.x, transform.position.y);
        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenLocation);
        direction = mouseWorldPosition - startPos;
        direction.Normalize();
    }
}
