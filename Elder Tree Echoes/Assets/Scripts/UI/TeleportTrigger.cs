using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportTrigger : PointTrigger
{
    [SerializeField]
    Vector3 teleportPos;

    protected override void Activate()
    {
        GameObject.Find("Player").transform.position = teleportPos;
        base.Activate();
    }

    protected override void Deactivate()
    {
        base.Deactivate();
    }
}
