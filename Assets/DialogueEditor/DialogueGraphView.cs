using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

#if UNITY_EDITOR
//Class to create the actual graph view
public class DialogueGraphView : GraphView
{
    //Declaring a variable to store the default node size
    public readonly Vector2 DefaultNodeSize = new Vector2(150, 200);

    //Constructor
    public DialogueGraphView()
    {
        //Use the style sheets for the graph
        styleSheets.Add(Resources.Load<StyleSheet>("DialogueGraph"));

        //Allows zooming in graph view
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

        //Allow these functions on the graph view
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        //Create a grid, attach it to the view and stretch to whole editor window
        var grid = new GridBackground();
        Insert(0, grid);
        grid.StretchToParentSize();

        //Add the first node to graph view
        AddElement(GenerateEntryPointNode());
    }


    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        //Create list to store compatible ports
        var compatiblePorts = new List<Port>();

        //Loop through each port
        ports.ForEach((port) =>
        {
            //If the input port is not connected to the output port
            if (startPort != port && startPort.node != port.node)
            {
                //Add it to compatible ports
                compatiblePorts.Add(port);
            }
        });

        return compatiblePorts;
    }

    /// <summary>
    /// Method to create a port for a dialogue node
    /// </summary>
    /// <param name="node">Target node</param>
    /// <param name="portDirection">Direction of port (input or output)</param>
    /// <param name="capacity">Show many connections to a node (single in our case)</param>
    /// <returns></returns>
    private Port GeneratePort(DialogueNode node, Direction portDirection, Port.Capacity capacity = Port.Capacity.Single)
    {
        return node.InstantiatePort(Orientation.Horizontal, portDirection, capacity, typeof(float));
    }

    //Method to create the first node
    private DialogueNode GenerateEntryPointNode()
    {
        //Creating a new node
        var node = new DialogueNode
        {
            title = "Start",
            GUID = Guid.NewGuid().ToString(),
            EntryPoint = true,  //This is the first node
        };

        //Create the "Next" port
        var generatedPort = GeneratePort(node, Direction.Output);
        generatedPort.portName = "Next";
        node.outputContainer.Add(generatedPort);

        //Entry point node is not movable or deletable
        //Need entry point node at all times
        node.capabilities &= ~Capabilities.Movable;
        node.capabilities &= ~Capabilities.Deletable;

        //Refresh the container after adding the port
        node.RefreshExpandedState();

        //Refresh layout of ports
        node.RefreshPorts();

        //Set the position of the node
        node.SetPosition(new Rect(x: 100, y: 200, width: 100, height: 150));
        return node;
    }

    //Method to create a new node
    public void CreateNode(string nodeName)
    {
        AddElement(CreateDialogueNode(nodeName));
    }

    //Method to create a new dialouge node
    public DialogueNode CreateDialogueNode(string nodeName)
    {
        //Create a new dialogue node
        var dialogueNode = new DialogueNode
        {
            title = nodeName,
            DialogueText = nodeName,
            GUID = Guid.NewGuid().ToString(),
        };

        //Create the input node (multiple because various options can lead to same dialogue)
        var inputPort = GeneratePort(dialogueNode, Direction.Input, Port.Capacity.Multi);
        inputPort.portName = "Input";
        dialogueNode.inputContainer.Add(inputPort);

        //Add the styling for node
        dialogueNode.styleSheets.Add(Resources.Load<StyleSheet>("Node"));

        //Create the "New Choice" button 
        var button = new Button(() =>
        {
            AddChoicePort(dialogueNode);
        });
        button.text = "New Choice";
        dialogueNode.titleContainer.Add(button);

        //Create a TextField for the Dialogue Node's title
        var textField = new TextField(string.Empty);

        //When user inputs for Dialogue Node's title, update it
        textField.RegisterValueChangedCallback(evt =>
        {
            dialogueNode.DialogueText = evt.newValue;
            dialogueNode.title = evt.newValue;
        });
        textField.SetValueWithoutNotify(dialogueNode.title);
        dialogueNode.mainContainer.Add(textField);

        //Refresh the properties of the Dialogue Node then set it's position
        //Refresh the container after adding the port
        dialogueNode.RefreshExpandedState();
        dialogueNode.RefreshPorts();
        dialogueNode.SetPosition(new Rect(Vector2.zero, DefaultNodeSize));

        return dialogueNode;
    }


    /// <summary>
    /// Method to add a choice port
    /// </summary>
    /// <param name="dialogueNode">Dialogue Node to add choice port to</param>
    /// <param name="overridenPortName">Port name coming from existing graph, override if exists</param>
    public void AddChoicePort(DialogueNode dialogueNode, string overridenPortName = "")
    {
        //Create a port for the choice
        var generatedPort = GeneratePort(dialogueNode, Direction.Output);

        //Port comes with existing label, so save it into variable
        var oldLabel = generatedPort.contentContainer.Q<Label>("type");

        //Remove the label
        generatedPort.contentContainer.Remove(oldLabel);

        //Store all the ports in the output container of dialogue node
        var outputPortCount = dialogueNode.outputContainer.Query("connector").ToList().Count;

        //Assign the choice port name by number
        //If overridenPortName exists (loading a graph) then assign that name
        var choicePortName = string.IsNullOrEmpty(overridenPortName) ? $"Choice {outputPortCount + 1}" : overridenPortName;

        //Creating a textfield for the ChoicePort
        var textField = new TextField
        {
            name = string.Empty,
            value = choicePortName,
        };

        //If user inputs into textField, update it
        textField.RegisterValueChangedCallback(evt => generatedPort.portName = evt.newValue);

        //Add the textfield to the choice port
        generatedPort.contentContainer.Add(new Label(" "));
        generatedPort.contentContainer.Add(textField);

        //Creating a button to remove a choice port
        var deleteButton = new Button(() => RemovePort(dialogueNode, generatedPort))
        {
            text = "X"
        };

        //Add the button to the choice port
        generatedPort.contentContainer.Add(deleteButton);

        //Assign the port name
        generatedPort.portName = choicePortName;

        //Add the choice port to the output container
        dialogueNode.outputContainer.Add(generatedPort);

        //Refresh the container after adding the port and the layout of the port
        dialogueNode.RefreshExpandedState();
        dialogueNode.RefreshPorts();
    }

    //Method to remove a port
    private void RemovePort(DialogueNode dialogueNode, Port generatedPort)
    {
        //Get target connection
        var targetEdge = edges.ToList().Where(x => x.output.portName == generatedPort.portName && x.output.node == generatedPort.node);

        //If there is a 
        if (!targetEdge.Any())
        {
            //Get the connection's first edge
            var edge = targetEdge.First();

            //Disconnect the connection
            edge.input.Disconnect(edge);

            //Remove the connection
            RemoveElement(targetEdge.First());
        }

        //Remove the desired port from output container
        dialogueNode.outputContainer.Remove(generatedPort);

        //Refresh the container after adding the port and the layout of the port
        dialogueNode.RefreshPorts();
        dialogueNode.RefreshExpandedState();
    }
}
#endif