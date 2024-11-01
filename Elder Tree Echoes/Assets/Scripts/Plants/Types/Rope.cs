using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : Plant
{
    public Rigidbody2D hook;
    public GameObject prefabRopeSeg; // Change this to an array if we have multiple sprites
    [SerializeField] public int numSegments = 1;

    public HingeJoint2D top;

    public PlayerController player;

    public bool startsGrown = false;

    public int direction;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        if (startsGrown)
        {
            GenerateRope();
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
    }

    public void AddSegment()
    {
        GameObject newLink = Instantiate(prefabRopeSeg);
        newLink.transform.parent = transform;
        newLink.transform.position = transform.position;

        HingeJoint2D hj = newLink.GetComponent<HingeJoint2D>();
        hj.connectedBody = hook;

        newLink.GetComponent<RopeSegment>().connectedBelow = top.gameObject;
        top.connectedBody = newLink.GetComponent<Rigidbody2D>();
        top.GetComponent<RopeSegment>().ResetAnchor();
        top = hj;
    }

    public void RemoveSegment()
    {
        if (top.gameObject.GetComponent<RopeSegment>().isPlayerAttatched)
        {
            player.gameObject.GetComponent<RopeMovement>().Slide(-1);
        }

        HingeJoint2D newTop = top.gameObject.GetComponent<RopeSegment>().connectedBelow.GetComponent<HingeJoint2D>();
        newTop.connectedBody = hook;
        newTop.gameObject.transform.position = hook.gameObject.transform.position;
        newTop.GetComponent<RopeSegment>().ResetAnchor();
        Destroy(top.gameObject);
        top = newTop;
    }

    public override void Grow()
    {
        Debug.Log("growing rope");
        GenerateRope();

        IsGrown = true;
    }
}
