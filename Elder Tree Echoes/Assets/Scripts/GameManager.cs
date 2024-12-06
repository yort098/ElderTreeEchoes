using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour, IDamageable
{
    private static GameManager instance;

    private float invincibilityTime = 1.5f;
    private bool invincible = false;

    private Coroutine invincibility;

    GameObject player;

    public int currLevel = 1;
    private float currLevelPercent = 0;
    public int totalCheckpoints;
    public int checkpoints;


    public bool Invincible { get { return invincible; } }

    [SerializeField]
    Slider healthBar;

    [SerializeField]
    Slider waterMeter;

    [SerializeField]
    Slider lightMeter;

    [SerializeField]
    Text percentageDisplay;


    public GameObject[] Enemies
    {
        get { return GameObject.FindGameObjectsWithTag("Enemy"); }
    }

    /// <summary>
    /// Returns a reference to the game manager class
    /// </summary>
    public static GameManager Instance { get { return instance; } }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
            totalCheckpoints = GameObject.FindGameObjectsWithTag("Checkpoint").Length;
            
        }

        player = GameObject.Find("Player");
    }

    public float MaxHealth { get; set; }
    public float CurrentHealth { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        MaxHealth = 100;
        CurrentHealth = MaxHealth;
        healthBar.value = MaxHealth;

        waterMeter.value = 0;
        lightMeter.value = 0;

        percentageDisplay.text = "Tree Roots Restored: " + currLevelPercent + "%";
    }

    public IEnumerator InvincibilityTimer()
    {
        SpriteRenderer sp = player.GetComponent<SpriteRenderer>();
        Color[] colors = new Color[2] { Color.red, sp.color };
        invincible = true;

        float elapsedTime = 0;
        int index = 0;

        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), true);
        while (elapsedTime < invincibilityTime)
        {

            sp.color = colors[index % 2];
            elapsedTime += Time.deltaTime;
            index++;
            yield return null;
        }

        invincible = false;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);
        elapsedTime = 0;
        sp.color = colors[1];
        invincibility = null;

    }

    void EnablePlayerControls()
    {
        player.GetComponent<PlayerController>().CanMove = true;
    }

    

    // Update is called once per frame
    void Update()
    {
        waterMeter.value = PlayerAbilities.Instance.WaterEnergy;
        lightMeter.value = PlayerAbilities.Instance.LightEnergy;

        Debug.Log("num checkpoints: " + checkpoints + " total num checkpoints: " + totalCheckpoints);
        currLevelPercent = ((float)checkpoints / (float)totalCheckpoints) * 100;
        percentageDisplay.text = "Tree Roots Restored: " + Mathf.RoundToInt(currLevelPercent) + "%";
        
    }

    public void Damage(float amount)
    {
        CurrentHealth -= amount;
        healthBar.value = CurrentHealth;

        Rigidbody2D playerBody = player.GetComponent<Rigidbody2D>();

        #region Knockback
        float kbForce = 10;

        // Get the point of collision between player and enemy
        /*ContactPoint2D contactPoint = col.GetContact(0);
        Vector2 playerPosition = player.transform.position;*/
        Vector2 dir = player.GetComponent<PlayerController>().Direction;

        // Normalize vector in opposite direction
        dir = -dir.normalized;

        // Making sure this is the only force on the player
        playerBody.velocity = new Vector2(0, 0);
        playerBody.inertia = 0;

        // Temporarily making the player unable to move
        player.GetComponent<PlayerController>().CanMove = false; //if its true player input buttons will work and vice versa.
        Invoke("EnablePlayerControls", 0.2f);
        
        playerBody.AddForce(dir * kbForce, ForceMode2D.Impulse);
        #endregion


        if (CurrentHealth <= 0)
        {
            Die();
        }

        if (invincibility == null)
        {
            invincibility = StartCoroutine(InvincibilityTimer());
        }
         
    }

    public void Progress()
    {
        checkpoints++;
    }

    public void Die()
    {
        //throw new System.NotImplementedException();
    }
}
