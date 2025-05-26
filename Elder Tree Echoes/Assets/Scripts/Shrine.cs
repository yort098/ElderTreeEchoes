using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shrine : MonoBehaviour
{
    [SerializeField] private Power grantedPower; // Type of power the shrine grants
    [SerializeField] private KeyCode interactionKey = KeyCode.E; // Default interaction key
    [SerializeField] private string popUpMessage = "Press E to interact";
    [SerializeField] private int requirementLevel = 1;
    private bool isPlayerNearby = false;
    private bool powerGranted = false;
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !powerGranted)
        {
            PopUpManager.Instance.CreatePopUp("Shrine", popUpMessage);

            isPlayerNearby = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PopUpManager.Instance.RemovePopUp();
            isPlayerNearby = false;
        }
    }

    private void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(interactionKey) && !powerGranted && grantedPower != Power.None)
        {
            PopUpManager.Instance.RemovePopUp();
            GrantPower();
        }

        if (grantedPower == Power.None && !powerGranted && isPlayerNearby && Input.GetKeyDown(interactionKey))
        {
            GetComponent<CameraTrigger>().Activate();
            NextLevel();
        }

        if (isPlayerNearby && !PopUpManager.Instance.popUpOnScreen && !powerGranted)
        {
            PopUpManager.Instance.CreatePopUp("Shrine", popUpMessage);
        }
    }

    public void NextLevel()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.shrineComplete);
        GameManager.Instance.currLevel++;
        PopUpManager.Instance.RemovePopUp();
        powerGranted = true;
    }

    private void GrantPower()
    {       
        if (GameManager.Instance.currLevel == requirementLevel)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.abilityGained);
            powerGranted = true;

            // Notify the player of the new power
            PopUpManager.Instance.CreatePopUp("Power Unlocked", $"You have unlocked the {grantedPower} power!");

            StartCoroutine(InformPlayer());
            PlayerAbilities.Instance.UnlockPower(grantedPower);
        }
        else
        {
            PopUpManager.Instance.CreatePopUp("", "Locked.");
        }    
    }

    private IEnumerator InformPlayer()
    {
        GameObject.Find("Player").GetComponent<PlayerController>().CanMove = false;

        yield return new WaitForSeconds(5f);

        switch (grantedPower)
        {
            case Power.Water:
                PopUpManager.Instance.CreatePopUp("Power Unlocked", $"Press Left click to melee enemies with water");
                break;
            
            case Power.Light:
                PopUpManager.Instance.CreatePopUp("Power Unlocked", $"Press Left click to shoot enemies with light");
                break;
        }

        yield return new WaitForSeconds(5f);

        switch (grantedPower)
        {
            case Power.Water:
                PopUpManager.Instance.CreatePopUp("Power Unlocked", $"Press Right click to grow plants");
                break;

            case Power.Light:
                PopUpManager.Instance.CreatePopUp("Power Unlocked", $"Hold Right click to revitalize platforms");
                break;
        }


        yield return new WaitForSeconds(5f);
        PopUpManager.Instance.RemovePopUp();
        //GameObject.Find("Player").GetComponent<PlayerController>().CanMove = true;
    }
}
