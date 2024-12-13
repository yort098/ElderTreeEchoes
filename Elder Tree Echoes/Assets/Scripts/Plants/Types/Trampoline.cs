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
        IsGrown = true;

        currForce = springForce;
           
    }

    public override void Shrink()
    {
        IsGrown = false;

        currForce = 0;
    }
}