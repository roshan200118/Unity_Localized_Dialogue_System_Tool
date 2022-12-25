using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

#if UNITY_EDITOR

//Class to save and load data in DialogueContainer
public class GraphSaveUtility
{
    //Declaring a variable to reference the target graph view
    private DialogueGraphView _targetGraphView;

    //Declaring a variable to cache the target dialogue container
    private DialogueContainer _containerCache;

    //Declaring a variable to store the edges (connections) of the target graph
    private List<Edge> Edges => _targetGraphView.edges.ToList();

    //Declaring a variable to store the nodes of the target graph
    private List<DialogueNode> Nodes => _targetGraphView.nodes.ToList().Cast<DialogueNode>().ToList();

    //Method to return new instance of GraveSaveUtility
    public static GraphSaveUtility GetInstance(DialogueGraphView targetGraphView)
    {
        return new GraphSaveUtility
        {
            _targetGraphView = targetGraphView
        };
    }

    //Method to save a graph
    public void SaveGraph(string fileName)
    {
        //If no connections, then don't save
        if (!Edges.Any()) return;

        //Create a new DialougeContainer
        var dialogueContainer = ScriptableObject.CreateInstance<DialogueContainer>();

        //Save the connected ports into an array
        var connectedPorts = Edges.Where(x => x.input.node != null).ToArray();

        //Loop through the connected ports
        for (var i = 0; i < connectedPorts.Length; i++)
        {
            //Save the output and input node of the connection
            var outputNode = connectedPorts[i].output.node as DialogueNode;
            var inputNode = connectedPorts[i].input.node as DialogueNode;

            //Save the connection as a NodeLinkData
            dialogueContainer.NodeLinks.Add(new NodeLinkData
            {
                BaseNodeGuid = outputNode.GUID,
                PortName = connectedPorts[i].output.portName,
                TargetNodeGuid = inputNode.GUID,
            });
        }

        //Loop through the nodes (not including entry node)
        foreach (var dialogueNode in Nodes.Where(node => !node.EntryPoint))
        {
            //Add the node to the DialogueNodeData list in the DialogueContainer
            dialogueContainer.DialogueNodeData.Add(new DialogueNodeData
            {
                Guid = dialogueNode.GUID,
                DialogueText = dialogueNode.DialogueText,
                Position = dialogueNode.GetPosition().position
            });
        }

        //If no resources folder exists
        if (!AssetDatabase.IsValidFolder("Assets/Resources"))
        {
            //Create a resources folder
            AssetDatabase.CreateFolder("Assets", "Resources");
        }

        //Create the asset to store the DialogueContainer using the inputted file name
        AssetDatabase.CreateAsset(dialogueContainer, $"Assets/Resources/{fileName}.asset");

        //Save the asset
        AssetDatabase.SaveAssets();
    }

    //Method to load a graph
    public void LoadGraph(string fileName)
    {
        //Load the container from resources 
        _containerCache = Resources.Load<DialogueContainer>(fileName);

        //Check if file exists
        if (_containerCache == null)
        {
            EditorUtility.DisplayDialog("File Not Found", "Target dialogue graph file does not exist!", "OK");
            return;
        }

        //Clear the graph (so its blank)
        ClearGraph();
        CreateNodes();
        ConnectNodes();
    }

    //Method to connect nodes
    private void ConnectNodes()
    {
        //Loop through nodes in graph view
        for (var i = 0; i < Nodes.Count; i++)
        {
            //Save the connections between current nodes and output nodes
            var connections = _containerCache.NodeLinks.Where(x => x.BaseNodeGuid == Nodes[i].GUID).ToList();

            //Loop through all the connections
            for (var j = 0; j < connections.Count; j++)
            {
                //Save the target node GUID
                var targetNodeGuid = connections[j].TargetNodeGuid;

                //Save the target node
                var targetNode = Nodes.First(x => x.GUID == targetNodeGuid);

                //Link the output port and input port of the connection
                LinkNodes(Nodes[i].outputContainer[j].Q<Port>(), (Port)targetNode.inputContainer[0]);

                //Set the position of the node
                targetNode.SetPosition(new Rect(
                    _containerCache.DialogueNodeData.First(x => x.Guid == targetNodeGuid).Position,
                    _targetGraphView.DefaultNodeSize
                ));
            }
        }
    }

    //Method to link node ports together
    private void LinkNodes(Port output, Port input)
    {
        //Create a new edge (connection) using the output and input ports
        var tempEdge = new Edge
        {
            output = output,
            input = input
        };

        //Connect the ports
        tempEdge?.input.Connect(tempEdge);
        tempEdge?.output.Connect(tempEdge);

        //Add the connection to the graph view
        _targetGraphView.Add(tempEdge);
    }

    //Create the nodes from the file
    private void CreateNodes()
    {
        //Loop through all nodes in target graph to load
        foreach (var nodeData in _containerCache.DialogueNodeData)
        {
            //Creating a variable to store the DialogueNodes
            var tempNode = _targetGraphView.CreateDialogueNode(nodeData.DialogueText);
            tempNode.GUID = nodeData.Guid;

            //Add the node to graph view
            _targetGraphView.AddElement(tempNode);

            //Creating a variable to store the NodeLinkData (connections)
            var nodePorts = _containerCache.NodeLinks.Where(x => x.BaseNodeGuid == nodeData.Guid).ToList();

            //Create the connections for the current node
            nodePorts.ForEach(x => _targetGraphView.AddChoicePort(tempNode, x.PortName));
        }
    }

    //Method to clear a graph
    private void ClearGraph()
    {
        //Find the entry point of graph and change its GUID
        Nodes.Find(x => x.EntryPoint).GUID = _containerCache.NodeLinks[0].BaseNodeGuid;

        //Loop through nodes
        foreach (var node in Nodes)
        {
            //Keep the entry point nodes
            if (node.EntryPoint)
            {
                continue;
            }

            //Get each output connection for each node (valid connections) and remove the connection from graph
            Edges.Where(x => x.input.node == node).ToList().ForEach(edge => _targetGraphView.RemoveElement(edge));

            //Remove the node from graph
            _targetGraphView.RemoveElement(node);
        }
    }
}
#endif