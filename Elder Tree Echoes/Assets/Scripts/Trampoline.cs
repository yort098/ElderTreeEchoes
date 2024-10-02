using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    public float springForce = 500;
    private Collision2D collision;
    public GameObject subject;

    void OnCollisionEnter2D(Collision2D coll)
    {
        coll.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, springForce));
    }
}