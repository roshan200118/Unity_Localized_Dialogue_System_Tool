using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class to manage the localization system
public class LocalizationSystem
{
    //Creating enum for supported languages
    public enum Language
    {
        English,
        French,
        Spanish
    }

    //Set standard language to English
    public static Language language = Language.English;

    //Creating dictionaries for each language
    private static Dictionary<string, string> localizedEN;
    private static Dictionary<string, string> localizedFR;
    private static Dictionary<string, string> localizedES;

    //Check if initialized
    public static bool isInit;

    //CSVLoader object
    public static CSVLoader csvLoader;

    //Initialize method
    public static void Init()
    {
        //Assign and load CSV
        csvLoader = new CSVLoader();
        csvLoader.LoadCSV();

        UpdateDictionaries();

        isInit = true;
    }

    //Method to update the dictionaries
    public static void UpdateDictionaries()
    {
        localizedEN = csvLoader.GetDictionaryValues("en");
        localizedFR = csvLoader.GetDictionaryValues("fr");
        localizedES = csvLoader.GetDictionaryValues("es");
    }

    //Method to get the dictionary for Editor for the selected language
    public static Dictionary<string, string> GetDictionaryForEditor()
    {
        if (!isInit)
        {
            Init();
        }

        //Return dictionary based on selected language (EN is default)
        switch (language)
        {
            case Language.English:
                return localizedEN;
            case Language.French:
                return localizedFR;
            case Language.Spanish:
                return localizedES;
            default:
                return localizedEN;
        }
    }

    //Method to return the value of the selected language
    public static string GetLocalizedValue(string key)
    {
        if (!isInit)
        {
            Init();
        }

        //Creating a variable to store the value
        string value = key;

        //Assign value based on selected language
        switch (language)
        {
            case Language.English:
                localizedEN.TryGetValue(key, out value);
                break;
            case Language.French:
                localizedFR.TryGetValue(key, out value);
                break;
            case Language.Spanish:
                localizedES.TryGetValue(key, out value);
                break;
        }

        return value;
    }

    //Method to add key-value pair
    public static void Add(string key, string valueEN, string valueFR, string valueES)
    {
        //Check values for parsing
        if (valueEN.Contains("\""))
        {
            valueEN.Replace('"', '\"');
        }

        if (valueFR.Contains("\""))
        {
            valueFR.Replace('"', '\"');
        }

        if (valueEN.Contains("\""))
        {
            valueES.Replace('"', '\"');
        }

        //Null check
        if (csvLoader == null)
        {
            csvLoader = new CSVLoader();
        }

        //Add the key-value pair to CSV
        csvLoader.LoadCSV();
        csvLoader.Add(key, valueEN, valueFR, valueES);
        csvLoader.LoadCSV();

        UpdateDictionaries();
    }

    //Method to replace key-value pair
    public static void Replace(string key, string valueEN, string valueFR, string valueES)
    {
        //Check values for parsing
        if (valueEN.Contains("\""))
        {
            valueEN.Replace('"', '\"');
        }

        if (valueFR.Contains("\""))
        {
            valueFR.Replace('"', '\"');
        }

        if (valueEN.Contains("\""))
        {
            valueFR.Replace('"', '\"');
        }

        //Null check
        if (csvLoader == null)
        {
            csvLoader = new CSVLoader();
        }

        //Update key-value pair
        csvLoader.LoadCSV();
        csvLoader.Edit(key, valueEN, valueFR, valueES);
        csvLoader.LoadCSV();

        UpdateDictionaries();
    }

    //Method to remove key-value pair
    public static void Remove(string key)
    {
        //Null check
        if (csvLoader == null)
        {
            csvLoader = new CSVLoader();
        }

        //Remove key-value pair
        csvLoader.LoadCSV();
        csvLoader.Remove(key);
        csvLoader.LoadCSV();

        UpdateDictionaries();
    }
}
