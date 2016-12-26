using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCommands : MonoBehaviour {

    private OnlineMaps onlineMaps;
    private OnlineMapsBuildings buildings;
    private OnlineMapsLimits limits;

    public Material[] Materials;

    void Start()
    {
        onlineMaps = GetComponent<OnlineMaps>();
        buildings = GetComponent<OnlineMapsBuildings>();
        limits = GetComponent<OnlineMapsLimits>();

        buildings.OnBuildingCreated += InitializeBuilding;
        //buildings.OnBuildingCreated += initializeMaterials;
    }

    private void InitializeBuilding(OnlineMapsBuildingBase building)
    {
        var interactible = building.gameObject.AddComponent<InteractibleBuilding>();
        interactible.buildingManager = GetComponent<BuildingManager>();
    }

    //private void initializeMaterials(OnlineMapsBuildingBase building)
    //{
    //    var go = building.gameObject;
    //    go.GetComponent<InteractibleBuilding>().defaultMaterials = Materials;
    //}

    public void ZoomIn()
    {
        if (onlineMaps.zoom < limits.maxZoom)
            onlineMaps.zoom++;
    }

    public void ZoomOut()
    {
        if (onlineMaps.zoom > limits.minZoom)
            onlineMaps.zoom--;
    }
}
