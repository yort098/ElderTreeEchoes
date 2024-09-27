using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script for a basic enemy that just walks back and forth for now
public class EnemyScript : MonoBehaviour
{
    float speed;
    private Rigidbody2D body;
    private GameObject player;
    private PlayerController script;

    // Start is called before the first frame update
    void Start()
    {
        speed = 2f;   
        body = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
        script = player.GetComponent<PlayerController>();
    }

    //Handle collision between the player and the enemy
    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            //Move the player to the right when colliding
            //Refine this later
            script.Direction = new Vector2(1, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        body.velocity = new Vector3(speed, body.velocity.y);
        //Have the enemy move towards the player
        if(player.transform.position.x < body.position.x)
        {
            speed = -2f;
        }
        else
        {
            speed = 2f;
        }
    }
}
