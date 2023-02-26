using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Facility Structure", menuName = "CityBuilder/StructureData/FacilityStructure")]
public class SingleFacilitySO : SingleStructureBaseSO
{
    public int maxCustomers;
    public int upkeepPerCustomer;

}
