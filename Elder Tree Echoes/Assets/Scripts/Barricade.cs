using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Barricade : MonoBehaviour
{
    public CameraFollow mainCam;
    bool Unlocked { get; set; }

    private Vector3 camVelocity = Vector3.zero;

    private Vector3 offset = new Vector3(0, 2f, -10f);

    public void Open()
    {
        Debug.Log("opening...");
        StartCoroutine(MoveCamera());
    }

    public IEnumerator MoveCamera()
    {
        //Debug.Log("hello there");
        mainCam.enabled = false;
        
        while (Camera.main.transform.position.x <= transform.position.x && Camera.main.transform.position.y <= transform.position.y)
        {
            Debug.Log(Camera.main.transform.position);
            Vector3 targetPosition = transform.position + offset;
            //Debug.Log("Moving camera...");
            Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, targetPosition, ref camVelocity, 0.5f);
            yield return new WaitForFixedUpdate();
        }

        //mainCam.transform.position = transform.position;

        yield return new WaitForSeconds(1);
        transform.localScale = Vector3.zero;

        yield return new WaitForSeconds(1.5f);

        mainCam.enabled = true;
        Destroy(gameObject);
    }
}
