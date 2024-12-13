using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LeafPlatform : MonoBehaviour
{
    // Start is called before the first frame update
    Collider2D col;
    SpriteRenderer sR;
    private float lightStrengthValue = 0;
    private bool isGrown = false;
    private bool weakBranch = false;

    public float LightStrengthValue
    {
        get { return lightStrengthValue; }
        set { lightStrengthValue = value; }
    }

    public bool IsGrown
    {
        get { return isGrown; }
    }

    private void Awake()
    {
        col = this.GetComponent<Collider2D>();
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
            weakBranch = false;

            if (lightStrengthValue <= 70)
            {
                weakBranch = true;
            }

            col.excludeLayers = LayerMask.GetMask("Nothing");
            isGrown = true;

            if (GetComponent<Trampoline>())
            {
                GetComponent<Trampoline>().Grow();
            }

        }
        else if (lightStrengthValue < 65)
        {
            col.excludeLayers = LayerMask.GetMask("Player", "Enemy");
            isGrown = false;
            weakBranch = false;

            if (GetComponent<Trampoline>())
            {
                GetComponent<Trampoline>().Shrink();
            }
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

        if (isGrown && !weakBranch)
        {
            sR.color = new Color(0.0f, 0.6f, 0.3f, 0.3f + lightStrengthValue * 0.007f);
        }
        else if (isGrown && weakBranch)
        {
            sR.color = new Color(0.5f, 0.05f, 0.05f, 0.0f + lightStrengthValue * 0.01f);
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

