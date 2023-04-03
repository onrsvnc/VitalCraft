using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ZonePlacementHelper : StructureModificationHelper
{
    Vector3 mapBottomLeftCorner;
    Vector3 startPoint;
    Vector3? previousEndPosition = null;
    bool startPositionAcquired = false;
    Queue<GameObject> gameObjectsToReuse = new Queue<GameObject>();
    private int structuresOldQuantity = 0;


    public ZonePlacementHelper(StructureRepository structureRepository, GridStructure grid, IPlacementManager placementManager, Vector3 mapBottomLeftCorner, IResourceManager resourceManager) : base(structureRepository, grid, placementManager, resourceManager)
    {
        this.mapBottomLeftCorner = mapBottomLeftCorner;
    }

    public override void PrepareStructureForModification(Vector3 inputPosition, string structureName, StructureType structureType)
    {
        base.PrepareStructureForModification(inputPosition, structureName, structureType);
        Vector3 gridPosition = grid.CalculateGridPosition(inputPosition);
        if (startPositionAcquired == false && grid.IsCellTaken(gridPosition) == false)
        {
            startPoint = gridPosition;
            startPositionAcquired = true;
        }
        if (startPositionAcquired && (previousEndPosition == null || ZoneCalculator.CheckIfPositionHasChanged(gridPosition, previousEndPosition.Value, grid)))
        {
            PlaceNewZoneUpToPosition(gridPosition);
        }
    }

    private void PlaceNewZoneUpToPosition(Vector3 endPoint)
    {
        Vector3Int minPoint = Vector3Int.FloorToInt(startPoint);
        Vector3Int maxPoint = Vector3Int.FloorToInt(endPoint);

        ZoneCalculator.PrepareStartAndEndPoints(startPoint, endPoint, ref minPoint, ref maxPoint, mapBottomLeftCorner);
        HashSet<Vector3Int> newPositionsSet = grid.GetAllPositionsFromTo(minPoint, maxPoint);

        newPositionsSet = CalculateZoneCost(newPositionsSet);

        previousEndPosition = endPoint;
        ZoneCalculator.CalculateZone(newPositionsSet, structuresToBeModified, gameObjectsToReuse);

        foreach (var positionToPlaceStructure in newPositionsSet)
        {
            if (grid.IsCellTaken(positionToPlaceStructure) == true)
                continue;
            GameObject structureToAdd = null;
            if (gameObjectsToReuse.Count > 0)
            {
                var gameObjectToReuse = gameObjectsToReuse.Dequeue();
                gameObjectToReuse.SetActive(true);
                structureToAdd = placementManager.MoveStructureOnTheMap(positionToPlaceStructure, gameObjectToReuse, structureData.prefab);
            }
            else
            {
                structureToAdd = placementManager.CreateGhostStructure(positionToPlaceStructure, structureData.prefab);
            }
            structuresToBeModified.Add(positionToPlaceStructure, structureToAdd);
        }
    }

    private HashSet<Vector3Int> CalculateZoneCost(HashSet<Vector3Int> newPositionsSet)
    {
        resourceManager.AddMoney(structuresOldQuantity * structureData.placementCost);
        int numberOfZonesToPlace = resourceManager.HowManyStructuresCanIPlace(structureData.placementCost, newPositionsSet.Count);
        if (numberOfZonesToPlace < newPositionsSet.Count)
        {
            newPositionsSet = new HashSet<Vector3Int>(newPositionsSet.Take(numberOfZonesToPlace).ToList());
        }
        structuresOldQuantity = newPositionsSet.Count;
        resourceManager.SpendMoney(structuresOldQuantity * structureData.placementCost);
        return newPositionsSet;
    }

    public override void CancelModification()
    {
        resourceManager.AddMoney(structuresOldQuantity * structureData.placementCost);
        base.CancelModification();
        ResetZonePlacementHelper();
    }

    private void ResetZonePlacementHelper()
    {
        structuresOldQuantity = 0;
        placementManager.DestroyStructures(gameObjectsToReuse);
        gameObjectsToReuse.Clear();
        startPositionAcquired = false;
        previousEndPosition = null;
    }

    public override void ConfirmModification()
    {
        if (structureData.GetType() == typeof(ZoneStructureSO) && ((ZoneStructureSO)structureData).zoneType == ZoneType.Residential)
        {
            resourceManager.AddToPopulation(structuresToBeModified.Count);
        }
        base.ConfirmModification();
        ResetZonePlacementHelper();
    }

    public override void StopContinuousPlacement()
    {
        startPositionAcquired = false;
        base.StopContinuousPlacement();
    }
}