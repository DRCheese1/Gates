using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridPlacementSystem : MonoBehaviour
{
    [SerializeField] private MouseManger mouseManger; //Lets you acces the mousePositionDetector;

    [SerializeField] private ObjectListSO objectList; //Allows you to acces the list of objects
    private int selectedObjectId = -1; //Id of selected object

    [SerializeField] private Grid grid; //Lets you acces the Grid component
    [SerializeField] private GameObject mouseIndicator, cellIndicator; //Shows where you are selecting
    [SerializeField] private GameObject GridVis; //Refrences to the grid visualization

    [SerializeField] Vector3Int debug1;
    [SerializeField] Vector3Int debug2;

    private void Start() //Unity start
    {
        StopPlacement();
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
        debug1 = gridPosition;
        GameObject newObject = Instantiate(objectList.GridObjectData[selectedObjectId].Prefab); //Creates the currently selected prefab
        newObject.transform.position = grid.CellToWorld(gridPosition); //Sets the newObject to the current grid cell
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
        debug2 = gridPosition;
        mouseIndicator.transform.position = mousePosition; //Sets mouseindicator to mouseposition
        cellIndicator.transform.position = grid.CellToWorld(gridPosition); //Sets the gridIndicator to the current grid cell
    }
}
