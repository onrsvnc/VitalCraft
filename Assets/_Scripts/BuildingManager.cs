using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager
{
    GridStructure grid;
    IPlacementManager placementManager;
    StructureRepository structureRepository;
    StructureModificationFactory helperFactory;
    StructureModificationHelper helper;
    

    public BuildingManager(int cellSize, int width, int length, IPlacementManager placementManager, StructureRepository structureRepository)
    {
        this.grid = new GridStructure(cellSize, width, length);
        this.placementManager = placementManager;
        this.structureRepository = structureRepository;
        this.helperFactory = new StructureModificationFactory(structureRepository,grid,placementManager);
        
    }

    public void PrepareBuildingManager(Type classType)
    {
        helper = helperFactory.GetHelper(classType);
    }

    public void PrepareStructureForModification(Vector3 inputPosition, string structureName, StructureType structureType)
    {
        helper.PrepareStructureForModification(inputPosition, structureName, structureType);
    }

    public void ConfirmModification()
    {
        helper.ConfirmModification();
    }

    public void CancelModification()
    {
        helper.CancelModification();
    }

    public void PrepareStructureForDemolitionAt(Vector3 inputPosition)
    {
        helper.PrepareStructureForModification(inputPosition, "", StructureType.None);
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
        structureToReturn = helper.AccessStructureInDictionary(gridPosition);
        if(structureToReturn != null)
        {
            return structureToReturn;
        }
        structureToReturn = helper.AccessStructureInDictionary(gridPosition);
        return structureToReturn;
    }




}
