using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportTrigger : PointTrigger
{
    [SerializeField]
    Vector3 teleportPos;

    private void Start()
    {
        enabled = false;
    }

    public override void Activate()
    {
        if (enabled)
        {
            GameObject.Find("Player").transform.position = teleportPos;
            enabled = false;
            base.Activate();
        }
        
    }

    public override void Deactivate()
    {
        base.Deactivate();
    }
}
