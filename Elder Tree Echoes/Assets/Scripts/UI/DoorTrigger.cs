using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : PointTrigger
{ 
    public Barricade barr;

    public override void Activate()
    {
        barr.Open();

        base.Activate();
    }

    public override void Deactivate()
    {
       // Do nothing
    }
}
