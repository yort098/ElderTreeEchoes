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


        while (Vector3.Distance(Camera.main.transform.position, camLoc + offset) > 0.01)
        {
            Vector3 targetPosition = camLoc + offset;
            Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, targetPosition, ref camVelocity, 0.5f);
            yield return new WaitForFixedUpdate();
        }

        Camera.main.transform.position = camLoc + offset; // Setting to the exact position

        yield return new WaitForSeconds(1.5f);

        mainCam.enabled = true;
    }
}
