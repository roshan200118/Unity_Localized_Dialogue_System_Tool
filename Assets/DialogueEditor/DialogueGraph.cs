using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

#if UNITY_EDITOR
//Class to create the editor window that displays the graph view
public class DialogueGraph : EditorWindow
{
    //Declaring a variable to show the actual graph view
    private DialogueGraphView _graphView;

    //Declaring a variable to store the file name (graph to load/save)
    private string _fileName = "New Narrative";

    //Create menu item to open the Dialogue Graph
    [MenuItem("Graph/Dialogue Graph")]
    public static void OpenDialogueGraphWindow()
    {
        var window = GetWindow<DialogueGraph>();
        window.titleContent = new GUIContent("Dialogue Graph");
    }

    //Method to handle when opening editor window
    private void OnEnable()
    {
        ConstructGraphView();
        GenerateToolbar();
        GenerateMiniMap();
    }

    //Method to create the graph view in the editor window
    private void ConstructGraphView()
    {
        //Create graph view
        _graphView = new DialogueGraphView
        {
            name = "Dialogue Graph"
        };

        //Stretch to size of editor window
        _graphView.StretchToParentSize();

        //Add the graph view to the window
        rootVisualElement.Add(_graphView);
    }

    //Method to generate a toolbar for the dialouge graph editor window 
    private void GenerateToolbar()
    {
        //Creating a toolbar 
        var toolbar = new Toolbar();

        //Creating a TextField for user to input the name of the file to be loaded (Dialogue graphs are stored as an asset)
        var fileNameTextField = new TextField("File Name:");

        //Assign a file name
        fileNameTextField.SetValueWithoutNotify(_fileName);

        //Refresh the TextField when user inputs values and add it to the TextField
        fileNameTextField.MarkDirtyRepaint();
        fileNameTextField.RegisterValueChangedCallback(evt => _fileName = evt.newValue);
        toolbar.Add(fileNameTextField);

        //Create buttons for Save and Load (to save a Dialogue Graph and to load an existing Dialogue Graph)
        toolbar.Add(new Button(() => RequestDataOperation(true)) { text = "Save Data" });
        toolbar.Add(new Button(() => RequestDataOperation(false)) { text = "Load Data" });

        //Creating a button to create a new Dialogue Node
        var nodeCreateButton = new Button(() =>
        {
            _graphView.CreateNode("Dialogue Node");
        });
        nodeCreateButton.text = "Create Node";

        //Add the button to the toolbar
        toolbar.Add(nodeCreateButton);

        //Add the toolbar to the editor window
        rootVisualElement.Add(toolbar);
    }

    //Method to create a mini-map (useful is Dialoge Graph gets too big)
    private void GenerateMiniMap()
    {
        //Create an anchored mini-map and set its position
        var miniMap = new MiniMap { anchored = true };
        miniMap.SetPosition(new Rect(10, 30, 200, 140));

        //Add the mini-map to the graph view
        _graphView.Add(miniMap);
    }

    //Method to handle the Load and Save operations
    //True if user wants to Save
    //False if user wants to Load
    private void RequestDataOperation(bool save)
    {
        //If the file name is null or empty, display Invalid Pop-up
        if (string.IsNullOrEmpty(_fileName))
        {
            EditorUtility.DisplayDialog("Invalid file name!", "Please enter a valid file name.", "OK");
            return;
        }

        //Create new instance of GraphSaveUtility
        var saveUtility = GraphSaveUtility.GetInstance(_graphView);

        //If user wants to save, save the Dialogue Graph
        if (save)
        {
            saveUtility.SaveGraph(_fileName);
        }

        //If user wants to load, load the Dialogue Graph
        else
        {
            saveUtility.LoadGraph(_fileName);
        }
    }

    //Method to handle when closing editor window
    private void OnDisable()
    {
        //Remove the current graph view
        rootVisualElement.Remove(_graphView);
    }

}
#endif