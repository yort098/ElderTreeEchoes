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

    GameObject textInstance;

    protected override void Activate()
    {
        textInstance = Instantiate(textBox);
        textInstance.GetComponent<Text>().text = text;

        textInstance.gameObject.transform.SetParent(canvas.transform, false);
        base.Activate();
    }

    protected override void Deactivate()
    {
        Destroy(textInstance);

        base.Deactivate();
    }
}
