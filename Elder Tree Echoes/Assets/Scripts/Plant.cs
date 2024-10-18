using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    [SerializeField]
    protected string plantName;

    public bool isGrowing;
    protected bool isGrown;

    [SerializeField]
    protected float growthSpeed;

    [SerializeField]
    protected Vector2 growth;

    private void FixedUpdate()
    {
        if (isGrowing && !isGrown)
        {
            Grow();
        }
        else
        {
            isGrowing = false;
        }
    }

    public void Grow()
    {

        transform.localScale = new Vector3(1, transform.localScale.y + growthSpeed * Time.deltaTime, 1);
        /*SpriteRenderer sr = GetComponent<SpriteRenderer>();
        
        sr.color = new Vector4(sr.color.r, sr.color.g, sr.color.b, Mathf.Clamp((sr.color.a - 1f * Time.deltaTime), 0f, 1f));*/

        if (transform.localScale.x >= growth.x && transform.localScale.y >= growth.y)
        {
            GetComponent<SpriteRenderer>().color = Color.green;

            isGrowing = false;
            isGrown = true;
            

            // Limiting the bounce
            if (!GetComponent<Trampoline>())
            {
                //gameObject.AddComponent(typeof(Trampoline));
            }
        }
    }

}
