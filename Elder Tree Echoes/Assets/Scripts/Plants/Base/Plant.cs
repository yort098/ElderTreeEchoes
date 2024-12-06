using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Plant : MonoBehaviour
{
    [SerializeField]
    protected string plantName;

    [SerializeField]
    protected float growthSpeed;

    [SerializeField]
    protected Vector2 growth;

    [SerializeField] float upTime;

    [SerializeField] protected bool startsGrown;

    public bool IsGrowing { get; set; }

    public bool IsGrown{ get; set; }

    protected virtual void Start()
    {
        IsGrown = false;
        IsGrowing = false;

        if (startsGrown)
        {
            Grow();
        }
    }

    private void FixedUpdate()
    {
        if (IsGrown)
        {
            IsGrowing = false; 
            return;
        }

        if (IsGrowing)
        {
            Grow();
        }
       
    }

    public virtual void Grow(){}

}
