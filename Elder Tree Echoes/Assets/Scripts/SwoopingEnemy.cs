using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwoopingEnemy : EnemyScript
{
    private float startY;
    private float endY;
    private bool isSwooping;


    protected override void Awake()
    {
        base.Awake();
        startY = body.position.y;
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        endY = startY + 2;
        isSwooping = false;
    }

    protected override void Move()
    {
        endY = player.transform.position.y + 2;
        distance = Vector2.Distance(body.transform.position, player.transform.position);
        body.velocity = new Vector3(direction.x * attributes.speed, direction.y * attributes.speed);
        //Have the enemy move towards the player if they are in range
        if (distance <= 4 && !isSwooping)
        {
            isSwooping = true;
            direction.y = -1;
            if (player.transform.position.x < body.position.x)
            {
                direction.x = -1;
            }
            else
            {
                direction.x = 1;
            }
        }
        else if (isSwooping)
        {
            if (body.position.y < endY)
            {
                direction.y = 1;
            }
            else if (body.position.y > startY + 0.5)
            {
                isSwooping = false;
            }
        }
        //Set the y value of direction before the move towards player codce changes it.
        else if (body.position.y < startY)
        {
            direction.y = 1;
        }
        else if (body.position.y > startY)
        {
            direction.y = 0;
        }
        //If the enemy is not in range, have it patrol
        if (body.position.x > endX && !isSwooping)
        {
            direction.x = -1;
        }
        else if (body.position.x < startX && !isSwooping)
        {
            direction.x = 1; ;
        }
    }
}
