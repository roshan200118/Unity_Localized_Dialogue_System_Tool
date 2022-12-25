using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//Class to create the property drawer for the LocalizedString object
[CustomPropertyDrawer(typeof(LocalizedString))]
public class LocalizedStringDrawer : PropertyDrawer
{
    //Declaring a variable to check if dropdown is active
    bool dropdown;

    //Declaring a variable to reference the height
    float height;

    //Method to extend dropdown if active
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (dropdown)
        {
            return height + 25;
        }

        return 20;
    }

    //Method to create the GUI
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //Begin the property
        EditorGUI.BeginProperty(position, label, property);

        //Draw the PrefixLabel of where key is inputted 
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        position.width -= 34;
        position.height = 18;

        //The Rect where the value value for the key will be stored
        Rect valueRect = new Rect(position);
        valueRect.x += 15;
        valueRect.width = 15;

        //The Rect for the fold button
        Rect foldButtonRect = new Rect(position);
        foldButtonRect.width = 15;

        //Create the foldout component and set the dropdown
        dropdown = EditorGUI.Foldout(foldButtonRect, dropdown, "");
        position.x += 15;
        position.width -= 15;

        //Create a text field for the key and read it
        SerializedProperty key = property.FindPropertyRelative("key");
        key.stringValue = EditorGUI.TextField(position, key.stringValue);
        position.x += position.width + 2;
        position.width = 17;
        position.height = 17;

        //Create search button
        Texture searchIcon = (Texture)Resources.Load("search");
        GUIContent searchContent = new GUIContent(searchIcon);
        GUIStyle guiStyle = new GUIStyle();

        //When search button is clicked
        if (GUI.Button(position, searchContent, guiStyle))
        {
            //Open search window
            TextLocalizerSearchWindow.Open();
        }

        position.x += position.width + 2;

        //Create edit button
        Texture editIcon = (Texture)Resources.Load("edit");
        GUIContent editContent = new GUIContent(editIcon);

        //When edit button is clicked
        if (GUI.Button(position, editContent, guiStyle))
        {
            //Open edit window
            TextLocalizerEditWindow.Open(key.stringValue);
        }

        //If dropdown is active
        if (dropdown)
        {
            //Get and store the selected language value
            var value = LocalizationSystem.GetLocalizedValue(key.stringValue);

            //Draw a label with the returned value
            GUIStyle style = GUI.skin.box;
            height = style.CalcHeight(new GUIContent(value), valueRect.width);
            valueRect.height = height;
            valueRect.y += 21;
            valueRect.width = 130;
            EditorGUI.LabelField(valueRect, value, EditorStyles.wordWrappedLabel);
        }

        //End the property
        EditorGUI.EndProperty();
    }
}
