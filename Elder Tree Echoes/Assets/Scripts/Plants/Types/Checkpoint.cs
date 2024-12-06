using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Checkpoint : Plant
{
    [SerializeField] bool countsTowardProgress;

    protected override void Start()
    {
        base.Start();

        if (!countsTowardProgress)
        {
            GameManager.Instance.totalCheckpoints--;
        }
    }

    public override void Grow()
    {
        SpriteRenderer[] srs = GetComponentsInChildren<SpriteRenderer>();

        for (int i = 1; i < srs.Length; i++)
        {
            srs[i].color = Color.green;
        }

        if (countsTowardProgress)
        {
            GameManager.Instance.Progress();
        }
        

        IsGrown = true;
        
    }
}
