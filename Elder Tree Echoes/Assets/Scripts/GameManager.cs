using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    private float playerHealth = 100;
    private float invincibilityFrames = 1.5f;
    private float timeInvincible = 0;
    private bool invincible = false;

    public bool Invincible { get { return invincible; } }

    [SerializeField]
    Slider healthBar;

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

    public IEnumerator InvincibilityTimer()
    {
        SpriteRenderer sp = GameObject.Find("body").GetComponent<SpriteRenderer>();
        Color[] colors = new Color[2] { Color.red, sp.color };
        int index = 0;

        invincible = true;
        
        while (timeInvincible < invincibilityFrames)
        {
            timeInvincible += Time.deltaTime;
            Debug.Log(timeInvincible);

            sp.color = colors[index % 2];
            index++;
            yield return null;
        }

        invincible = false;
        timeInvincible = 0;
        sp.color = colors[1];


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
