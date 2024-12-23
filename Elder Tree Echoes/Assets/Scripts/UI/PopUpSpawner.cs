using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PopUpSpawner : MonoBehaviour
{

    [SerializeField] private string popUpName;
    [SerializeField] private string popUpDescription;
    [SerializeField] private ActionType requiredAction; // Enum for specifying the action
    [SerializeField] KeyCode requiredKey;
    [SerializeField] MouseButton requiredMouseInput;
    private bool popUpDisplayed = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !popUpDisplayed)
        {
            PopUpManager.Instance.CreatePopUp(popUpName, popUpDescription);

            // Start listening for the required action
            PopUpManager.Instance.RemovePopUpOnAction(() => IsActionCompleted(requiredAction));

            if (requiredAction != ActionType.Traverse) popUpDisplayed = true; // Prevent re-triggering

            if (GetComponent<TeleportTrigger>())
            {
                GameObject.Find("Player").GetComponent<PlayerController>().isDoor = true;
                GameObject.Find("Player").GetComponent<PlayerController>().currentDoor = this.gameObject;

            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && PopUpManager.Instance.popUpOnScreen && requiredAction == ActionType.Traverse)
        {
            PopUpManager.Instance.RemovePopUp();
        }

        if (GetComponent<TeleportTrigger>())
        {
            GameObject.Find("Player").GetComponent<PlayerController>().isDoor = false;
            GameObject.Find("Player").GetComponent<PlayerController>().currentDoor = null;

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
                if (requiredKey != KeyCode.None)
                {
                    return Input.GetKeyDown(requiredKey);
                }
                else
                {
                    return Input.GetMouseButtonDown((int)requiredMouseInput);
                }
                
            
            case ActionType.Traverse:
                return Input.GetKeyDown(requiredKey);
                

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
        Traverse, 
    }
}
