using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManagerTestStub : MonoBehaviour, IResourceManager
{
    public int StartMoneyAmount { get; }

    public float MoneyCalculationInterval { get; }

    public int DemolitionPrice { get; }

    public void AddMoney(int amount)
    {
        
    }

    public void CalculateTownIncome()
    {
        
    }

    public bool CanIBuyIt(int amount)
    {
        return true;
    }

    public int HowManyStructuresCanIPlace(int placementCost, int count)
    {
        return 0;
    }

    public void PrepareResourceManager(BuildingManager buildingManager)
    {
        
    }

    public bool SpendMoney(int amount)
    {
        return true;
    }
}
