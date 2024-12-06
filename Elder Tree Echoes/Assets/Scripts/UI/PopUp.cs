using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopUp : MonoBehaviour
{

    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descriptionText;

    public void SetName(string name)
    {
        nameText.text = name;
    } 
    
    public void SetDescription(string description)
    {
        descriptionText.text = description;
    }
}
