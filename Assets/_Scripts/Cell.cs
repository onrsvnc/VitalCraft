using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    UnityEngine.GameObject structureModel = null;
    bool isTaken = false;

    public bool IsTaken { get => isTaken; }

    public void SetConstruction(UnityEngine.GameObject structureModel)
    {
        if(structureModel == null)
            return;
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
        isTaken = false;
    }
}

