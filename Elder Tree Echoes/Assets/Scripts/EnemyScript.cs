using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script for a basic enemy that just walks back and forth for now
public class EnemyScript : MonoBehaviour
{
    [SerializeField]
    float speed = 2f;

    private Vector2 direction = Vector2.zero;
    private Rigidbody2D body;
    private GameObject player;
    private PlayerController script;



    private float damage = 5f;

    public Vector2 Direction { get { return direction; } }

    // Start is called before the first frame update
    void Start()
    {  
        body = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
        script = player.GetComponent<PlayerController>();
    }

    //Handle collision between the player and the enemy
    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Player" && !GameManager.Instance.Invincible)
        {
            //Move the player to the right when colliding
            //Refine this later
            //script.Direction = new Vector2(1, 0);

            GameManager.Instance.TakeDamage(damage);
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        body.velocity = new Vector3(direction.x * speed, body.velocity.y);
        //Have the enemy move towards the player
        if(player.transform.position.x < body.position.x)
        {
            direction = Vector2.left;
        }
        else
        {
            direction = Vector2.right;
        }
    }
}
