using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
[Serializable]

//Creating a DialogueContainer class to store data regarding NodeLinks and DialogueNodeData
//Using ScriptableObject to act as a data container to save large amounts of data 
public class DialogueContainer : ScriptableObject
{
    //Creating a list to store NodeLinkData
    public List<NodeLinkData> NodeLinks = new List<NodeLinkData>();

    //Creating a list to store DialogueNodeData
    public List<DialogueNodeData> DialogueNodeData = new List<DialogueNodeData>();
}
#endif