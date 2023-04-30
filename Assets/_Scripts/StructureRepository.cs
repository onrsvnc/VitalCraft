using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StructureRepository : MonoBehaviour
{
    public CollectionSO modelDataCollection;


    public List<string> GetZoneNames()
    {
        return modelDataCollection.zonesList.Select(zone => zone.buildingName).ToList();
    }

    public List<string> GetSingleStructureNames()
    {
        return modelDataCollection.singleStructureList.Select(facility => facility.buildingName).ToList();
    }

    public string GetRoadStructureName()
    {
        return modelDataCollection.roadStructure.buildingName;
    }

    public List<int> GetZoneCosts()
    {
        return modelDataCollection.zonesList.Select(zone => zone.placementCost).ToList();
    }

    public List<int> GetSingleStructureCosts()
    {
        return modelDataCollection.singleStructureList.Select(facility => facility.placementCost).ToList();
    }

    public int GetRoadStructureCost()
    {
        return modelDataCollection.roadStructure.placementCost;
    } 



    public GameObject GetBuildingPrefabByName(string structureName, StructureType structureType)
    {
        GameObject structurePrefabToReturn = null;
        switch (structureType)
        {
            case StructureType.Zone:
                structurePrefabToReturn = GetZoneBuildingPrefabByName(structureName);
                break;
            case StructureType.SingleStructure:
                structurePrefabToReturn = GetSingleStructureBuildingPrefabByName(structureName);
                break;
            case StructureType.Road:
                structurePrefabToReturn = GetRoadBuildingPrefabByName();
                break;
            default:
                throw new Exception("No such type. not implemented for " + structureType);
        }
        if (structurePrefabToReturn == null)
        {
            throw new Exception("No prefab for that name  " + structureName);
        }
        return structurePrefabToReturn;
    }

    private GameObject GetRoadBuildingPrefabByName()
    {
        return modelDataCollection.roadStructure.prefab;
    }

    private GameObject GetSingleStructureBuildingPrefabByName(string structureName)
    {
        var foundStructure = modelDataCollection.singleStructureList.Where(structure => structure.buildingName == structureName).FirstOrDefault();
        if (foundStructure != null)
        {
            return foundStructure.prefab;
        }
        return null;
    }

    private GameObject GetZoneBuildingPrefabByName(string structureName)
    {
        var foundStructure = modelDataCollection.zonesList.Where(structure => structure.buildingName == structureName).FirstOrDefault();
        if (foundStructure != null)
        {
            return foundStructure.prefab;
        }
        return null;
    }

    public StructureBaseSO GetStructureData(string structureName, StructureType structureType)
    {
        switch (structureType)
        {
            case StructureType.Zone:
                return modelDataCollection.zonesList.Where(structure => structure.buildingName == structureName).FirstOrDefault();
            case StructureType.SingleStructure:
                return modelDataCollection.singleStructureList.Where(structure => structure.buildingName == structureName).FirstOrDefault();
            case StructureType.Road:
                return modelDataCollection.roadStructure;
            case StructureType.None:
                return null;
        }
        return null;
    }

}

public enum StructureType
{
    Zone,
    SingleStructure,
    Road,
    None
}


