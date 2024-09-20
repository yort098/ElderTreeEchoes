using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script for a basic enemy that just walks back and forth for now
public class EnemyScript : MonoBehaviour
{
    float firstEnd;
    float lastEnd;
    float speed;
    private Rigidbody2D body;

    // Start is called before the first frame update
    void Start()
    {
        firstEnd = 0f;
        lastEnd = 1f;
        speed = 1f;   
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        body.velocity = new Vector3(speed, body.velocity.y);
        if ((speed > 0f && body.position.x > lastEnd) || (speed < 0f && body.position.x < firstEnd))
        {
            speed *= -1f;
        }
    }
}
