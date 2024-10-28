using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Plant : MonoBehaviour
{
    [SerializeField]
    protected string plantName;

    [SerializeField]
    protected float growthSpeed;

    [SerializeField]
    protected Vector2 growth;

    [SerializeField] float upTime;

    public bool IsGrowing { get; set; }

    public bool IsGrown{ get; set; }

    private void FixedUpdate()
    {
        if (IsGrowing)
        {
            Grow();
        }
        else
        {
            IsGrowing = false;
        }
    }

    public void Grow()
    {

        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y + growthSpeed * Time.deltaTime, transform.localScale.z);
        /*SpriteRenderer sr = GetComponent<SpriteRenderer>();
        
        sr.color = new Vector4(sr.color.r, sr.color.g, sr.color.b, Mathf.Clamp((sr.color.a - 1f * Time.deltaTime), 0f, 1f));*/

        if (transform.localScale.x >= growth.x && transform.localScale.y >= growth.y)
        {
            //GetComponent<SpriteRenderer>().color = Color.green;

            IsGrowing = false;
            IsGrown = true;
            

            // Limiting the bounce
            if (!GetComponent<Trampoline>() && tag == "Bouncy")
            {
                gameObject.AddComponent(typeof(Trampoline));
            }

            //Debug.Log(GetComponent<Rope>().isGrown);
            if (tag == "Crank" && !GetComponent<Rope>().isGrown)
            {
                GetComponent<Rope>().GenerateRope();
            }
            else if (tag == "Crank" && GetComponent<Rope>().isGrown)
            {
                //Debug.Log("hey");
                GetComponentInChildren<Crank>().Rotate(1);
            }
        }
    }

}
