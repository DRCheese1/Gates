using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridPlacementSystem : MonoBehaviour
{
    //Used for multiple things:
    [SerializeField] private MouseManger mouseManger; //Lets you acces the mousePositionDetector;
    private Renderer previewRenderer; //Used to render the gridIndicator in different ways

    //Used for Main object placement:
    [SerializeField] private ObjectListSO objectList; //Allows you to acces the list of objects
    [SerializeField] private GridData floorData, objectData; //Used for storing and checking grid datapr
    private List<GameObject> placedGameObjects = new();
    private int selectedObjectId = -1; //Id of selected object

    //Used for main grid worings:
    [SerializeField] private Grid grid; //Lets you acces the Grid component
    [SerializeField] private GameObject mouseIndicator, cellIndicator; //Shows where you are selecting
    [SerializeField] private GameObject GridVis; //Refrences to the grid visualization

    private void Start() //Unity start
    {
        StopPlacement(); //Stops placement
        floorData = new();
        objectData = new();
        previewRenderer = cellIndicator.GetComponentInChildren<Renderer>();
    }

    public void StartPlacement(int Id) //Starts placement and shows grid
    {
        selectedObjectId = objectList.GridObjectData.FindIndex(data => data.Id == Id); //Gets the id of the object
        if(selectedObjectId < 0) //Makes sure there is actually a object selected
        {
            Debug.LogError($"No ID found {Id}"); //Throws a error
            return; //Returns
        }
        GridVis.SetActive(true); //Shows the visualization for the grid
        cellIndicator.SetActive(true); //Shows the indicator for the current cell
        mouseManger.OnClicked += PlaceStructure; //Calls PlaceStructure
        mouseManger.OnExit += StopPlacement; //Stops the placement
    }

    private void PlaceStructure()
    {
        if(mouseManger.PointerUI()) //Makes sure your not clicking ui
        {
            return; //Returns if your clicking ui
        }

        Vector3 mousePosition = mouseManger.GetMousePosition(); //Gets the mousePosition from mousePositionDetector script
        Vector3Int gridPosition = grid.WorldToCell(mousePosition); //Rounds mouseposition to nearest grid cell

        bool canPlace = canPlaceTest(gridPosition, selectedObjectId);
        if(canPlace == false)
            return;

        GameObject newObject = Instantiate(objectList.GridObjectData[selectedObjectId].Prefab); //Creates the currently selected prefab
        newObject.transform.position = grid.CellToWorld(gridPosition); //Sets the newObject to the current grid cell
        placedGameObjects.Add(newObject);
        GridData selectedData = objectList.GridObjectData[selectedObjectId].Id == 0 ? floorData : objectData;
        selectedData.AddObject(gridPosition, objectList.GridObjectData[selectedObjectId].Size, objectList.GridObjectData[selectedObjectId].Id, placedGameObjects.Count -1);

    }

    private bool canPlaceTest(Vector3Int gridPosition, int selectedObjectId)
    {
        GridData selectedData = objectList.GridObjectData[selectedObjectId].Id == 0 ? floorData : objectData;
        return !selectedData.Blocked(gridPosition, objectList.GridObjectData[selectedObjectId].Size);
    }

    private void StopPlacement()
    {
        selectedObjectId = -1;
        GridVis.SetActive(false); //Hides the visualization for the grid
        cellIndicator.SetActive(false); //Hides the indicator for the current cell
        mouseManger.OnClicked -= PlaceStructure; //Stops PlaceStructure
        mouseManger.OnExit -= StopPlacement; //Makes so you can't exit multiple times
    }

    private void Update() //Unity update
    {
        if(selectedObjectId < 0) //Disables the gridIndicator when there is nothing selected
            return; //Returns
        Vector3 mousePosition = mouseManger.GetMousePosition(); //Gets the mousePosition from mousePossitionDetector script
        Vector3Int gridPosition = grid.WorldToCell(mousePosition); //Rounds mouseposition to nearest grid cell

        bool canPlace = canPlaceTest(gridPosition, selectedObjectId);
        previewRenderer.material.color = canPlace ? Color.white : Color.red;

        mouseIndicator.transform.position = mousePosition; //Sets mouseindicator to mouseposition
        Vector3 pos = grid.CellToWorld(gridPosition); //Calculates the position of the cellindicator
        pos.y = 0.01f; cellIndicator.transform.position = pos; //Sets the gridIndicator to the current grid cell
    }
}
