using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shrine : MonoBehaviour
{
    [SerializeField] private Power grantedPower; // Type of power the shrine grants
    [SerializeField] private KeyCode interactionKey = KeyCode.E; // Default interaction key
    [SerializeField] private string popUpMessage = "Press E to interact";
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
        if (isPlayerNearby && Input.GetKeyDown(interactionKey) && !powerGranted)
        {
            PopUpManager.Instance.RemovePopUp();
            GrantPower();
        }

        if (isPlayerNearby && !PopUpManager.Instance.popUpOnScreen && !powerGranted)
        {
            PopUpManager.Instance.CreatePopUp("Shrine", popUpMessage);
        }
    }

    private void GrantPower()
    {
        powerGranted = true;

        // Notify the player of the new power
        PopUpManager.Instance.CreatePopUp("Power Unlocked", $"You have unlocked the {grantedPower} power!");

        StartCoroutine(InformPlayer());
        PlayerAbilities.Instance.UnlockPower(grantedPower);
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
        GameObject.Find("Player").GetComponent<PlayerController>().CanMove = true;
    }
}
