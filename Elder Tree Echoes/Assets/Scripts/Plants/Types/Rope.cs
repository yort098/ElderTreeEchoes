using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : Plant
{
    public Rigidbody2D hook;
    public GameObject prefabRopeSeg; // Change this to an array if we have multiple sprites
    [SerializeField] public int numSegments = 1;
    private int currentNumSegments = 0;

    public HingeJoint2D top;

    public PlayerController player;

    public int direction;

    private float upTime = 10;
    private float upTimeCounter = 0;

    private void Awake()
    {
        upTimeCounter = upTime;
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (IsGrown)
        {
            upTimeCounter -= Time.deltaTime;

            if (upTimeCounter <= 0)
            {
                Shrink();
                upTimeCounter = upTime;
            }
        }
    }

    public void GenerateRope()
    {
        Rigidbody2D prevBody = hook;
        for (int i = 0; i < numSegments; i++)
        {
            GameObject newSeg = Instantiate(prefabRopeSeg);
            newSeg.transform.parent = transform;
            newSeg.transform.position = transform.position;

            HingeJoint2D hj = newSeg.GetComponent<HingeJoint2D>();
            hj.connectedBody = prevBody;

            prevBody = newSeg.GetComponent<Rigidbody2D>();

            if (i == 0)
            {
                top = hj;
            }
        }

        IsGrown = true;
        currentNumSegments = numSegments;
    }

    public void RemoveSegment()
    {
        if (top.gameObject.GetComponent<RopeSegment>().isPlayerAttatched)
        {
            top.gameObject.GetComponent<RopeSegment>().isPlayerAttatched = false;
            player.GetComponent<RopeMovement>().Detatch();
        }

        Debug.Log("current number of segments: " + currentNumSegments);

        if (currentNumSegments == 1)
        {
            Destroy(top.gameObject);
        }
        else
        {
            HingeJoint2D newTop = top.gameObject.GetComponent<RopeSegment>().connectedBelow.GetComponent<HingeJoint2D>();
            newTop.connectedBody = hook;
            newTop.gameObject.transform.position = hook.gameObject.transform.position;
            newTop.GetComponent<RopeSegment>().ResetAnchor();
            Destroy(top.gameObject);
            top = newTop;
        }
        
        currentNumSegments--;
        
    }

    public override void Grow()
    {
        Debug.Log("growing rope");
        GenerateRope();

        IsGrown = true;
    }

    public override void Shrink()
    {
        StartCoroutine(DestroyRope());
    }

    private IEnumerator DestroyRope()
    {
        while (currentNumSegments > 0)
        {
            RemoveSegment();
            yield return new WaitForSeconds(0.1f);
        }

        IsGrown = false;
    }
}
