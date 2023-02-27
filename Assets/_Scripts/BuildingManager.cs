using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager
{
    GridStructure grid;
    PlacementManager placementManager;
    StructureRepository structureRepository;

    public BuildingManager(int cellSize, int width, int length, PlacementManager placementManager, StructureRepository structureRepository)
    {
        this.grid = new GridStructure(cellSize, width, length);
        this.placementManager = placementManager;
        this.structureRepository = structureRepository;
    }

    public void PlaceStructureAt(Vector3 inputPosition, string structureName, StructureType structureType)
    {
        GameObject buildingPrefab = this.structureRepository.GetBuildingPrefabByName(structureName, structureType);
        Vector3 gridPosition = grid.CalculateGridPosition(inputPosition);
        if (!grid.IsCellTaken(gridPosition))
        {
            placementManager.CreateBuilding(gridPosition, grid, buildingPrefab);
        }
    }

    public void RemoveBuildingAt(Vector3 inputPosition)
    {
        Vector3 gridPosition = grid.CalculateGridPosition(inputPosition);
        if (grid.IsCellTaken(gridPosition))
        {
            placementManager.RemoveBuilding(gridPosition, grid);
        }
    }


}
