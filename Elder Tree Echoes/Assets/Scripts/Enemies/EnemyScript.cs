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

    [SerializeField]
    public float startX;

    [SerializeField]
    public float endX;

    [SerializeField]
    public float hardEnd1;

    [SerializeField]
    public float hardEnd2;

    protected float damageTransparency = 1.0f;

    public Vector2 Direction { get { return direction; } }

    [field: SerializeField] public float MaxHealth { get; set; }
    public float CurrentHealth { get; set; }

    protected virtual void Awake()
    {
        player = GameObject.Find("Player");
        body = GetComponent<Rigidbody2D>();
        startX = body.position.x;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {  
        //attributes.startX = body.position.x;
        //attributes.endX = body.position.x + 1;

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
        spriteRenderer.color = new Color(1, 1, 1, damageTransparency);
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

            if(body.position.x < hardEnd1 || body.position.x > hardEnd2)
            {
                direction = Vector2.zero;
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
        damageTransparency -= 0.2f;

        if (CurrentHealth <= 0)
        {
            Die();
        }

        // Show enemy damage (temp until knockback or other damage feedback is implemented)
        //if (CurrentHealth < 5 && CurrentHealth >= 3)
        //{
        //    spriteRenderer.color = Color.yellow;
        //}
        //else if (CurrentHealth < 3)
        //{
        //    spriteRenderer.color = Color.red;
        //}
 
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
