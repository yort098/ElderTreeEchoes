using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script for a basic enemy that just walks back and forth for now
public class EnemyScript : MonoBehaviour, IDamageable
{
    [SerializeField]
    protected EnemyAttributes attributes;

    protected Vector2 direction = Vector2.right;
    protected Rigidbody2D body;
    protected GameObject player;
    protected PlayerController script;
    public float distance;
    protected SpriteRenderer spriteRenderer;

    protected float startX;
    protected float endX;

    public Vector2 Direction { get { return direction; } }

    [field: SerializeField] public float MaxHealth { get; set; }
    public float CurrentHealth { get; set; }

    protected virtual void Awake()
    {
        player = GameObject.Find("Player");
        body = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {  
        //attributes.startX = body.position.x;
        //attributes.endX = body.position.x + 1;
        startX = body.position.x;
        endX = body.position.x + 3;

        CurrentHealth = MaxHealth;
    }

    //Handle collision between the player and the enemy
    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            //Move the player to the right when colliding
            //Refine this later

            GameManager.Instance.Damage(attributes.damage);
            
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    protected virtual void Move()
    {
        distance = Vector2.Distance(body.transform.position, player.transform.position);
        body.velocity = new Vector3(direction.x * attributes.speed, body.velocity.y);
        //Have the enemy move towards the player if in range
        if (distance <= 4)
        {
            if (player.transform.position.x < body.position.x)
            {
                direction = Vector2.left;
            }
            else
            {
                direction = Vector2.right;
            }
        }
        //Have the enemy patrol if the pklayer is not in range
        else if (body.position.x > endX)
        {
            direction = Vector2.left;
        }
        else if (body.position.x < startX)
        {
            direction = Vector2.right;
        }
    }

    public void Damage(float amount)
    {
        CurrentHealth -= amount;

        if (CurrentHealth <= 0)
        {
            Die();
        }

        // Show enemy damage (temp until knockback or other damage feedback is implemented)
        if (CurrentHealth < 5 && CurrentHealth >= 3)
        {
            spriteRenderer.color = Color.yellow;
        }
        else if (CurrentHealth < 3)
        {
            spriteRenderer.color = Color.red;
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
