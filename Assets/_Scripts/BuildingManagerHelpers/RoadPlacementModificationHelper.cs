using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadPlacementModificationHelper : StructureModificationHelper
{
    Dictionary<Vector3Int, GameObject> existingRoadStructuresToBeModified = new Dictionary<Vector3Int, GameObject>();

    public RoadPlacementModificationHelper(StructureRepository structureRepository, GridStructure grid, IPlacementManager placementManager, IResourceManager resourceManager) : base(structureRepository, grid, placementManager, resourceManager)
    {
    }

    public override void PrepareStructureForModification(Vector3 inputPosition, string structureName, StructureType structureType)
    {
        base.PrepareStructureForModification(inputPosition, structureName, structureType);
        Vector3 gridPosition = grid.CalculateGridPosition(inputPosition);
        if (grid.IsCellTaken(gridPosition) == false)
        {
            var gridPositionInt = Vector3Int.FloorToInt(gridPosition);
            var roadStructure = RoadManager.GetCorrectRoadPrefab(gridPosition, structureData, structuresToBeModified, grid);
            if (structuresToBeModified.ContainsKey(gridPositionInt))
            {
                RevokeRoadPlacementAt(gridPositionInt);
                resourceManager.AddMoney(structureData.placementCost);
            }
            else if (resourceManager.CanIBuyIt(structureData.placementCost))
            {
                PlaceNewRoadAt(roadStructure, gridPosition, gridPositionInt);
                resourceManager.SpendMoney(structureData.placementCost);
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
            var neighboursStructure = RoadManager.GetCorrectRoadPrefab(neighbourGridPosition.Value, structureData, structuresToBeModified, grid);
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



    public override void CancelModification()
    {
        resourceManager.AddMoney(structuresToBeModified.Count * structureData.placementCost);
        base.CancelModification();
        existingRoadStructuresToBeModified.Clear();
    }

    public override void ConfirmModification()
    {
        RoadManager.ModifyRoadCellsOnTheGrid(existingRoadStructuresToBeModified, structureData, structuresToBeModified, grid, placementManager);
        base.ConfirmModification();
    }


}
