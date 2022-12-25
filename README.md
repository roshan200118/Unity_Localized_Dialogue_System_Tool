# Unity Localized Dialogue System Tool

The goal of this project is to create a node-based dialogue system using C#, Unity's GraphView API, Unity3D Editor API, the Model-View-Controller pattern and OOP. (2022).

#### This Localized Dialogue System Tool:
* Uses **CustomEditor** for the creation, modification and deletion of branching response trees for dialogues, with multiple responses per prompt
* This implementation supports creating, editing and saving many dialogue trees
* The dialogue information is encoded as **ScriptableObjects** and the in-game UI displays the prompt and responses
* Integrated the [Localization Tool](https://github.com/roshan200118/Unity_Localization_Tool "Localization Tool") to support localization by storing keys from the Language data and updating the in-game UI based on the chosen language to populate the dialogue system at run-time
* Uses ScriptableObjects to store the dialogue tree data
* Created own custom Node Editor using **Unity's GraphView API**
* The dialogue system supports localization by storing keys from the Language data and updating the in-game UI based on the chosen language.

#### How to use:

* Navigate to Unity Edtitor Menu, select the "Graph" menu item and then select the "Dialogue Graph" option
  * This will open the Dialogue Graph window

##

* Use mouse scroll to zoom in/out
* Hold "Alt" and drag to move around
* Click node in mini-map to navigate to it

##

* To create a Dialogue Node, click the "Create Node" button at the top of the window
  * Drag "Input" port to a "Choice" port to create a connection
  * Click the "New Choice" button on a Dialogue Node to create a new choice
    * Click the "X" button to the left of a choice to remove it
    * Drag the connector from an "Input" port to the corresponding choice port to make the choice correspond to the target Dialogue node
  * Change the Dialogue text by inputting into the TextField at bottom of Dialogue Node

##

* To remove a Dialogue Node, right click it and select "Delete"

##

* To **save** a graph, type in the desired filename for the graph at the top of the Dialogue Graph editor window and click "Save Data"
  * This will create a new asset with the filename in the Resources folder

##

* To **load/edit** a graph, type in the desired graph's filename and click "Load Data"

#### Using In-Game UI:

* Attach DialogueParser.cs script to a GameObject
* In the "Dialogue" property, add the desired saved dialogue graph from Resources
* Add the dialogue TextField component to "Dialogue Text Field"
* In the "Option Button" field, add a button prefab
* In the "Button Containers Array" field, add the button containers transfrom
* Run the game and can change Language dynamically using Localization Tool



https://user-images.githubusercontent.com/61467608/209458178-b3a4bd95-f117-4686-978f-369d8ad5a17a.mp4


https://user-images.githubusercontent.com/61467608/209458179-ae8e0644-da14-4aa9-bb24-8a13e03a0a47.mp4


https://user-images.githubusercontent.com/61467608/209458180-832f0259-f403-4ccd-a7ab-3a90f5c33f44.mp4


https://user-images.githubusercontent.com/61467608/209458182-72dfa7a6-2c8d-4322-8a36-f55055a2c57a.mp4
