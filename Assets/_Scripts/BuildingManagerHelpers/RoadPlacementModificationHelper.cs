using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadPlacementModificationHelper : StructureModificationHelper
{
    Dictionary<Vector3Int, GameObject> existingRoadStructuresToBeModified = new Dictionary<Vector3Int, GameObject>();

    public RoadPlacementModificationHelper(StructureRepository structureRepository, GridStructure grid, IPlacementManager placementManager) : base(structureRepository, grid, placementManager)
    {
    }

    public override void PrepareStructureForModification(Vector3 inputPosition, string structureName, StructureType structureType)
    {
        base.PrepareStructureForModification(inputPosition, structureName, structureType);
        Vector3 gridPosition = grid.CalculateGridPosition(inputPosition);
        if (grid.IsCellTaken(gridPosition) == false)
        {
            var gridPositionInt = Vector3Int.FloorToInt(gridPosition);
            var roadStructure = GetCorrectRoadPrefab(gridPosition, structureData);
            if (structuresToBeModified.ContainsKey(gridPositionInt))
            {
                RevokeRoadPlacementAt(gridPositionInt);
            }
            else
            {
                PlaceNewRoadAt(roadStructure, gridPosition, gridPositionInt);
            }
            AdjustNeighboursIfAreRoadStructures(gridPosition);
        }

    }

    private void AdjustNeighboursIfAreRoadStructures(Vector3 gridPosition)
    {
        AdjustNeighboursIfRoad(gridPosition, Direction.Up);
        AdjustNeighboursIfRoad(gridPosition, Direction.Down);
        AdjustNeighboursIfRoad(gridPosition, Direction.Right);
        AdjustNeighboursIfRoad(gridPosition, Direction.Left);
    }

    private void AdjustNeighboursIfRoad(Vector3 gridPosition, Direction direction)
    {
        var neighbourGridPosition = grid.GetPositionOfTheNeighbourIfExists(gridPosition, direction);
        if (neighbourGridPosition.HasValue)
        {
            var neighbourPositionInt = neighbourGridPosition.Value;
            AdjustStructureIfIsInDictionary(neighbourGridPosition, neighbourPositionInt);
            AdjustStructureIfIsOnGrid(neighbourGridPosition, neighbourPositionInt);
        }
    }

    private void AdjustStructureIfIsOnGrid(Vector3Int? neighbourGridPosition, Vector3Int neighbourPositionInt)
    {
        if (RoadManager.CheckIfNeighbourIsRoadOnTheGrid(grid, neighbourPositionInt))
        {
            var neighbourStructureData = grid.GetStructureDataFromTheGrid(neighbourGridPosition.Value);
            if (neighbourStructureData != null && neighbourStructureData.GetType() == typeof(RoadStructureSO) && existingRoadStructuresToBeModified.ContainsKey(neighbourPositionInt) == false)
            {
                existingRoadStructuresToBeModified.Add(neighbourPositionInt, grid.GetStructureFromTheGrid(neighbourGridPosition.Value));
            }
        }
    }

    private void AdjustStructureIfIsInDictionary(Vector3Int? neighbourGridPosition, Vector3Int neighbourPositionInt)
    {
        if (RoadManager.CheckIfNeighbourIsRoadInDictionary(neighbourPositionInt, structuresToBeModified))
        {
            RevokeRoadPlacementAt(neighbourPositionInt);
            var neighboursStructure = GetCorrectRoadPrefab(neighbourGridPosition.Value, structureData);
            PlaceNewRoadAt(neighboursStructure, neighbourGridPosition.Value, neighbourPositionInt); //Instantiating a new road instead of rotating the existing one.(Not optimal)
        }
    }

    private void PlaceNewRoadAt(RoadStructureHelper roadStructure, Vector3 gridPosition, Vector3Int gridPositionInt)
    {
        structuresToBeModified.Add(gridPositionInt, placementManager.CreateGhostStructure(gridPosition, roadStructure.RoadPrefab, roadStructure.RoadPrefabRotation));
    }

    private void RevokeRoadPlacementAt(Vector3Int gridPositionInt)
    {
        var structure = structuresToBeModified[gridPositionInt];
        placementManager.DestroySingleStructure(structure);
        structuresToBeModified.Remove(gridPositionInt);
    }

    private RoadStructureHelper GetCorrectRoadPrefab(Vector3 gridPosition, StructureBaseSO structureData)
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

    public override void CancelModification()
    {
        base.CancelModification();
        existingRoadStructuresToBeModified.Clear();
    }

    public override void ConfirmModification()
    {
        ModifyRoadCellsOnTheGrid(existingRoadStructuresToBeModified, structureData); 
        base.ConfirmModification();
    }

    public void ModifyRoadCellsOnTheGrid(Dictionary<Vector3Int, GameObject> neighbourDictionary, StructureBaseSO structureData)
    {
        foreach (var keyValuePair in neighbourDictionary)
        {
            grid.RemoveStructureFromTheGrid(keyValuePair.Key);
            placementManager.DestroySingleStructure(keyValuePair.Value);
            var roadStructure = GetCorrectRoadPrefab(keyValuePair.Key, structureData);
            var stucture = placementManager.PlaceStructureOnTheMap(keyValuePair.Key, roadStructure.RoadPrefab, roadStructure.RoadPrefabRotation);
            grid.PlaceStructureOnTheGrid(stucture, keyValuePair.Key, structureData);
        }
        neighbourDictionary.Clear();
    }
}
