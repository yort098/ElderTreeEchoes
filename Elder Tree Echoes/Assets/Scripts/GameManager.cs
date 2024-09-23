using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    private int playerHealth = 100;

    [SerializeField]
    Slider healthBar;

    /// <summary>
    /// Returns a reference to the game manager class
    /// </summary>
    public static GameManager Instance { get { return instance; } }

    public int PlayerHealth { get { return playerHealth; } }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(playerHealth);
        healthBar.value = playerHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
