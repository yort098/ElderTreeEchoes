using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LeafPlatform : MonoBehaviour
{
    // Start is called before the first frame update
    BoxCollider2D collider;
    SpriteRenderer sR;
    private float lightStrengthValue = 0;
    private bool isGrown = false;

    public float LightStrengthValue
    {
        get { return lightStrengthValue; }
        set { lightStrengthValue = value; }
    }

    private void Awake()
    {
        collider = this.GetComponent<BoxCollider2D>();
        sR = this.GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        sR.color = new Color(0.35f, 0.05f, 0.05f, 0.2f);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (lightStrengthValue >= 65)
        {
            collider.excludeLayers = LayerMask.GetMask("Nothing");
            isGrown = true;
        }
        else if (lightStrengthValue < 65)
        {
            collider.excludeLayers = LayerMask.GetMask("Player", "Enemy");
            isGrown = false;
        }

        if (lightStrengthValue > 100)
        {
            lightStrengthValue = 100;
        }

        lightStrengthValue -= 2.0f * Time.deltaTime;

        if (lightStrengthValue < 0)
        {
            lightStrengthValue = 0;
        }

        //Debug.Log(lightStrengthValue);

        if (isGrown)
        {
            sR.color = new Color(0.0f, 0.4f, 0.2f, 0.3f + lightStrengthValue * 0.007f);
        }
        else
        {
            // using grey for now, the brown looks to red
            sR.color = new Color(0.3f, 0.3f, 0.3f, 0.3f + lightStrengthValue * 0.009f);
        }

        // 0.35f, 0.05f, 0.05f, -> dead root brown
        // 0.4f, 0.5f, 0.1f, -> tree green
    }
}

