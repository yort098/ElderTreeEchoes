using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shrine : MonoBehaviour
{
    [SerializeField] Power powerHeld;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Absorb()
    {
        if (powerHeld == Power.Water)
        {
            GameManager.Instance.hasWater = true;
        }

        if (powerHeld == Power.Light)
        {
            GameManager.Instance.hasLight = true;
        }
    }
}
