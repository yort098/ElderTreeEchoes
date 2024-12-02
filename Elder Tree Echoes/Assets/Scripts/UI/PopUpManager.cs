using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpManager : MonoBehaviour
{
    [SerializeField] private GameObject popUpPrefab;
    [SerializeField] private GameObject canvasObject;
    private GameObject createdPopUp;

    private bool popUpOnScreen = false;

    private void Start()
    {
        popUpPrefab.GetComponent<RectTransform>().position = new Vector2(-popUpPrefab.GetComponent<RectTransform>().sizeDelta.x, 0f);
        CreatePopUp("Tooltip", "This is a test");

    }

    public void CreatePopUp(string name, string description)
    {
        createdPopUp = Instantiate(popUpPrefab, canvasObject.transform);
        popUpPrefab.GetComponent<PopUp>().SetName(name);
        popUpPrefab.GetComponent<PopUp>().SetDescription(description);

        ShowPopUp();
    }

    private void ShowPopUp()
    {
        popUpOnScreen = true;
        createdPopUp.GetComponent<RectTransform>().DOAnchorPosX(0, 1f)
        .OnComplete(() => HidePopUp());

    }

    private void HidePopUp()
    {
        popUpOnScreen = false;
        createdPopUp.GetComponent<RectTransform>().DOAnchorPosX(-popUpPrefab.GetComponent<RectTransform>().sizeDelta.x, 0.5f)
            .OnComplete(() => Destroy(createdPopUp));

    }

    public void RemovePopUp()
    {
        if (popUpOnScreen)
        {
            HidePopUp();
        }
    }
}
