using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    float projSpeed;

    private Vector2 location;
    private Vector2 direction;
    private Rigidbody2D body;
    private int damage;

    [SerializeField]
    float upTime = 3;
    private bool alive;
    private float aliveTime;

    public Vector2 Direction
    {
        get { return direction; }
        set { direction = value; }
    }

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        alive = false;
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

        if (alive)
        {
            aliveTime += Time.deltaTime;

            if (aliveTime >= upTime)
            {
                alive = false;
                Destroy(gameObject);
            }

            if (Physics2D.OverlapCircle(transform.position, 0.05f, LayerMask.GetMask("Floor", "Walls")))
            {
                Destroy(gameObject);
            }

            Collider2D col;
            if (col = Physics2D.OverlapCircle(transform.position, 0.05f, LayerMask.GetMask("Interactable")))
            {
                if (col.GetComponent<Plant>())
                {
                    col.GetComponent<Plant>().Grow();
                    col.GetComponent<SpriteRenderer>().color = Color.green;
                }

                Destroy(gameObject);
            }

        }

    }

    public void Fire()
    {
        alive = true;
        body.velocity = new Vector2(projSpeed * direction.x, 0);
        
    }

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
}
