using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager
{
    GridStructure grid;
    PlacementManager placementManager;
    StructureRepository structureRepository;
    StructureModificationHelper singleStructurePlacementHelper;
    StructureModificationHelper structureDemolitionHelper;

    public BuildingManager(int cellSize, int width, int length, PlacementManager placementManager, StructureRepository structureRepository)
    {
        this.grid = new GridStructure(cellSize, width, length);
        this.placementManager = placementManager;
        this.structureRepository = structureRepository;
        singleStructurePlacementHelper = new SingleStructurePlacementHelper(structureRepository, grid, placementManager);
        structureDemolitionHelper = new StructureDemolitionHelper(structureRepository, grid, placementManager);
    }

    public void PrepareStructureForPlacement(Vector3 inputPosition, string structureName, StructureType structureType)
    {
        singleStructurePlacementHelper.PrepareStructureForModification(inputPosition, structureName, structureType);
    }

    public void ConfirmPlacement()
    {
        singleStructurePlacementHelper.ConfirmModification();
    }

    public void CancelPlacement()
    {
        singleStructurePlacementHelper.CancelModification();
    }

    public void PrepareStructureForDemolitionAt(Vector3 inputPosition)
    {
        structureDemolitionHelper.PrepareStructureForModification(inputPosition, "", StructureType.None);
    }

    public void CancelDemolition()
    {
        structureDemolitionHelper.CancelModification();
    }

    public void ConfirmDemolition()
    {
        structureDemolitionHelper.ConfirmModification();
    }

    //This method is created for unit testing purposes.
    public GameObject CheckForStructureInGrid(Vector3 inputPosition)
    {
        Vector3 gridPosition = grid.CalculateGridPosition(inputPosition);
        if (grid.IsCellTaken(gridPosition))
        {
            return grid.GetStructureFromTheGrid(gridPosition);
        }
        return null;
    }

    //This method is created for unit testing purposes.
    public GameObject CheckForStructureInDictionary(Vector3 inputPosition)
    {
        Vector3 gridPosition = grid.CalculateGridPosition(inputPosition);
        GameObject structureToReturn = null;
        structureToReturn = singleStructurePlacementHelper.AccessStructureInDictionary(gridPosition);
        if(structureToReturn != null)
        {
            return structureToReturn;
        }
        structureToReturn = structureDemolitionHelper.AccessStructureInDictionary(gridPosition);
        return structureToReturn;
    }




}
