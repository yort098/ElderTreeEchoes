using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script for a basic enemy that just walks back and forth for now
public class EnemyScript : MonoBehaviour
{
    float speed;
    private Rigidbody2D body;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        speed = 2f;   
        body = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
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
