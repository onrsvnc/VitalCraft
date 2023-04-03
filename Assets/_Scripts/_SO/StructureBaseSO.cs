using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StructureBaseSO : ScriptableObject
{
    public string buildingName;
    public UnityEngine.GameObject prefab;
    public int placementCost;
    public int upkeepCost;
    [SerializeField] protected int income;
    public bool requireRoadAccess;
    public bool requireWater;
    public bool requirePower;
    public int structureRange = 1;
    private SingleFacilitySO powerProvider = null;
    private SingleFacilitySO waterProvider = null;
    private RoadStructureSO roadProvider = null;

    public SingleFacilitySO PowerProvider { get => powerProvider; }
    public SingleFacilitySO WaterProvider { get => waterProvider; }
    public RoadStructureSO RoadProvider { get => roadProvider; }

    public virtual int GetIncome()
    {
        return income;
    }

    public bool HasPower()
    {
        return powerProvider != null;
    }

    public bool HasWater()
    {
        return waterProvider != null;
    }

    public bool HasRoadAccess()
    {
        return roadProvider != null;
    }

    public void PrepareStructure(IEnumerable<StructureBaseSO> structuresInRange)
    {
        AddRoadProvider(structuresInRange);
    }

    public bool AddPowerFacility(SingleFacilitySO facility)
    {
        if (powerProvider == null)
        {
            powerProvider = facility;
            return true;
        }
        return false;
    }

    public bool AddWaterFacility(SingleFacilitySO facility)
    {
        if (waterProvider == null)
        {
            waterProvider = facility;
            return true;
        }
        return false;
    }

    public virtual IEnumerable<StructureBaseSO> PrepareForDestruction()
    {
        if (powerProvider != null)
        {
            powerProvider.RemoveClient(this);
        }
        if (waterProvider != null)
        {
            waterProvider.RemoveClient(this);
        }
        return null;
    }

    private void AddRoadProvider(IEnumerable<StructureBaseSO> structuresInRange)
    {
        if (roadProvider != null)
        {
            return;
        }
        foreach (var nearbyStructure in structuresInRange)
        {
            if (nearbyStructure.GetType() == typeof(RoadStructureSO))
            {
                roadProvider = (RoadStructureSO)nearbyStructure;
                return;
            }
        }
    }

    internal void RemoveWaterFacility()
    {
        waterProvider = null;
    }

    internal void RemovePowerFacility()
    {
        powerProvider = null;
    }

    internal void RemoveRoadProvider()
    {
        roadProvider = null;
    }
}
