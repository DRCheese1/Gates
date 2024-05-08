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
    }

    private List<Vector3Int> CalculatePositions(Vector3Int gridPosition, Vector2Int objectSize)
    {
        throw new NotImplementedException();
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
