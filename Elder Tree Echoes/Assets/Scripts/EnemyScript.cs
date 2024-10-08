using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script for a basic enemy that just walks back and forth for now
public class EnemyScript : MonoBehaviour
{
    [SerializeField]
    EnemyAttributes attributes;

    private Vector2 direction = Vector2.right;
    private Rigidbody2D body;
    private GameObject player;
    private PlayerController script;
    public float distance;
    private SpriteRenderer spriteRenderer;
    private int health = 5;


    private float damage = 5f;
    private float startX;
    private float endX;

    public Vector2 Direction { get { return direction; } }

    // Start is called before the first frame update
    void Start()
    {  
        body = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
        script = player.GetComponent<PlayerController>();
        spriteRenderer = this.GetComponent<SpriteRenderer>();

        startX = body.position.x;
        endX = body.position.x + 3;
    }

    //Handle collision between the player and the enemy
    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Player" && !GameManager.Instance.Invincible)
        {
            //Move the player to the right when colliding
            //Refine this later
            //script.Direction = new Vector2(1, 0);

            GameManager.Instance.TakeDamage(attributes.damage, col);
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector2.Distance(body.transform.position, player.transform.position);
        body.velocity = new Vector3(direction.x * attributes.speed, body.velocity.y);
        //Have the enemy move towards the player if in range
        if(distance <= 4)
        {
            if(player.transform.position.x < body.position.x)
            {
                direction = Vector2.left;
            }
            else
            {
                direction = Vector2.right;
            }
        }
        //Have the enemy patrol if the pklayer is not in range
        else if(body.position.x > endX)
        {
            direction = Vector2.left;
        }
        else if(body.position.x < startX)
        {
            direction = Vector2.right;
        }
    }

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;

        if (health <= 0)
        {
            Destroy(gameObject);
        }

        // Show enemy damage (temp until knockback or other damage feedback is implemented)
        if (health < 5 && health >= 3)
        {
            spriteRenderer.color = Color.yellow;
        }
        else if (health < 3)
        {
            spriteRenderer.color = Color.red;
        }
    }
}
