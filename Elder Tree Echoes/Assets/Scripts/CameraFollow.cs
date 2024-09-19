using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //  How much to offset the camera from the object it's
    //  following
    private Vector3 offset = new Vector3(0, 2f, -10f);

    //  How long it takes for the camera
    //  to reach the object it is following   
    private float smoothTime = 0.15f; 

    private Vector3 velocity = Vector3.zero;

    // The object to center the camera around
    [SerializeField]
    private Transform target;

    // Update is called once per frame
    void FixedUpdate()
    {
        //  The position in the scene to place the camera
        Vector3 targetPosition = target.position + offset;

        //  Smoothly moves the camera each frame to position itself to the
        //  object its following
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
