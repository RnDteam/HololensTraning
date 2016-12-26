using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCommands : MonoBehaviour {

    private OnlineMaps onlineMaps;
    private OnlineMapsBuildings buildings;
    private OnlineMapsLimits limits;

    public Material[] Materials;
    public GameObject TextPrefab;

    void Start()
    {
        onlineMaps = GetComponent<OnlineMaps>();
        buildings = GetComponent<OnlineMapsBuildings>();
        limits = GetComponent<OnlineMapsLimits>();

        buildings.OnBuildingCreated += InitializeBuilding;
    }

    private void InitializeBuilding(OnlineMapsBuildingBase building)
    {
        var interactible = building.gameObject.AddComponent<InteractibleBuilding>();
        interactible.buildingManager = GetComponent<BuildingManager>();

        var textHolder = Instantiate(TextPrefab, Vector3.zero, Quaternion.identity);
        textHolder.transform.parent = building.gameObject.transform;
        textHolder.transform.localPosition = new Vector3(0, 100, 0);
        textHolder.transform.localRotation = Quaternion.identity;
        textHolder.transform.localScale = Vector3.one;

        interactible.TextHolder = textHolder;
    }

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
