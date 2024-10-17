using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwoopingEnemy : MonoBehaviour
{
    [SerializeField]
    EnemyAttributes attributes;

    private Vector2 direction = Vector2.right;
    private Rigidbody2D body;
    private GameObject player;
    private PlayerController script;
    public float distance;
    private float startX;
    private float endX;
    private float startY;
    private float endY;

    private int health;


    private SpriteRenderer spriteRenderer;

    public Vector2 Direction { get { return direction; } }

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
        script = player.GetComponent<PlayerController>();
        spriteRenderer = player.GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {

        startX = body.position.x;
        endX = body.position.x + 3;
        startY = body.position.y;

        health = (int)attributes.health;
    }

    //Handle collision between the player and the enemy
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player" && !GameManager.Instance.Invincible)
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
        endY = player.transform.position.y + 2;
        distance = Vector2.Distance(body.transform.position, player.transform.position);
        body.velocity = new Vector3(direction.x * attributes.speed, direction.y * attributes.speed);
        //Set the y value of direction before the move towards player codce changes it.
        if (body.position.y < startY)
        {
            direction.y = 1;
        }
        else
        {
            direction.y = 0;
        }
        //Have the enemy move towards the player if they are in range
        if (distance <= 4)
        {
            if (player.transform.position.x < body.position.x)
            {
                direction.x = -1;
            }
            else
            {
                direction.x = 1;
            }
            
            //If it is on the same level as the player, stop it from going down, otherwise, have it go down.
            if(body.transform.position.y < endY)
            {
                direction.y = 1;
            }
            else
            {
                direction.y = -1;
            }
        }
        //If the enemy is not in range, have it patrol
        else if (body.position.x > endX)
        {
            direction.x = -1;
        }
        else if (body.position.x < startX)
        {
            direction.x = 1; ;
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
