using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IResourceManager
{
    int StartMoneyAmount { get; }
    float MoneyCalculationInterval { get; }
    int DemolitionPrice { get; }

    void AddMoney(int amount);
    void CalculateTownIncome();
    bool CanIBuyIt(int amount);
    int HowManyStructuresCanIPlace(int placementCost, int count);
    bool SpendMoney(int amount);
    void PrepareResourceManager(BuildingManager buildingManager);
}