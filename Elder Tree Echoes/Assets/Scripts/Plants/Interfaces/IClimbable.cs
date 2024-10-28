using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public interface IClimbable
{
    bool StandingInFront { get; set; }

    void Climb();

    void Exit();
}
