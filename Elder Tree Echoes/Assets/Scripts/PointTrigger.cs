using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PointTrigger : MonoBehaviour
{
    protected bool activated;
    public Barricade barr;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!activated)
        {
            barr.Open();
            activated = true;
        }
        
    }

}
