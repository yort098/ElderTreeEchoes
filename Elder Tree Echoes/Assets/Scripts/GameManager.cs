using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    private float playerHealth = 100;
    private float invincibilityTime = 1.5f;
    private bool invincible = false;

    public bool Invincible { get { return invincible; } }

    [SerializeField]
    Slider healthBar;

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
        }
    }

    public float PlayerHealth { 
        get { return playerHealth; }
        set { 
            playerHealth = value;
            healthBar.value = playerHealth;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(playerHealth);
        healthBar.value = playerHealth;
    }

    public void TakeDamage(float amount)
    {
        playerHealth -= amount;
        healthBar.value = playerHealth;
        
        StartCoroutine(GameManager.Instance.InvincibilityTimer());
    }

    public IEnumerator InvincibilityTimer()
    {
        SpriteRenderer sp = GameObject.Find("body").GetComponent<SpriteRenderer>();
        Color[] colors = new Color[2] { Color.red, sp.color };
        invincible = true;

        float elapsedTime = 0;
        int index = 0;

        while (elapsedTime < invincibilityTime)
        {
            
            Debug.Log("Elapsed time = " + elapsedTime + " < time = " + invincibilityTime + " deltaTime " + Time.deltaTime);

            sp.color = colors[index % 2];
            elapsedTime += Time.deltaTime;
            index++;
            yield return null;
        }

        invincible = false;
        elapsedTime = 0;
        sp.color = colors[1];


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
