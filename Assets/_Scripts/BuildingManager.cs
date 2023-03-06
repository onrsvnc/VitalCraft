using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager
{
    GridStructure grid;
    PlacementManager placementManager;
    StructureRepository structureRepository;
    Dictionary<Vector3Int, GameObject> structuresToBeModified = new Dictionary<Vector3Int, GameObject>();

    public BuildingManager(int cellSize, int width, int length, PlacementManager placementManager, StructureRepository structureRepository)
    {
        this.grid = new GridStructure(cellSize, width, length);
        this.placementManager = placementManager;
        this.structureRepository = structureRepository;
    }

    public void PrepareStructureForPlacement(Vector3 inputPosition, string structureName, StructureType structureType)
    {
        GameObject buildingPrefab = this.structureRepository.GetBuildingPrefabByName(structureName, structureType);
        Vector3 gridPosition = grid.CalculateGridPosition(inputPosition);
        var gridPositionInt = Vector3Int.FloorToInt(gridPosition);
        if (grid.IsCellTaken(gridPosition) == false)
        {
            if(structuresToBeModified.ContainsKey(gridPositionInt))
            {
                RemoveStructurePlacementAt(gridPositionInt);
            }
            else
            {
                PlaceNewStructureAt(buildingPrefab, gridPosition, gridPositionInt);
            }
        }
    }

    private void PlaceNewStructureAt(GameObject buildingPrefab, Vector3 gridPosition, Vector3Int gridPositionInt)
    {
        structuresToBeModified.Add(gridPositionInt, placementManager.CreateGhostStructure(gridPosition, buildingPrefab));
    }

    private void RemoveStructurePlacementAt(Vector3Int gridPositionInt)
    {
        var structure = structuresToBeModified[gridPositionInt];
        placementManager.DestroySingleStructure(structure);
        structuresToBeModified.Remove(gridPositionInt);
    }

    public void ConfirmPlacement()
    {
        placementManager.PlaceStructuresOnTheMap(structuresToBeModified.Values);
        foreach (var keyValuePair in structuresToBeModified)
        {
            grid.PlaceStructureOnTheGrid(keyValuePair.Value, keyValuePair.Key);
        }
        structuresToBeModified.Clear();
    }

    public void CancelPlacement()
    {
        placementManager.DestroyStructures(structuresToBeModified.Values);
        structuresToBeModified.Clear();
    }

    public void PrepareStructureForDemolitionAt(Vector3 inputPosition)
    {
        Vector3 gridPosition = grid.CalculateGridPosition(inputPosition);
        if (grid.IsCellTaken(gridPosition))
        {
            var gridPositionInt = Vector3Int.FloorToInt(gridPosition);
            var structure = grid.GetStructureFromTheGrid(gridPosition);
            if (structuresToBeModified.ContainsKey(gridPositionInt))
            {
                RevokeStructureDemolitionAt(gridPositionInt, structure);
            }
            else
            {
                AddStructureForDemolition(gridPositionInt, structure);
            }

        }
    }

    private void AddStructureForDemolition(Vector3Int gridPositionInt, GameObject structure)
    {
        structuresToBeModified.Add(gridPositionInt, structure);
        placementManager.SetBuildingForDemolition(structure);
    }

    private void RevokeStructureDemolitionAt(Vector3Int gridPositionInt, GameObject structure)
    {
        placementManager.ResetBuildingMaterial(structure);
        structuresToBeModified.Remove(gridPositionInt);
    }

    public void CancelDemolition()
    {
        this.placementManager.PlaceStructuresOnTheMap(structuresToBeModified.Values);
        structuresToBeModified.Clear();
    }

    public void ConfirmDemolition()
    {
        foreach (var gridPosition in structuresToBeModified.Keys)
        {
            grid.RemoveStructureFromTheGrid(gridPosition);
        }
        this.placementManager.DestroyStructures(structuresToBeModified.Values);
        structuresToBeModified.Clear();
    }

    //This method is created for unit testing purposes.
    public GameObject CheckForStructureInGrid(Vector3 inputPosition)
    {
        Vector3 gridPosition = grid.CalculateGridPosition(inputPosition);
        if(grid.IsCellTaken(gridPosition))
        {
            return grid.GetStructureFromTheGrid(gridPosition);
        }
        return null;
    }

    //This method is created for unit testing purposes.
    public GameObject CheckForStructureInDictionary(Vector3 inputPosition)
    {
        Vector3 gridPosition = grid.CalculateGridPosition(inputPosition);
        var gridPositionInt= Vector3Int.FloorToInt(gridPosition);
        if(structuresToBeModified.ContainsKey(gridPositionInt))
        {
            return structuresToBeModified[gridPositionInt];
        }
        return null;
    }




}
