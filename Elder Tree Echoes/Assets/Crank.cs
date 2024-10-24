using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crank : MonoBehaviour
{
    public float rotateSpeed = 10f;
    private Rope rope;
    private int numSegments;
    public int maxLinks = 15;

    private void Awake()
    {
        rope = transform.parent.GetComponent<Rope>();
        numSegments = rope.numSegments;
    }

    public void Rotate(int direction)
    {
        if (direction > 0 && rope != null && numSegments <= maxLinks)
        {
            transform.Rotate(0, 0, direction * rotateSpeed);
            rope.AddSegment();
            numSegments++;
        }
        else if (direction < 0 && rope != null && numSegments > 1)
        {
            transform.Rotate(0, 0, direction * rotateSpeed);
            rope.RemoveSegment();
            numSegments--;
        }
    }
}
