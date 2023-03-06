using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleStructurePlacementHelper : StructureModificationHelper
{
    public SingleStructurePlacementHelper(StructureRepository structureRepository, GridStructure grid, PlacementManager placementManager) : base(structureRepository, grid, placementManager)
    {
    }

    public override void PrepareStructureForModification(Vector3 inputPosition, string structureName, StructureType structureType)
    {
        GameObject buildingPrefab = this.structureRepository.GetBuildingPrefabByName(structureName, structureType);
        Vector3 gridPosition = grid.CalculateGridPosition(inputPosition);
        var gridPositionInt = Vector3Int.FloorToInt(gridPosition);
        if (grid.IsCellTaken(gridPosition) == false)
        {
            if (structuresToBeModified.ContainsKey(gridPositionInt))
            {
                RevokeStructurePlacementAt(gridPositionInt);
            }
            else
            {
                PlaceNewStructureAt(buildingPrefab, gridPosition, gridPositionInt);
            }
        }
    }

    public override void ConfirmModification()
    {
        placementManager.PlaceStructuresOnTheMap(structuresToBeModified.Values);
        foreach (var keyValuePair in structuresToBeModified)
        {
            grid.PlaceStructureOnTheGrid(keyValuePair.Value, keyValuePair.Key);
        }
        structuresToBeModified.Clear();
    }

    public override void CancelModification()
    {
        placementManager.DestroyStructures(structuresToBeModified.Values);
        structuresToBeModified.Clear();
    }

    private void PlaceNewStructureAt(GameObject buildingPrefab, Vector3 gridPosition, Vector3Int gridPositionInt)
    {
        structuresToBeModified.Add(gridPositionInt, placementManager.CreateGhostStructure(gridPosition, buildingPrefab));
    }

    private void RevokeStructurePlacementAt(Vector3Int gridPositionInt)
    {
        var structure = structuresToBeModified[gridPositionInt];
        placementManager.DestroySingleStructure(structure);
        structuresToBeModified.Remove(gridPositionInt);
    }

    


}
