using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpSpawner : MonoBehaviour
{

    [SerializeField] private string popUpName;
    [SerializeField] private string popUpDescription;
    [SerializeField] private ActionType requiredAction; // Enum for specifying the action
    private bool popUpDisplayed = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !popUpDisplayed)
        {
            PopUpManager.Instance.CreatePopUp(popUpName, popUpDescription);

            // Start listening for the required action
            PopUpManager.Instance.RemovePopUpOnAction(() => IsActionCompleted(requiredAction));

            if (requiredAction != ActionType.Traverse) popUpDisplayed = true; // Prevent re-triggering
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && PopUpManager.Instance.popUpOnScreen && requiredAction == ActionType.Traverse)
        {
            PopUpManager.Instance.RemovePopUp();
        }
    }

    private bool IsActionCompleted(ActionType action)
    {
        switch (action)
        {
            case ActionType.Move:
                return Input.GetAxis("Horizontal") != 0;

            case ActionType.Jump:
                return Input.GetKeyDown(KeyCode.Space);

            case ActionType.Interact:
                return Input.GetKeyDown(KeyCode.E);
            
            case ActionType.Traverse:
                return Input.GetKeyDown(KeyCode.W);

            // Add more actions as needed
            default:
                return false;
        }
    }

    public enum ActionType
    {
        Move,
        Jump,
        Interact,
        Traverse, // Placeholder for custom logic if needed
    }
}
