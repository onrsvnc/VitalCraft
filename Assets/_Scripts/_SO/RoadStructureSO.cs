using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Road Structure", menuName = "CityBuilder/StructureData/RoadStructure")]
public class RoadStructureSO : StructureBaseSO
{
    [Tooltip("Road Facing Up & Right")]
    public UnityEngine.GameObject cornerPrefab;
    [Tooltip("Road Facing Up & Right & Down")]
    public UnityEngine.GameObject threewayPrefab;
    public UnityEngine.GameObject fourwayPrefab;
    public RotationValue prefabRotation = RotationValue.R0;

    public void PrepareRoad(IEnumerable<StructureBaseSO> structuresAround)
    {
        foreach (var nearbyStructure in structuresAround)
        {
            nearbyStructure.PrepareStructure(new StructureBaseSO[] { this });
        }
    }

    public IEnumerable<StructureBaseSO> PrepareRoadDemolition(IEnumerable<StructureBaseSO> structuresAround)
    {
        List<StructureBaseSO> listToReturn = new List<StructureBaseSO>();
        foreach (var nearbyStructure in structuresAround)
        {
            if(nearbyStructure.RoadProvider == this)
            {
                nearbyStructure.RemoveRoadProvider();
                listToReturn.Add(nearbyStructure);
            }
            
        }
        return listToReturn;
    }
    


}

public enum RotationValue
{
    R0,
    R90,
    R180,
    R270
}
