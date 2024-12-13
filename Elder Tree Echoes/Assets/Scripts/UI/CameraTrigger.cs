using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    [SerializeField] CameraFollow mainCam;
    [SerializeField] Vector3 camLoc;

    private Vector3 camVelocity = Vector3.zero;

    private Vector3 offset = new Vector3(0, 2f, -10f);

    public void Activate()
    {
        StartCoroutine(MoveCamera());
    }

    private IEnumerator MoveCamera()
    {
        mainCam.enabled = false;


        while (Camera.main.transform.position.x >= camLoc.x && Camera.main.transform.position.y <= camLoc.y)
        {
            Vector3 targetPosition = camLoc + offset;
            //Debug.Log("Moving camera...");
            Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, targetPosition, ref camVelocity, 0.5f);
            Debug.Log("still trying");
            yield return new WaitForFixedUpdate();
        }

        //mainCam.transform.position = transform.position;

        yield return new WaitForSeconds(1.5f);

        mainCam.enabled = true;
    }
}
