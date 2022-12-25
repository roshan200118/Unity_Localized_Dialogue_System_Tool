using System;
using UnityEngine;

#if UNITY_EDITOR
[Serializable]

//This class will hold every Dialogue Node's data
public class DialogueNodeData
{
    //Declaring a variable to store the Node's GUID
    public string Guid;

    //Declaring a variable to store the Node's dialogue text
    public string DialogueText;

    //Declaring a variable to store the Node's position
    public Vector2 Position;
}
#endif