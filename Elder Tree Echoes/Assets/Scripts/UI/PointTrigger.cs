using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PointTrigger : MonoBehaviour
{
    protected bool activated;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!activated)
        {
            Activate();
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (activated)
        {
            Deactivate();
        }    
    }

    protected virtual void Activate()
    {
        activated = true;
    }

    protected virtual void Deactivate()
    {
        activated = false;
    }

}
