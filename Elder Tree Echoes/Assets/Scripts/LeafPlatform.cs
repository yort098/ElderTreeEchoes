using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LeafPlatform : MonoBehaviour
{
    // Start is called before the first frame update
    BoxCollider2D collider;

    private void Awake()
    {
        collider = this.GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
