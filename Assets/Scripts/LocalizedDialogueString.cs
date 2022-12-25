using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class to represent a localized dialogue string
[System.Serializable]
public class LocalizedDialogueString
{
    //Declaring a variable to store the key
    public string key;

    //Key setter
    public void SetKey(string value)
    {
        this.key = value;
    }

    //Constructor
    public LocalizedDialogueString(string key)
    {
        this.key = key;
    }

    //Declaring a variable to store the value
    public string value
    {
        //Accessor
        get
        {
            return LocalizationSystem.GetLocalizedValue(key);
        }
    }
}
