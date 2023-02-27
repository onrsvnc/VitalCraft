using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private Action<string> OnBuildZoneHandler;
    private Action<string> OnBuildSingleStructureHandler;
    private Action<string> OnBuildRoadHandler;

    private Action OnCancelActionHandler;
    private Action OnDemolishActionHandler;

    public StructureRepository structureRepository;
    public Button buildResidentialAreaButton; //This unused button is kept for testing purposes, as tests won't run without it.
    public Button cancelActionButton;
    public UnityEngine.GameObject cancelActionPanel;

    public UnityEngine.GameObject buildingMenuPanel;
    public Button openBuildMenuButton;
    public Button demolishButton;

    public GameObject zonesPanel;
    public GameObject facilitiesPanel;
    public GameObject roadsPanel;
    public Button closeBuildMenuButton;

    public GameObject buildButtonPrefab;

    void Start()
    {
        buildingMenuPanel.SetActive(false);
        cancelActionPanel.SetActive(false);
       //buildResidentialAreaButton.onClick.AddListener(OnBuildAreaCallback);
        cancelActionButton.onClick.AddListener(OnCancelActionCallback);
        openBuildMenuButton.onClick.AddListener(OnOpenBuildMenu);
        demolishButton.onClick.AddListener(OnDemolishHandler);
        closeBuildMenuButton.onClick.AddListener(OnCloseMenuHandler);
    }

    private void OnCloseMenuHandler()
    {
        buildingMenuPanel.SetActive(false);
    }

    private void OnDemolishHandler()
    {
        OnDemolishActionHandler?.Invoke();
        cancelActionPanel.SetActive(true);
        OnCloseMenuHandler();
    }

    private void OnOpenBuildMenu()
    {
        buildingMenuPanel.SetActive(true);
        PrepareBuildMenu();
    }

    public void OnBuildZoneCallback(string nameOfStructure) //Made public for testing.
    {
        PrepareUIForBuilding();
        OnBuildZoneHandler?.Invoke(nameOfStructure);
    }

    public void OnBuildRoadCallback(string nameOfStructure) //Made public for testing.
    {
        PrepareUIForBuilding();
        OnBuildRoadHandler?.Invoke(nameOfStructure);
    }

    public void OnBuildSingleStructureCallback(string nameOfStructure) //Made public for testing.
    {
        PrepareUIForBuilding();
        OnBuildSingleStructureHandler?.Invoke(nameOfStructure);
    }

    private void PrepareUIForBuilding()
    {
        cancelActionPanel.SetActive(true);
        OnCloseMenuHandler();
    }

    private void OnCancelActionCallback()
    {
        cancelActionPanel.SetActive(false);
        OnCancelActionHandler?.Invoke();
    }

    private void PrepareBuildMenu()
    {
        CreateButtonsInPanel(zonesPanel.transform, structureRepository.GetZoneNames(), OnBuildZoneCallback);
        CreateButtonsInPanel(roadsPanel.transform, new List<string>() { structureRepository.GetRoadStructureName() }, OnBuildRoadCallback);
        CreateButtonsInPanel(facilitiesPanel.transform, structureRepository.GetSingleStructureNames(), OnBuildSingleStructureCallback);
    }

    private void CreateButtonsInPanel(Transform panelTransform, List<string> dataToShow, Action<string> callback)
    {
        if(dataToShow.Count > panelTransform.childCount)
        {
            int quantityDifference = dataToShow.Count-panelTransform.childCount;
            for (int i = 0; i < quantityDifference; i++)
            {
                Instantiate(buildButtonPrefab,panelTransform);
            }
        }
        for (int i = 0; i < panelTransform.childCount; i++)
        {
            var button = panelTransform.GetChild(i).GetComponent<Button>();
            if (button != null)
            {
                button.GetComponentInChildren<TextMeshProUGUI>().SetText(dataToShow[i]); //SetText() used instead of <TextMeshProUGUI>().text = dataToShow[i]; to avoid creating a new string object every time the text is updated. Might result in a bug?
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(()=>callback(button.GetComponentInChildren<TextMeshProUGUI>().text)); 
            }
        }
    }

    public void AddListenerOnBuildZoneEvent(Action<string> listener)
    {
        OnBuildZoneHandler += listener;
    }

    public void RemoveListenerOnBuildZoneEvent(Action<string> listener)
    {
        OnBuildZoneHandler -= listener;
    }


    public void AddListenerOnCancelActionEvent(Action listener)
    {
        OnCancelActionHandler += listener;
    }

    public void RemoveListenerOnCancelActionEvent(Action listener)
    {
        OnCancelActionHandler -= listener;
    }

    public void AddListenerOnDemolishActionEvent(Action listener)
    {
        OnDemolishActionHandler += listener;
    }

    public void RemoveListenerOnDemolishActionEvent(Action listener)
    {
        OnDemolishActionHandler -= listener;
    }

    public void AddListenerOnBuildSingleStructureEvent(Action<string> listener)
    {
        OnBuildSingleStructureHandler += listener;
    }

    public void RemoveListenerOnBuildSingleStructureEvent(Action<string> listener)
    {
        OnBuildSingleStructureHandler -= listener;
    }

    public void AddListenerOnBuildRoadEvent(Action<string> listener)
    {
        OnBuildRoadHandler += listener;
    }

    public void RemoveListenerOnBuildRoadEvent(Action<string> listener)
    {
        OnBuildRoadHandler -= listener;
    }




}
