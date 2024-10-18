using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    public float springForce = 10;

    void OnCollisionEnter2D(Collision2D coll)
    {
        Debug.Log("boing");
        if (coll.gameObject.tag == "Player") // Since people didn't like enemies bouncing
        {
            coll.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, springForce), ForceMode2D.Impulse);
        }
        
    }
}