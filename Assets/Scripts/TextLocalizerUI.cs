using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//Class to be attached to UI Text elements
[RequireComponent(typeof(TextMeshProUGUI))]
public class TextLocalizerUI : MonoBehaviour
{
    //Declaring a variable to store the TextMeshProUGUI field
    TextMeshProUGUI textField;

    //Creating a LocalizedString object
    public LocalizedString localizedString;

    private void Start()
    {
        //Assign the TextMeshProUGUI component
        textField = GetComponent<TextMeshProUGUI>();

        //Update the text
        textField.text = localizedString.value;
    }

    //Can update the text dynamically (for changing languages)
    private void Update()
    {
        textField.text = localizedString.value;
    }
}
