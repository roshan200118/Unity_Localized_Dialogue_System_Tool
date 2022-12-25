using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

#if UNITY_EDITOR
//Class to represent the Dialogue Node object
public class DialogueNode : Node
{
    //Unique ID for each node (to identify each node)
    public string GUID;

    //Dialogue text
    public string DialogueText;

    //Checks if node is a start point
    public bool EntryPoint = false;
}
#endif