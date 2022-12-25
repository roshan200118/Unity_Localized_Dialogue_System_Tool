using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//Class to parse a dialogue container, localize the dialogue and options, and display it in UI
public class DialogueParser : MonoBehaviour
{
    //Creating a variable to reference to dialogue container (which dialogue tree file)
    [SerializeField] private DialogueContainer dialogue;

    //Creating a variable to reference the TextField to display the dialogue
    [SerializeField] private TextMeshProUGUI dialogueTextField;

    //Creating a variable to reference the button prefab
    [SerializeField] private Button optionButton;

    //Creating an array of button containers (currently only for 4 options, able to add more)
    [SerializeField] private Transform[] buttonContainersArray;

    //Creating a list to store the current buttons 
    private List<Button> currentButtonsList = new List<Button>();

    //Creating a variable to reference the localized dictionary 
    private Dictionary<string, string> dictionary;

    //Creating a variable to store the current dialogue text
    private string dialogueText;

    //Checks if there is a localized version of text
    bool isDialogueTextLocalized = false;
    bool isOption1Localized = false;
    bool isOption2Localized = false;
    bool isOption3Localized = false;
    bool isOption4Localized = false;

    //Creating variables to store localized strings for dialogue and options
    public LocalizedDialogueString localizedDialogueString;
    public LocalizedDialogueString localizedOption1String;
    public LocalizedDialogueString localizedOption2String;
    public LocalizedDialogueString localizedOption3String;
    public LocalizedDialogueString localizedOption4String;

    private void Start()
    {
        //Get the entry point node
        var narrativeData = dialogue.NodeLinks.First();

        //Proceed the narratuve 
        ProceedToNarrative(narrativeData.TargetNodeGuid);
    }

    private void OnEnable()
    {
        //Assign the dictionary
        dictionary = LocalizationSystem.GetDictionaryForEditor();
    }

    //Method to progress through the narrative
    private void ProceedToNarrative(string narrativeDataGUID)
    {
        //Assign the dialogue text from current node
        dialogueText = dialogue.DialogueNodeData.Find(x => x.Guid == narrativeDataGUID).DialogueText;

        isDialogueTextLocalized = false;

        //For each entry in the dictionary
        foreach (KeyValuePair<string, string> element in dictionary)
        {
            //If a key in the dictionary equals the dialogue text (this means the dialogue text is localized if text is a key in dictionary)
            if (string.Equals(element.Key.ToLower(), dialogueText.ToLower()))
            {
                //The dialogue text is localized
                isDialogueTextLocalized = true;

                //Set the key
                localizedDialogueString.SetKey(element.Key);

                //Update the dialogue text to be localized
                dialogueText = localizedDialogueString.value;
            }
        }

        //Set the dialogue TextField to be the dialogue text
        dialogueTextField.text = dialogueText;

        //Creating a variable to store the choices
        var choices = dialogue.NodeLinks.Where(x => x.BaseNodeGuid == narrativeDataGUID);

        //Loop though the button containers array
        for (int i = 0; i < buttonContainersArray.Length; i++)
        {
            //For each button container in the array, destory the button (must refresh buttons)
            foreach (Transform child in buttonContainersArray[i].transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }

        //Clear the current button list (new set of choices)
        currentButtonsList.Clear();

        //Creating a variable to keep track of loop iteration
        //choices does not properly convert to array 
        int count = 0;

        //All options start unlocalized
        isOption1Localized = false;
        isOption2Localized = false;
        isOption3Localized = false;
        isOption4Localized = false;

        //Loop through choices
        foreach (var choice in choices)
        {
            //Creating a variable to store the choice text
            string optionText = choice.PortName;

            //Creating a button for the choice in the corresponding button container
            var button = Instantiate(optionButton, buttonContainersArray[count]);

            //For each entry in the dictionary
            foreach (KeyValuePair<string, string> element in dictionary)
            {
                //If a key in the dictionary equals the option text (this means the option text is localized if option text is a key in dictionary)
                if (string.Equals(element.Key.ToLower(), optionText.ToLower()))
                {
                    //Assign the option text the dictionary value
                    optionText = element.Value;

                    //The corresponding option is localized
                    //Set the key for the localized option
                    //Assign the localized value
                    switch (count)
                    {
                        case 0:
                            isOption1Localized = true;
                            localizedOption1String.SetKey(element.Key);
                            optionText = localizedOption1String.value;
                            break;
                        case 1:
                            isOption2Localized = true;
                            localizedOption2String.SetKey(element.Key);
                            optionText = localizedOption2String.value;
                            break;
                        case 2:
                            isOption3Localized = true;
                            localizedOption3String.SetKey(element.Key);
                            optionText = localizedOption3String.value;
                            break;
                        case 3:
                            isOption4Localized = true;
                            localizedOption4String.SetKey(element.Key);
                            optionText = localizedOption4String.value;
                            break;
                    }
                }
            }

            //Set the text of the option button 
            button.GetComponentInChildren<TextMeshProUGUI>().text = optionText;

            //When click button, progress through the narrative
            button.onClick.AddListener(() => ProceedToNarrative(choice.TargetNodeGuid));

            //Add the button to the current button list
            currentButtonsList.Add(button);

            count++;
        }
    }

    private void Update()
    {
        //If the dialogue text is localized, then assign the localized value to the TextField (for dynamic update)
        if (isDialogueTextLocalized)
        {
            dialogueTextField.text = localizedDialogueString.value;
        }

        //If the option 1 is localized, then assign the localized value to the TextField (for dynamic update)
        if (isOption1Localized)
        {
            currentButtonsList[0].GetComponentInChildren<TextMeshProUGUI>().text = localizedOption1String.value;
        }

        //If the option 2 is localized, then assign the localized value to the TextField (for dynamic update)
        if (isOption2Localized)
        {
            currentButtonsList[1].GetComponentInChildren<TextMeshProUGUI>().text = localizedOption2String.value;
        }

        //If the option 3 is localized, then assign the localized value to the TextField (for dynamic update)
        if (isOption3Localized)
        {
            currentButtonsList[2].GetComponentInChildren<TextMeshProUGUI>().text = localizedOption3String.value;
        }

        //If the option 4 is localized, then assign the localized value to the TextField (for dynamic update)
        if (isOption4Localized)
        {
            currentButtonsList[3].GetComponentInChildren<TextMeshProUGUI>().text = localizedOption4String.value;
        }
    }
}
