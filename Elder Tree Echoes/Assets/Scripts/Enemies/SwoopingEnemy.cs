using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwoopingEnemy : EnemyScript
{
    private float startY;
    private float endY;

    protected override void Awake()
    {
        base.Awake();
        script = player.GetComponent<PlayerController>();
    }
    private bool isSwooping;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        endX = startX + 3;
        hardEnd1 = -100;
        hardEnd2 = 100;
        startY = body.position.y;
        endY = startY + 2;
        isSwooping = false;
    }

    protected override void Move()
    {
        distance = Vector2.Distance(body.transform.position, player.transform.position);
        body.velocity = new Vector3(direction.x * attributes.speed, direction.y * attributes.speed);
        if (body.position.y > startY)
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
        //Have the enemy move towards the player if they are in range
        else if (distance <= 4 && !isSwooping)
        {
            isSwooping = true;
            direction.y = -1;
            endY = player.transform.position.y + 1;
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
            else if (body.position.y >= startY)
            {
                isSwooping = false;
            }
        }
        //Set the y value of direction before the move towards player codce changes it.
        else if (body.position.y < startY)
        {
            direction.y = 1;
        }
    }

    //public void TakeDamage(int damageAmount)
    //{
    //    health -= damageAmount;

    //    if (health <= 0)
    //    {
    //        Destroy(gameObject);
    //    }

    //    // Show enemy damage (temp until knockback or other damage feedback is implemented)
    //    if (health < 5 && health >= 3)
    //    {
    //        spriteRenderer.color = Color.yellow;
    //    }
    //    else if (health < 3)
    //    {
    //        spriteRenderer.color = Color.red;
    //    }
    //}
}
