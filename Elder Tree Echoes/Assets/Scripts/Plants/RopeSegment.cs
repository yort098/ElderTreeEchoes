using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeSegment : MonoBehaviour
{
    public GameObject connectedAbove, connectedBelow;

    public bool isPlayerAttatched;
    public int direction;

    // Start is called before the first frame update
    void Start()
    {
        connectedAbove = GetComponent<HingeJoint2D>().connectedBody.gameObject;
        RopeSegment aboveSegment = connectedAbove.GetComponent<RopeSegment>();

        if (aboveSegment != null)
        {
            aboveSegment.connectedBelow = gameObject;

            Debug.Log(direction);
            if (direction < 0)
            {
               
                float spriteBottom = connectedAbove.GetComponent<SpriteRenderer>().bounds.size.y;
                GetComponent<HingeJoint2D>().connectedAnchor = new Vector2(0, spriteBottom * -1);
            }

            else
            {
                Debug.Log(connectedAbove.GetComponent<SpriteRenderer>().bounds.size.y);
                float spriteTop = connectedAbove.GetComponent<SpriteRenderer>().bounds.size.y;
                GetComponent<HingeJoint2D>().connectedAnchor = new Vector2(0, spriteTop);
            }


        }
        else
        {
            GetComponent<HingeJoint2D>().connectedAnchor = new Vector2(0, GetComponent<SpriteRenderer>().bounds.size.y/2);
        }    
    }

    public void ResetAnchor()
    {
        connectedAbove = GetComponent<HingeJoint2D>().connectedBody.gameObject;
        RopeSegment aboveSegment = connectedAbove.GetComponent<RopeSegment>();

        if (aboveSegment != null)
        {
            aboveSegment.connectedBelow = gameObject;
            float spriteBottom = connectedAbove.GetComponent<SpriteRenderer>().bounds.size.y;
            GetComponent<HingeJoint2D>().connectedAnchor = new Vector2(0, spriteBottom * -1);
        }
        else
        {
            GetComponent<HingeJoint2D>().connectedAnchor = Vector2.zero;
        }
    }
}
