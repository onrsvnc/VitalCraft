using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject placementManagerGameObject;
    private IPlacementManager placementManager;
    public StructureRepository structureRepository;
    public IInputManager inputManager;
    public UIController uiController;
    public int width, length;
    public CameraMovement cameraMovement;
    public LayerMask inputMask;
    private BuildingManager buildingManager;
    private int cellSize = 3;

    private PlayerState state;

    public GameObject resourceManagerGameObject;
    private IResourceManager resourceManager;

    public PlayerSelectionState selectionState;
    public PlayerBuildingSingleStructureState buildingSingleStructureState;
    public PlayerDemolitionState demolishState;
    public PlayerBuildingRoadState buildingRoadState;
    public PlayerBuildingZoneState buildingZoneState;

    public NatureManager natureManager;

    public PlayerState State { get => state; } //Exposed for testing purposes

    void Awake()
    {
#if (UNITY_EDITOR && TEST) || !(UNITY_IOS || UNITY_ANDROID)
        inputManager = gameObject.AddComponent<InputManager>();
#endif
#if (UNITY_IOS || UNITY_ANDROID)

#endif        
    }

    private void PrepareStates()
    {
        buildingManager = new BuildingManager(natureManager.Grid, placementManager, structureRepository, resourceManager);
        resourceManager.PrepareResourceManager(buildingManager);
        selectionState = new PlayerSelectionState(this, buildingManager);
        demolishState = new PlayerDemolitionState(this, buildingManager);
        buildingSingleStructureState = new PlayerBuildingSingleStructureState(this, buildingManager);
        buildingZoneState = new PlayerBuildingZoneState(this, buildingManager);
        buildingRoadState = new PlayerBuildingRoadState(this, buildingManager);
        state = selectionState;
    }

    void Start()
    {
        placementManager = placementManagerGameObject.GetComponent<IPlacementManager>();
        placementManager.PreparePlacementManager(natureManager);
        resourceManager = resourceManagerGameObject.GetComponent<IResourceManager>();
        natureManager.PrepareNature(cellSize, width, length);
        PrepareStates();
        PrepareGameComponents();
        AssignInputListeners();
        AssignUIControllerListeners();
    }

    private void PrepareGameComponents()
    {
        inputManager.MouseInputMask = inputMask;
        cameraMovement.SetCameraLimits(0, width, 0, length);
    }

    private void AssignUIControllerListeners()
    {
        uiController.AddListenerOnBuildZoneEvent((structureName) => state.OnBuildZone(structureName)); //using lambda expression so that state.OnBuildArea will access the gameManager on the heap and not the stack
        uiController.AddListenerOnBuildRoadEvent((structureName) => state.OnBuildRoad(structureName));
        uiController.AddListenerOnBuildSingleStructureEvent((structureName) => state.OnBuildSingleStructure(structureName));
        uiController.AddListenerOnCancelActionEvent(() => state.OnCancel());
        uiController.AddListenerOnDemolishActionEvent(() => state.OnDemolishAction());
        uiController.AddListenerOnConfirmActionEvent(() => state.OnConfirmAction());
    }

    private void AssignInputListeners()
    {
        inputManager.AddListenerOnPointerDownEvent((position) => state.OnInputPointerDown(position));
        inputManager.AddListenerOnPointerChangeEvent((position) => state.OnInputPointerChange(position));
        inputManager.AddListenerOnPointerSecondChangeEvent((position) => state.OnInputPanChange(position));
        inputManager.AddListenerOnPointerSecondUpEvent(() => state.OnInputPanUp());
        inputManager.AddListenerOnPointerUpEvent(() => state.OnInputPointerUp());
    }

    public void TransitionToState(PlayerState newState, string variable)
    {
        this.state = newState;
        this.state.EnterState(variable);
    }





}
