using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RoadManager
{
    public static int GetRoadNeighboursStatus(Vector3 gridPosition, GridStructure grid, Dictionary<Vector3Int, GameObject> structuresToBeModified)
    {
        int roadNeighboursStatus = 0;
        foreach (Direction direction in Enum.GetValues(typeof(Direction)))
        {
            var neighbourPosition = grid.GetPositionOfTheNeighbourIfExists(gridPosition, direction);
            if (neighbourPosition.HasValue)
            {
                if (CheckIfNeighbourIsRoadOnTheGrid(grid, neighbourPosition) || CheckIfNeighbourIsRoadInDictionary(neighbourPosition, structuresToBeModified))
                {
                    roadNeighboursStatus += (int)direction;
                }
            }
        }
        return roadNeighboursStatus;
    }

    public static bool CheckIfNeighbourIsRoadOnTheGrid(GridStructure grid, Vector3Int? neighbourPosition)
    {
        if (grid.IsCellTaken(neighbourPosition.Value))
        {
            var neighbourStructureData = grid.GetStructureDataFromTheGrid(neighbourPosition.Value);
            if (neighbourStructureData != null && neighbourStructureData.GetType() == typeof(RoadStructureSO))
            {
                return true;
            }
        }
        return false;
    }

    public static bool CheckIfNeighbourIsRoadInDictionary(Vector3Int? neighbourPosition, Dictionary<Vector3Int, GameObject> structuresToBeModified)
    {
        if(structuresToBeModified==null)
            return false;
        return structuresToBeModified.ContainsKey(neighbourPosition.Value);
    }

    public static RoadStructureHelper CheckIfStraightRoadFits(int neighbourStatus, RoadStructureHelper roadToReturn, StructureBaseSO structureData)
    {
        if (neighbourStatus == ((int)Direction.Up | (int)Direction.Down) || neighbourStatus == (int)Direction.Up || neighbourStatus == (int)Direction.Down)
        {
            roadToReturn = new RoadStructureHelper(((RoadStructureSO)structureData).prefab, RotationValue.R90);
        }
        else if (neighbourStatus == ((int)Direction.Right | (int)Direction.Left) || neighbourStatus == (int)Direction.Right
            || neighbourStatus == (int)Direction.Left || neighbourStatus == 0)
        {
            roadToReturn = new RoadStructureHelper(((RoadStructureSO)structureData).prefab, RotationValue.R0);
        }
        return roadToReturn;
    }

    public static RoadStructureHelper CheckIfCornerRoadFits(int neighbourStatus, RoadStructureHelper roadToReturn, StructureBaseSO structureData)
    {
        if (neighbourStatus == ((int)Direction.Up | (int)Direction.Right))
        {
            roadToReturn = new RoadStructureHelper(((RoadStructureSO)structureData).cornerPrefab, RotationValue.R0);
        }
        else if (neighbourStatus == ((int)Direction.Down | (int)Direction.Right))
        {
            roadToReturn = new RoadStructureHelper(((RoadStructureSO)structureData).cornerPrefab, RotationValue.R90);
        }
        else if (neighbourStatus == ((int)Direction.Down | (int)Direction.Left))
        {
            roadToReturn = new RoadStructureHelper(((RoadStructureSO)structureData).cornerPrefab, RotationValue.R180);
        }
        else if (neighbourStatus == ((int)Direction.Up | (int)Direction.Left))
        {
            roadToReturn = new RoadStructureHelper(((RoadStructureSO)structureData).cornerPrefab, RotationValue.R270);
        }
        return roadToReturn;
    }

    public static RoadStructureHelper CheckIf3WayRoadFits(int neighbourStatus, RoadStructureHelper roadToReturn, StructureBaseSO structureData)
    {
        if (neighbourStatus == ((int)Direction.Up | (int)Direction.Right | (int)Direction.Down))
        {
            roadToReturn = new RoadStructureHelper(((RoadStructureSO)structureData).threewayPrefab, RotationValue.R0);
        }
        else if (neighbourStatus == ((int)Direction.Down | (int)Direction.Right | (int)Direction.Left))
        {
            roadToReturn = new RoadStructureHelper(((RoadStructureSO)structureData).threewayPrefab, RotationValue.R90);
        }
        else if (neighbourStatus == ((int)Direction.Down | (int)Direction.Left | (int)Direction.Up))
        {
            roadToReturn = new RoadStructureHelper(((RoadStructureSO)structureData).threewayPrefab, RotationValue.R180);
        }
        else if (neighbourStatus == ((int)Direction.Up | (int)Direction.Left | (int)Direction.Right))
        {
            roadToReturn = new RoadStructureHelper(((RoadStructureSO)structureData).threewayPrefab, RotationValue.R270);
        }
        return roadToReturn;
    }

    public static RoadStructureHelper CheckIf4WayRoadFits(int neighbourStatus, RoadStructureHelper roadToReturn, StructureBaseSO structureData)
    {
        if (neighbourStatus == ((int)Direction.Up | (int)Direction.Right | (int)Direction.Down | (int)Direction.Left))
        {
            roadToReturn = new RoadStructureHelper(((RoadStructureSO)structureData).fourwayPrefab, RotationValue.R0);
        }
        return roadToReturn;
    }

    public static Dictionary<Vector3Int, GameObject> GetRoadNeighboursForPosition(GridStructure grid, Vector3Int position)
    {
        Dictionary<Vector3Int, GameObject> dictionaryToReturn = new Dictionary<Vector3Int, GameObject>();
        List<Vector3Int?> neighbourPossibleLocations = new List<Vector3Int?>();
        neighbourPossibleLocations.Add(grid.GetPositionOfTheNeighbourIfExists(position, Direction.Up));
        neighbourPossibleLocations.Add(grid.GetPositionOfTheNeighbourIfExists(position, Direction.Down));
        neighbourPossibleLocations.Add(grid.GetPositionOfTheNeighbourIfExists(position, Direction.Left));
        neighbourPossibleLocations.Add(grid.GetPositionOfTheNeighbourIfExists(position, Direction.Right));
        foreach (var possiblePosition in neighbourPossibleLocations)
        {
            if (possiblePosition.HasValue)
            {
                if (CheckIfNeighbourIsRoadOnTheGrid(grid, possiblePosition.Value)
                && dictionaryToReturn.ContainsKey(possiblePosition.Value) == false)
                {
                    dictionaryToReturn.Add(possiblePosition.Value, grid.GetStructureFromTheGrid(possiblePosition.Value));
                }
            }
        }
        return dictionaryToReturn;
    }

    public static void ModifyRoadCellsOnTheGrid(Dictionary<Vector3Int, GameObject> neighbourDictionary, StructureBaseSO structureData, 
        Dictionary<Vector3Int, GameObject> structuresToBeModified, GridStructure grid, IPlacementManager placementManager)
    {
        foreach (var keyValuePair in neighbourDictionary)
        {
            grid.RemoveStructureFromTheGrid(keyValuePair.Key);
            placementManager.DestroySingleStructure(keyValuePair.Value);
            var roadStructure = GetCorrectRoadPrefab(keyValuePair.Key, structureData, structuresToBeModified, grid);
            var stucture = placementManager.PlaceStructureOnTheMap(keyValuePair.Key, roadStructure.RoadPrefab, roadStructure.RoadPrefabRotation);
            grid.PlaceStructureOnTheGrid(stucture, keyValuePair.Key, structureData);
        }
        neighbourDictionary.Clear();
    }

    public static RoadStructureHelper GetCorrectRoadPrefab(Vector3 gridPosition, StructureBaseSO structureData, 
        Dictionary<Vector3Int, GameObject> structuresToBeModified, GridStructure grid) //this possibly be turned into a helper object
    {
        var neighbourStatus = RoadManager.GetRoadNeighboursStatus(gridPosition, grid, structuresToBeModified);
        RoadStructureHelper roadToReturn = null;
        roadToReturn = RoadManager.CheckIfStraightRoadFits(neighbourStatus, roadToReturn, structureData);
        if (roadToReturn != null)
            return roadToReturn;
        roadToReturn = RoadManager.CheckIfCornerRoadFits(neighbourStatus, roadToReturn, structureData);
        if (roadToReturn != null)
            return roadToReturn;
        roadToReturn = RoadManager.CheckIf3WayRoadFits(neighbourStatus, roadToReturn, structureData);
        if (roadToReturn != null)
            return roadToReturn;
        roadToReturn = RoadManager.CheckIf4WayRoadFits(neighbourStatus, roadToReturn, structureData);
        return roadToReturn;
    }

}

