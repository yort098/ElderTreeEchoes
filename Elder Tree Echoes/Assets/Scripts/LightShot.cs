using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LightShot : MonoBehaviour
{
    private Vector2 location;
    private Vector2 velocity;
    private bool isFacingRight;
    private int damage;

    // Start is called before the first frame update
    void Start()
    {
        location = transform.position;
        velocity = new Vector2(10.0f, 0);
        if (!isFacingRight)
        {
            velocity *= -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        location += velocity * Time.deltaTime;
        transform.position = location;
    }

    public void setDirection(bool isFacingRight)
    {
        this.isFacingRight = isFacingRight;
    }
}
