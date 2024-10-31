using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : Plant
{
    public float springForce = 10;
    private float currForce = 0;

    public void OnCollisionEnter2D(Collision2D col)
    {        
        //if (coll.gameObject.)
        if (col.gameObject.CompareTag("Player") && IsGrown) // Stops enemies from bouncing
        {
            col.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, currForce), ForceMode2D.Impulse);
        }
        
    }

    public override void Grow()
    {
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y + growthSpeed * Time.deltaTime, transform.localScale.z);
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        
        sr.color = new Vector4(sr.color.r, sr.color.g, sr.color.b, Mathf.Clamp((sr.color.a - 1f * Time.deltaTime), 0f, 1f));

        if (transform.localScale.x >= growth.x && transform.localScale.y >= growth.y)
        {
            sr.color = Color.green;

            IsGrown = true;

            currForce = springForce;
            
        }
    }
}