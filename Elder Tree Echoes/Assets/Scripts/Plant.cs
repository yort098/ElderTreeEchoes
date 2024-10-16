using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    [SerializeField]
    private string plantName;

    [SerializeField]
    private float growthThreshold;

    [SerializeField]
    private float growthTime;

    [SerializeField]
    private float growthSpeed;

    private bool isGrown;

    public void Grow()
    {
        if (!isGrown)
        {
            transform.localScale = Vector3.one;

            // Resetting the collider
            Destroy(GetComponent<PolygonCollider2D>());
            gameObject.AddComponent(typeof(PolygonCollider2D));

            // Limiting the bounce
            if (!GetComponent<Trampoline>())
            {
                gameObject.AddComponent(typeof(Trampoline));
            }
            
            
        }
    }

}
