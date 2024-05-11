using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridData
{
    Dictionary<Vector3Int, PlacementData> placedObjects = new(); //Creates the dictionary

    public void AddObject(Vector3Int gridPosition, Vector2Int objectSize, int Id, int selectedObjectId) //Lets you add objects to the dictionary
    {
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize); //Gets which cells to occupy
        PlacementData data = new PlacementData(positionToOccupy, Id, selectedObjectId); //Gets the data ready to be written

        foreach (var pos in positionToOccupy) //Checks if slots are occupied
        {
            if(placedObjects.ContainsKey(pos)) //If they are occupied throws a exception
                    throw new Exception($"Slot occupied {pos}"); //Throws the exception
            placedObjects[pos] = data; //Writes the data to the dictionary
        }
    }

    private List<Vector3Int> CalculatePositions(Vector3Int gridPosition, Vector2Int objectSize) //Used to calculate what cells objects are/are going to be placed in;
    {
        List<Vector3Int> returnVal = new(); //List to write the values in
        for (int x = 0; x < objectSize.x; x++) //Uses the size on x to calculate what cells to occupy
        {
            for (int y = 0; y < objectSize.y; y++) //Uses the size on y to calculate what cells to occupy
            {
                returnVal.Add(gridPosition + new Vector3Int(x, 0, y)); //Writes the value to returnVal
            }
        }
        return returnVal; //Returns the cells that should/are occupied
    }

    public bool Blocked(Vector3Int gridPosition, Vector2Int objectSize) //Used to check if a object is blocked
    {
        List<Vector3Int> positionsToOccupy = CalculatePositions(gridPosition, objectSize); //Gets the position a object is in
        foreach (var pos in positionsToOccupy) //Check the positions
        {
            if(placedObjects.ContainsKey(pos)) //If the position contains somthing
                return true; //Returns that it is blocked
        }
        return false; //Otherwise it says that it isn't blocked
    }
}


public class PlacementData //Dictionary
{
    public List<Vector3Int> occupiedPositions; //Stores the occupied positions
    public int Id {get; private set;} //Stores the id of the object
    public int selectedObjectId {get; private set;} //Gets the selectedObjectId

    public PlacementData(List<Vector3Int> occupiedPositions, int id, int selectedObjectId)//Allows for other scripts to set data
    {
        this.occupiedPositions = occupiedPositions; //Sets the occupied positions
        this.Id = id; //Sets the id
        this.selectedObjectId = selectedObjectId; //Sets the selected object id
    }
}
