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
            //PopUpManager.Instance.RemovePopUpOnAction(() => IsActionCompleted(requiredAction));

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
            GrantPower();
        }
    }

    private void GrantPower()
    {
        powerGranted = true;
        // Notify the player of the new power
        PopUpManager.Instance.CreatePopUp("Power Unlocked", $"You have unlocked the {grantedPower} power!");

        // Unlock the power in the PlayerController or PlayerAbility system
        PlayerAbilities.Instance.UnlockPower(grantedPower);
    }
}
