using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    UnityEngine.GameObject structureModel = null;
    List<GameObject> natureElementList = new List<GameObject>();
    StructureBaseSO structureData;
    bool isTaken = false;

    public bool IsTaken { get => isTaken; }

    public void SetConstruction(UnityEngine.GameObject structureModel, StructureBaseSO structureData)
    {
        if (structureModel == null)
            return;
        this.structureData = structureData;
        this.structureModel = structureModel;
        this.isTaken = true;
    }

    public UnityEngine.GameObject GetStructure()
    {
        return structureModel;
    }

    public void RemoveStructure()
    {
        structureModel = null;
        structureData = null;
        isTaken = false;
    }

    public StructureBaseSO GetStructureData()
    {
        return structureData;
    }

    public void AddNatureObject(GameObject element)
    {
        natureElementList.Add(element);
    }

    public List<GameObject> GetNatureOnThisCell()
    {
        return natureElementList;
    }
}

