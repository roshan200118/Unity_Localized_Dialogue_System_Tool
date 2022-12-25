using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//Class to create the Edit Window
public class TextLocalizerEditWindow : EditorWindow
{
    //Declaring variables to store the current key, and corresponding values
    public string key;
    public string valueEnglish;
    public string valueFrench;
    public string valueSpanish;

    //Method to create the Edit Window
    public static void Open(string key)
    {
        //Create the Edit Window
        TextLocalizerEditWindow window = ScriptableObject.CreateInstance<TextLocalizerEditWindow>();
        window.titleContent = new GUIContent("Localizer Window");
        window.ShowUtility();

        //Assign the key
        window.key = key;
    }


    //Method to design the Edit Window
    public void OnGUI()
    {
        //Create the textfield and assign the input to the key
        key = EditorGUILayout.TextField("Key :", key);

        //Create the TextArea for the English value and assign it
        EditorGUILayout.BeginHorizontal();
        EditorStyles.label.wordWrap = true;
        EditorGUILayout.LabelField("English Value:", GUILayout.MaxWidth(50));
        EditorStyles.textArea.wordWrap = true;
        valueEnglish = EditorGUILayout.TextArea(valueEnglish, EditorStyles.textArea, GUILayout.Height(50), GUILayout.Width(400));
        EditorGUILayout.EndHorizontal();

        //Create the TextArea for the French value and assign it
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("French Value:", GUILayout.MaxWidth(50));
        EditorStyles.textArea.wordWrap = true;
        valueFrench = EditorGUILayout.TextArea(valueFrench, EditorStyles.textArea, GUILayout.Height(50), GUILayout.Width(400));
        EditorGUILayout.EndHorizontal();

        //Create the TextArea for the Spanish value and assign it
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Spanish Value:", GUILayout.MaxWidth(50));
        EditorStyles.textArea.wordWrap = true;
        valueSpanish = EditorGUILayout.TextArea(valueSpanish, EditorStyles.textArea, GUILayout.Height(50), GUILayout.Width(400));
        EditorGUILayout.EndHorizontal();

        //Create the add button, and if it is clicked
        if (GUILayout.Button("Add"))
        {
            //Check if key or any of values fields are empty
            //If empty return a dialog to user informing invalidity
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
            {
                EditorUtility.DisplayDialog("Invalid Key", "Key can not be blank, please enter a meaningful key", "OK");
            }
            else if (string.IsNullOrEmpty(valueEnglish) || string.IsNullOrWhiteSpace(valueEnglish))
            {
                EditorUtility.DisplayDialog("Invalid English Value", "English Value can not be blank, please enter a meaningful value", "OK");
            }
            else if (string.IsNullOrEmpty(valueFrench) || string.IsNullOrWhiteSpace(valueFrench))
            {
                EditorUtility.DisplayDialog("Invalid French Value", "French Value can not be blank, please enter a meaningful value", "OK");
            }
            else if (string.IsNullOrEmpty(valueSpanish) || string.IsNullOrWhiteSpace(valueSpanish))
            {
                EditorUtility.DisplayDialog("Invalid Spanish Value", "Spanish Value can not be blank, please enter a meaningful value", "OK");
            }

            //If all fields are valid
            else
            {
                //If key exists, then replace the key and values
                if (LocalizationSystem.GetLocalizedValue(key) != string.Empty)
                {
                    LocalizationSystem.Replace(key, valueEnglish, valueFrench, valueSpanish);
                }

                //If key doesn't exist, then add the key and values
                else
                {
                    LocalizationSystem.Add(key, valueEnglish, valueFrench, valueSpanish);
                }
            }
        }
    }
}


//Class to create the Search Window
public class TextLocalizerSearchWindow : EditorWindow
{
    //Declaring variables to store the search input, scroll and dictionary
    public string searchInput;
    public Vector2 scroll;
    public Dictionary<string, string> dictionary;

    //Assign the dictionary onEnable
    private void OnEnable()
    {
        dictionary = LocalizationSystem.GetDictionaryForEditor();
    }

    //Method to create the Search Window
    public static void Open()
    {
        //Create the Search Window
        TextLocalizerSearchWindow window = ScriptableObject.CreateInstance<TextLocalizerSearchWindow>();
        window.titleContent = new GUIContent("Localization Search");
        window.ShowUtility();
    }

    //Method to design the Search Window
    public void OnGUI()
    {
        //Create the Search LabelField, and assign the search input
        EditorGUILayout.BeginHorizontal("Box");
        EditorGUILayout.LabelField("Search: ", EditorStyles.boldLabel);
        searchInput = EditorGUILayout.TextField(searchInput);
        EditorGUILayout.EndHorizontal();

        //Get the search results (update dynamically)
        GetSearchResults();
    }

    //Method to get the search results
    private void GetSearchResults()
    {
        //Null check
        if (searchInput == null)
        {
            return;
        }

        //Create each row of results that back search input
        EditorGUILayout.BeginVertical();
        scroll = EditorGUILayout.BeginScrollView(scroll);

        //Assign the dictionary
        dictionary = LocalizationSystem.GetDictionaryForEditor();

        //For each entry in the dictionary
        foreach (KeyValuePair<string, string> element in dictionary)
        {
            //If the key of the entry contains the search input text
            if (element.Key.ToLower().Contains(searchInput.ToLower()) || element.Value.ToLower().Contains(searchInput.ToLower()))
            {
                //Create a row for the result
                EditorGUILayout.BeginHorizontal("box");

                //Create elements for remove button
                GUIStyle guiStyle = new GUIStyle();
                Texture removeIcon = (Texture)Resources.Load("remove");
                GUIContent removeContent = new GUIContent(removeIcon);

                //Create remove button and if it is clicked
                if (GUILayout.Button(removeContent, guiStyle, GUILayout.MaxWidth(20), GUILayout.MaxHeight(20)))
                {
                    //Confirm user wants to remove entry and if user agrees
                    if (EditorUtility.DisplayDialog("Remove Key " + element.Key + "?", "This will remove the element from localization, are you sure?", "Yes"))
                    {
                        //Remove the entry and update
                        LocalizationSystem.Remove(element.Key);
                        AssetDatabase.Refresh();
                        LocalizationSystem.Init();
                        dictionary = LocalizationSystem.GetDictionaryForEditor();
                    }
                }

                //Create a TextField for the key
                EditorGUILayout.TextField(element.Key);

                //Create a TextField for the value
                EditorGUILayout.LabelField(element.Value);

                //End view
                EditorGUILayout.EndHorizontal();
            }
        }

        //End views
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }
}
