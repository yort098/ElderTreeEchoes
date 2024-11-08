using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : PointTrigger
{ 
    public Barricade barr;

    protected override void Activate()
    {
        barr.Open();

        base.Activate();
    }

    protected override void Deactivate()
    {
       // Do nothing
    }
}
