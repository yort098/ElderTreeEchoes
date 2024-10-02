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

    PhysicsMaterial2D material;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Grow()
    {
        if (!isGrown)
        {
            Debug.Log("Growing plant");
            transform.localScale = Vector3.one;
            Destroy(GetComponent<PolygonCollider2D>());
            gameObject.AddComponent(typeof(PolygonCollider2D));

            gameObject.AddComponent(typeof(Trampoline));
            
        }
    }

}
