using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridData
{
    Dictionary<Vector3Int, PlacementData> placedObjects = new();

    public void AddObject(Vector3Int gridPosition, Vector2Int objectSize, int Id, int selectedObjectId)
    {
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);
        PlacementData data = new PlacementData(positionToOccupy, Id, selectedObjectId);

        foreach (var pos in positionToOccupy)
        {
            if(placedObjects.ContainsKey(pos))
                    throw new Exception($"Slot occupied {pos}");
            placedObjects[pos] = data;
        }
    }

    private List<Vector3Int> CalculatePositions(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> returnVal = new();
        for (int x = 0; x < objectSize.x; x++)
        {
            for (int y = 0; y < objectSize.y; y++)
            {
                returnVal.Add(gridPosition + new Vector3Int(x, 0, y));
            }
        }
        return returnVal;
    }

    public bool Blocked(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> positionsToOccupy = CalculatePositions(gridPosition, objectSize);
        foreach (var pos in positionsToOccupy)
        {
            if(placedObjects.ContainsKey(pos))
                return true;
        }
        return false;
    }
}


public class PlacementData
{
    public List<Vector3Int> occupiedPositions;
    public int Id {get; private set;}
    public int selectedObjectId {get; private set;}

    public PlacementData(List<Vector3Int> occupiedPositions, int id, int selectedObjectId)
    {
        this.occupiedPositions = occupiedPositions;
        Id = id;
        this.selectedObjectId = selectedObjectId;
    }
}
