using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Checkpoint : Plant
{
   // [SerializeField] Text instructions;

    public override void Grow()
    {
        SpriteRenderer[] srs = GetComponentsInChildren<SpriteRenderer>();

        for (int i = 1; i < srs.Length; i++)
        {
            srs[i].color = Color.green;
        }

        GameManager.Instance.Progress();

        IsGrown = true;
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameManager.Instance.currLevel == 1 && GameManager.Instance.checkpoints == 0)
        {
            //instructions.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //instructions.gameObject.SetActive(false);
    }
}
