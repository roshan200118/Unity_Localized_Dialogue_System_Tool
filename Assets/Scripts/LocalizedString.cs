using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Defining an object to be used by the UI Text
[System.Serializable]
public class LocalizedString
{
    //Declaring a variable to store the key
    public string key;

    //Constructor
    public LocalizedString(string key)
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

    //Assigns a new key to a localized string
    public static implicit operator LocalizedString(string key)
    {
        return new LocalizedString(key);
    }
}
