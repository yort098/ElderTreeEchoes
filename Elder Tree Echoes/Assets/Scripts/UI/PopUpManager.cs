using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpManager : MonoBehaviour
{
    public static PopUpManager Instance { get; private set; }
    [SerializeField] private GameObject popUpPrefab;
    [SerializeField] private GameObject canvasObject;
    private GameObject createdPopUp;

    public bool popUpOnScreen = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        popUpPrefab.GetComponent<RectTransform>().position = new Vector2(-popUpPrefab.GetComponent<RectTransform>().sizeDelta.x, 0f);
        //CreatePopUp("Tooltip", "This is a test");

    }

    public void CreatePopUp(string name, string description)
    {
        Debug.Log(popUpOnScreen);
        if (!popUpOnScreen)
        {
            createdPopUp = Instantiate(popUpPrefab, canvasObject.transform);
            createdPopUp.GetComponent<PopUp>().SetName(name);
            createdPopUp.GetComponent<PopUp>().SetDescription(description);

            ShowPopUp();
        }
        else
        {
            //StartCoroutine(WaitForNewPopup(name, description));
            DestroyPopUp();
            CreatePopUp(name, description);
        }
        
    }

    private void ShowPopUp()
    {
        popUpOnScreen = true;
        createdPopUp.GetComponent<RectTransform>().DOAnchorPosX(0, 1f);

    }

    private void HidePopUp()
    {
        createdPopUp.GetComponent<RectTransform>().DOAnchorPosX(-popUpPrefab.GetComponent<RectTransform>().sizeDelta.x, 1f)
            .OnComplete(() => DestroyPopUp());

    }

    public void RemovePopUp()
    {
        if (popUpOnScreen)
        {
            HidePopUp();
        }
    }

    private void DestroyPopUp()
    {
        DOTween.Kill(createdPopUp.GetComponent<RectTransform>());
        Destroy(createdPopUp);
        popUpOnScreen = false;
    }

    public void RemovePopUpOnAction(System.Func<bool> actionCondition)
    {
        StartCoroutine(WaitForAction(actionCondition));
    }

    private IEnumerator WaitForAction(System.Func<bool> actionCondition)
    {
        while (!actionCondition()) yield return null;

        yield return new WaitForSeconds(0.5f);

        RemovePopUp();
    }

    private IEnumerator WaitForNewPopup(string name, string description)
    {
        while (createdPopUp != null) yield return null;
    }
}
