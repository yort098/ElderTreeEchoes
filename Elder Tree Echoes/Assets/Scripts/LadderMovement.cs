using UnityEngine;

public class LadderMovement : MonoBehaviour
{
    private float vertical;
    private float speed = 3f;

    Transform currentClimbable;

    public bool IsClimbing { get; set; }

    public bool IsLadder { get; set; }

    [SerializeField] private Rigidbody2D rb;
    private PlayerController pScript;

    private void Awake()
    {
        pScript = GetComponent<PlayerController>();
    }

    void Update()
    {
        vertical = pScript.Direction.y;

         //Debug.Log(IsLadder);
        if (IsLadder)
        {
            IsClimbing = true;
        }
        else
        {
            IsClimbing = false;
        }    
    }

    private void FixedUpdate()
    {
        if (IsClimbing)  
        {
            rb.gravityScale = 0f;
            rb.velocity = new Vector2(rb.velocity.x, vertical * speed);
            rb.transform.position = new Vector2(currentClimbable.position.x, rb.transform.position.y);
        }
        else
        {
            rb.gravityScale = pScript.MovementData.gravityScale;
            IsLadder = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Hello");
        if (collision.CompareTag("Climbable") && collision.GetComponent<Plant>().IsGrown)
        {
            IsLadder = true;
            currentClimbable = collision.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Goodbye");
        if (collision.CompareTag("Climbable"))
        {
            IsLadder = false;
            IsClimbing = false;
        }
    }
}