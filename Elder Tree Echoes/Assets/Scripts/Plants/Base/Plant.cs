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

    public bool IsGrowing { get; set; }

    public bool IsGrown{ get; set; }

    private void Start()
    {
        IsGrown = false;
        IsGrowing = false;
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
