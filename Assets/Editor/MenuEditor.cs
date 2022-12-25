using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MenuEditor : MonoBehaviour
{
    //Creating a Languages --> English Menu Item
    //Change language to English when selected
    [MenuItem("Languages/English")]
    private static void LanguagesEnglishOption()
    {
        LocalizationSystem.language = LocalizationSystem.Language.English;
    }

    //Creating a Languages --> French Menu Item
    //Change language to French when selected
    [MenuItem("Languages/French")]
    private static void LanguagesFrenchOption()
    {
        LocalizationSystem.language = LocalizationSystem.Language.French;
    }

    //Creating a Languages --> Spanish Menu Item
    //Change language to Spanish when selected
    [MenuItem("Languages/Spanish")]
    private static void LanguagesSpanishOption()
    {
        LocalizationSystem.language = LocalizationSystem.Language.Spanish;
    }
}
