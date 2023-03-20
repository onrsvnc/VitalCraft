using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour, IResourceManager
{
    [SerializeField] private int startMoneyAmount = 5000;
    [SerializeField] private int demolitionPrice = 20;
    [SerializeField] private float moneyCalculationInterval = 2;
    MoneyHelper moneyHelper;
    private BuildingManager buildingManager;
    public UIController uiController;

    public int StartMoneyAmount { get => startMoneyAmount; }
    public float MoneyCalculationInterval { get => moneyCalculationInterval; }

    int IResourceManager.DemolitionPrice { get => demolitionPrice; }

    void Start()
    {
        moneyHelper = new MoneyHelper(startMoneyAmount);
        UpdateMoneyValueUI();
    }

    public void PrepareResourceManager(BuildingManager buildingManager)
    {
        this.buildingManager = buildingManager;
        InvokeRepeating("CalculateTownIncome", 0, moneyCalculationInterval);
    }

    public bool SpendMoney(int amount)
    {
        if (CanIBuyIt(amount))
        {
            try
            {
                moneyHelper.ReduceMoney(amount);
                UpdateMoneyValueUI();
                return true;
            }
            catch (MoneyException)
            {
                ReloadGame();
            }
        }
        return false;
    }

    public void AddMoney(int amount)
    {
        moneyHelper.AddMoney(amount);
        UpdateMoneyValueUI();
    }

    private void ReloadGame()
    {
        Debug.Log("End the game");
    }

    public bool CanIBuyIt(int amount)
    {
        if (moneyHelper.Money >= amount)
        {
            return true;
        }
        return false;
    }

    public void CalculateTownIncome()
    {
        try
        {
            moneyHelper.CalculateMoney(buildingManager.GetAllStructures());
            UpdateMoneyValueUI();
        }
        catch (MoneyException)
        {
            ReloadGame();
        }
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void UpdateMoneyValueUI()
    {
        uiController.SetMoneyValue(moneyHelper.Money);
    }

    public int HowManyStructuresCanIPlace(int placementCost, int numberOfStructures)
    {
        int amount = (int)(moneyHelper.Money / placementCost);
        return amount > numberOfStructures ? numberOfStructures : amount;
    }

    // Update is called once per frame
    void Update()
    {

    }


}
