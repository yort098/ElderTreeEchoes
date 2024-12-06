using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TextTrigger : PointTrigger
{
    [SerializeField] Canvas canvas;
    [SerializeField] GameObject textBox;
    [SerializeField] string text;

    private bool seen = false;

    GameObject textInstance;

    public override void Activate()
    {
        if (!seen)
        {
            textInstance = Instantiate(textBox);
            textInstance.GetComponent<Text>().text = text;
            textInstance.gameObject.transform.SetParent(canvas.transform, false);
        }
        
        if (GetComponent<TeleportTrigger>())
        {
            GameObject.Find("Player").GetComponent<PlayerController>().isDoor = true;
            GameObject.Find("Player").GetComponent<PlayerController>().currentDoor = this.gameObject;
        }

        if (GetComponent<Shrine>())
        {

        }

        base.Activate();
    }

    public override void Deactivate()
    {
        Destroy(textInstance);
        seen = true;

        if (GetComponent<TeleportTrigger>())
        {
            GameObject.Find("Player").GetComponent<PlayerController>().isDoor = false;
            GameObject.Find("Player").GetComponent<PlayerController>().currentDoor = null;

        }

        base.Deactivate();
    }
}
