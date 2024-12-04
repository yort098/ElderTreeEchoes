using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingEnemy : EnemyScript
{

    private float jumpForce;

    [SerializeField]
    LayerMask groundLayer;

    protected override void Awake()
    {
        base.Awake();
        body = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
        script = player.GetComponent<PlayerController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        jumpForce = 8f;
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapBox(body.position, new Vector2(0.1f, 0.4f), 0, groundLayer);
    }

    protected override void Move()
    {
        if (IsGrounded())
        {
            body.velocity = new Vector2(0, body.velocity.y);
        }
        distance = Vector2.Distance(body.transform.position, player.transform.position);
        if (IsGrounded() && distance <= 4)
        {
            if (player.transform.position.x < body.position.x)
            {
                body.AddForce(new Vector2((attributes.speed * -1), jumpForce), ForceMode2D.Impulse);
            }
            else
            {
                body.AddForce(new Vector2(attributes.speed, jumpForce), ForceMode2D.Impulse);
            }
        }
    }
}
