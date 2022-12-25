using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

//Class to load the CSV (where the keys and corresponding language values are stored)
public class CSVLoader
{
    //Declaring a variable to store the CSV contents
    private TextAsset csvFile;

    //Declaring variables to be used for parsing
    private const char lineSeperator = '\n';
    private const char surround = '"';
    private string[] fieldSeperator = { "\",\"" };

    //Method to load the CSV contents
    public void LoadCSV()
    {
        //Load the contents from localization.csv
        csvFile = Resources.Load<TextAsset>("localization");
    }

    //Method to extract and parse dictionary values based on language code
    public Dictionary<string, string> GetDictionaryValues(string languageId)
    {
        Dictionary<string, string> dictionary = new Dictionary<string, string>();

        //Split each line in CSV
        string[] lines = csvFile.text.Split(lineSeperator);

        //Declaring a variable to store the current language index (ex. EN = 1, FR = 2)
        int languageIndex = 1;

        //Array to store different languages
        string[] languageHeaders = lines[0].Split(fieldSeperator, System.StringSplitOptions.None);

        //Loop through languages and see which one was selected
        for (int i = 0; i < languageHeaders.Length; i++)
        {
            if (languageHeaders[i].Contains(languageId))
            {
                languageIndex = i;
                break;
            }
        }

        //Regex to parse CSV
        Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

        //Start at i = 1 since first line is to define keys
        for (int i = 1; i < lines.Length; i++)
        {
            //Assign current line
            string line = lines[i];

            //Split the current line into its texts in each language
            string[] fields = CSVParser.Split(line);

            //For each lanuage of the current phrase, parse it
            for (int j = 0; j < fields.Length; j++)
            {
                fields[j] = fields[j].TrimStart(' ', surround);
                fields[j] = fields[j].TrimEnd(surround);

                //This is what some text looks like --> "\"Bonjour le monde\"\r"
                //This statement will remove carriage return in the text
                if (fields[j].EndsWith("\r") || fields[j].Contains('\r'))
                {
                    fields[j] = fields[j].Replace("\r", "");
                    fields[j] = fields[j].TrimEnd(surround);
                }
            }

            //Only assign if there are as many texts as there are languages
            if (fields.Length > languageIndex)
            {
                //Store the key
                var key = fields[0];

                //Check if dictionary has the key
                if (dictionary.ContainsKey(key))
                {
                    continue;
                }

                //Store the selected languages text
                var value = fields[languageIndex];

                //Add the key value pair to dictionary
                dictionary.Add(key, value);
            }

        }
        return dictionary;
    }

#if UNITY_EDITOR
    //Method to add a key-value pair
    public void Add(string key, string valueEN, string valueFR, string valueES)
    {
        //Format the entry
        string lineToAppend = string.Format("\n\"{0}\",\"{1}\",\"{2}\",\"{3}\"", key, valueEN, valueFR, valueES);

        //Append the entry
        File.AppendAllText("Assets/Resources/localization.csv", lineToAppend);

        //Refresh
        UnityEditor.AssetDatabase.Refresh();
    }

    //Method to remove a key-value pair
    public void Remove(string key)
    {
        //Arrays to store the lines and keys
        string[] lines = csvFile.text.Split(lineSeperator);
        string[] keys = new string[lines.Length];

        //Loop through each line and assign the keys
        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            keys[i] = line.Split(fieldSeperator, System.StringSplitOptions.None)[0];
        }

        //Declare a variable to store the index of the key to be removed
        //Set at -1 if not found
        int index = -1;

        //Loop to find where the key is
        for (int i = 0; i < keys.Length; i++)
        {
            if (keys[i].Contains(key))
            {
                index = i;
                break;
            }
        }

        //If the key exists
        if (index > -1)
        {
            //Create a new array to store updated array
            string[] newLines;

            //Copy the same array but removed the desired line
            newLines = lines.Where(w => w != lines[index]).ToArray();

            //Join back the array 
            string replaced = string.Join(lineSeperator.ToString(), newLines);

            //Update array
            File.WriteAllText("Assets/Resources/localization.csv", replaced);
        }
    }

    //Method to edit a key-value pair
    public void Edit(string key, string valueEN, string valueFR, string valueES)
    {
        Remove(key);
        Add(key, valueEN, valueFR, valueES);
    }
#endif
}
