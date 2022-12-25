using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
[Serializable]

//Class to store connection data between 2 Dialogue Nodes
public class NodeLinkData
{
    //Declaring a variable to store the base Node's GUID
    public string BaseNodeGuid;

    //Declaring a variable to store the port name that connects the 2 Dialogue Nodes
    public string PortName;

    //Declaring a variable to store the target node's (node its connected to) GUID 
    public string TargetNodeGuid;
}
#endif
