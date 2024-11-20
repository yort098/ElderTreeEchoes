using System.Collections;
using UnityEngine;

public class RopeMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    private HingeJoint2D hj;

    public Vector2 pushForce = new Vector2(8f, 3f);
    public Vector2 topForce = new Vector2(8f, 3f);

    public bool attached = false;

    public Transform attatchedTo;
    public GameObject disregard;

    public float slideSpeed = 5;

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
        attached = true;
        attatchedTo = ropeSeg.gameObject.transform.parent;
    }

    public void Detatch()
    {
        hj.connectedBody.gameObject.GetComponent<RopeSegment>().isPlayerAttatched = false;
        rb.gravityScale = GetComponent<PlayerController>().MovementData.gravityScale;
        attached = false;

        hj.enabled = false;
        hj.connectedBody = null;

        disregard = attatchedTo.gameObject;
        attatchedTo = null;

        StartCoroutine(DetatchCooldown());
    }

    private GameObject GetNewSegment(RopeSegment myConnection, int direction)
    {
        if (direction > 0)
        {
            return myConnection.direction > 0 ? myConnection.connectedBelow : myConnection.connectedAbove;
        }
        else if (direction < 0)
        {
            return myConnection.direction > 0 ? myConnection.connectedAbove : myConnection.connectedBelow;
        }
        return null;
    }


    public IEnumerator SmoothSlide(Transform targetSegment)
    {
        Vector2 startPos = transform.position;
        Vector2 endPos = targetSegment.position;
        float elapsedTime = 0f;

        while (elapsedTime < 1f / slideSpeed)
        {
            transform.position = Vector2.Lerp(startPos, endPos, elapsedTime * slideSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;
    }

    public void Slide(int direction)
    {
        RopeSegment myConnection = hj.connectedBody.gameObject.GetComponent<RopeSegment>();
        GameObject newSeg = GetNewSegment(myConnection, direction);

        if (newSeg != null)
        {
            StartCoroutine(SmoothSlide(newSeg.transform));
            myConnection.isPlayerAttatched = false;
            newSeg.GetComponent<RopeSegment>().isPlayerAttatched = true;
            hj.connectedBody = newSeg.GetComponent<Rigidbody2D>();
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log(attached);
        if (!attached && col.CompareTag("Rope"))
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

    public IEnumerator DetatchCooldown()
    {
        yield return new WaitForSeconds(0.3f);
        disregard = null;
    }
}