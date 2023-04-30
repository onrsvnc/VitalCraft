using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyHelper
{
    private int money;
    private int currentBalance;

    public MoneyHelper(int startMoneyAmount)
    {
        this.money = startMoneyAmount;
    }

    public int Money
    {
        get => money;
        private set
        {
            if (value < 0)
            {
                money = 0;
                throw new MoneyException("Not enough money");
            }
            else
            {
                money = value;
            }
        }
    }

    public int CurrentBalance { get => currentBalance; }

    public void ReduceMoney(int amount)
    {
        Money -= amount;
    }

    public void AddMoney(int amount)
    {
        Money += amount;
    }

    public void CalculateMoney(IEnumerable<StructureBaseSO> buildings)
    {
        CollectIncome(buildings);
        ReduceUpkeep(buildings);
        CalculateCurrentBalance(buildings);
    }

    private void ReduceUpkeep(IEnumerable<StructureBaseSO> buildings)
    {
        foreach (var structure in buildings)
        {
            Money -= structure.upkeepCost;
        }
    }

    private void CollectIncome(IEnumerable<StructureBaseSO> buildings)
    {
        foreach (var structure in buildings)
        {
            if(structure.HasRoadAccess() && structure.HasPower())
            Money += structure.GetIncome();
        }
    }

    private void CalculateCurrentBalance(IEnumerable<StructureBaseSO> buildings)
    {
        int newCurrentBalance = 0;
        foreach (var structure in buildings)
        {
            if(structure.HasRoadAccess() && structure.HasPower())
            {
                newCurrentBalance += structure.GetIncome();
            }
            newCurrentBalance -= structure.upkeepCost;
        }
        currentBalance = newCurrentBalance;
    }


    // Backup code:
    // private void ReduceUpkeep(IEnumerable<StructureBaseSO> buildings)
    // {
    //     foreach (var structure in buildings)
    //     {
    //         Money -= structure.upkeepCost;
    //     }
    // }

    // private void CollectIncome(IEnumerable<StructureBaseSO> buildings)
    // {
    //     foreach (var structure in buildings)
    //     {
    //         Money += structure.GetIncome();
    //     }
    // }


}
