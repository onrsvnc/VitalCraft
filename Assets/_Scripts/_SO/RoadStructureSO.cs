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
}

public enum RotationValue
{
    R0,
    R90,
    R180,
    R270
}
