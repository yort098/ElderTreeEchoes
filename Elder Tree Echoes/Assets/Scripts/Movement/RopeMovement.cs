using System.Collections;
using UnityEngine;

public class RopeMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    private HingeJoint2D hj;

    public Vector2 pushForce = new Vector2(8f, 3f);

    public bool attatched = false;

    public Transform attatchedTo;
    public GameObject disregard;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        hj = GetComponent<HingeJoint2D>();
    }

    public void Attatch(Rigidbody2D ropeSeg)
    {
        ropeSeg.gameObject.GetComponent<RopeSegment>().isPlayerAttatched = true;
        rb.gravityScale = 0;

        hj.connectedBody = ropeSeg;
        hj.enabled = true;
        attatched = true;
        attatchedTo = ropeSeg.gameObject.transform.parent;
    }

    public void Detatch()
    {
        hj.connectedBody.gameObject.GetComponent<RopeSegment>().isPlayerAttatched = false;
        rb.gravityScale = GetComponent<PlayerController>().MovementData.gravityScale;
        attatched = false;

        hj.enabled = false;
        hj.connectedBody = null;

        disregard = attatchedTo.gameObject;
        attatchedTo = null;

        StartCoroutine(Time());
    }

    public void Slide(int direction)
    {
        RopeSegment myConnection = hj.connectedBody.gameObject.GetComponent<RopeSegment>();
        GameObject newSeg = null;

        if (direction > 0 && myConnection.direction > 0) // upward rope and upward movement
        {
            if (myConnection.connectedBelow != null)
            {
                newSeg = myConnection.connectedBelow;
            }

        }
        else if (direction > 0 && myConnection.direction < 0) // upward movement, downward rope
        {
            Debug.Log("up");
            if (myConnection.connectedAbove != null)
            {
                if (myConnection.connectedAbove.gameObject.GetComponent<RopeSegment>() != null)
                {
                    //Debug.Log(myConnection);
                    newSeg = myConnection.connectedAbove;
                }
            }
        }

        if (direction < 0 & myConnection.direction > 0) // downward movement, upwards rope
        {
            
            if (myConnection.connectedAbove != null)
            {
                if (myConnection.connectedAbove.gameObject.GetComponent<RopeSegment>() != null)
                {
                    //Debug.Log(myConnection);
                    newSeg = myConnection.connectedAbove;
                }
            }

        }
        else if (direction < 0 && myConnection.direction < 0) // downards movement, downwards rope
        {
            Debug.Log("down");
            if (myConnection.connectedBelow != null)
            {
                newSeg = myConnection.connectedBelow;
            }
        }

        if (newSeg != null)
        {
            transform.position = new Vector2(Mathf.Lerp(transform.position.x, newSeg.transform.position.x, 0.5f), Mathf.Lerp(transform.position.y, newSeg.transform.position.y, 0.5f));

            //transform.position = new Vector3(newSeg.transform.position.x, 0, transform.position.z); ; // change this to lerp for smoother movement
            myConnection.isPlayerAttatched = false;
            newSeg.GetComponent<RopeSegment>().isPlayerAttatched = true;
            hj.connectedBody = newSeg.GetComponent<Rigidbody2D>();
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log(attatched);
        if (!attatched)
        {
            if (col.tag == "Rope")
            {
                if (attatchedTo != col.gameObject.transform.parent)
                {
                    if (disregard == null || col.gameObject.transform.parent.gameObject != disregard)
                    {
                        Attatch(col.gameObject.GetComponent<Rigidbody2D>());
                    }
                }
            }
        }
    }

    public IEnumerator Time()
    {
        yield return new WaitForSeconds(0.3f);
        disregard = null;
    }
}