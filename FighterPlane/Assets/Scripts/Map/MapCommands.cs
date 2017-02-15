﻿using Academy.HoloToolkit.Unity;
using HoloToolkit;
using HoloToolkit.Unity;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public partial class MapCommands : Singleton<MapCommands> {
    
    private OnlineMapsBuildings buildings;
    private OnlineMapsLimits limits;

    public float MovementFactor = .25f;

    public Color SelectedBuildingColor;
    public GameObject TextPrefab;
    public GameObject ExplosionPrefab;
    public GameObject RuinBuildingPrefab;

    private bool mapLocked = false;

    private void Start()
    {
        buildings = OnlineMapsBuildings.instance;
        limits = GetComponent<OnlineMapsLimits>();
        
        buildings.OnBuildingCreated += InitializeBuilding;
    }

    private void Update()
    {
        if (!mapLocked && GestureManager.Instance.IsNavigating)
        {
            var motionvector = Camera.main.transform.TransformVector(GestureManager.Instance.NavigationPosition);
            MoveMap(motionvector);
        }
        GetGazePosition();
    }

    private void MoveMap(Vector3 direction)
    {
        double px, pz, tlx, tly, brx, bry, dx, dy;

        OnlineMaps.instance.GetPosition(out px, out pz);
        OnlineMaps.instance.GetTopLeftPosition(out tlx, out tly);
        OnlineMaps.instance.GetBottomRightPosition(out brx, out bry);

        OnlineMapsUtils.DistanceBetweenPoints(tlx, tly, brx, bry, out dx, out dy);

        double mx = (brx - tlx) / dx;
        double my = (tly - bry) / dy;
        
        double ox = MovementFactor * mx * direction.x * Time.deltaTime;
        double oy = MovementFactor * my * direction.z * Time.deltaTime;

        px += ox;
        pz += oy;

        OnlineMaps.instance.SetPosition(px, pz);
    }
    

    private void InitializeBuilding(OnlineMapsBuildingBase building)
    {
        building.gameObject.layer = LayerMask.NameToLayer("Map");
        building.gameObject.AddComponent<InteractibleBuilding>();
        building.gameObject.AddComponent<BuildingWeapon>();

        var buildingDisplay = building.gameObject.AddComponent<BuildingDisplay>();
        buildingDisplay.SetText();
        buildingDisplay.SelectedBuildingColor = SelectedBuildingColor;
        buildingDisplay.ExplosionPrefab = ExplosionPrefab;
        buildingDisplay.RuinBuildingPrefab = RuinBuildingPrefab;

        var textHolder = Instantiate(TextPrefab, Vector3.zero, Quaternion.identity);
        textHolder.transform.parent = building.gameObject.transform;
        textHolder.transform.localPosition = new Vector3(0, 120, 0);
        textHolder.transform.localRotation = Quaternion.identity;
        textHolder.transform.localScale = Vector3.one * 2;
        buildingDisplay.TextHolder = textHolder;

        if (BuildingManager.Instance.IsBuildingSelected && building.id == BuildingManager.Instance.SelectedBuildingId)
        {
            if (Contains(building.centerCoordinates))
            {
                building.gameObject.SetActive(true);
                BuildingManager.Instance.ReselectBuilding(building.gameObject);
            }
        }
    }

    public void ZoomIn()
    {
       ZoomToGaze(1);
    }

    public void ZoomOut()
    {
        ZoomToGaze(-1);
    }

    private void ZoomToGaze(int zoomDifference)
    {
        if (mapLocked || OnlineMaps.instance.zoom >= limits.maxZoom)
            return;

        var target = GetGazePosition();
        OnlineMaps.instance.SetPositionAndZoom(target.x, target.y, OnlineMaps.instance.zoom + zoomDifference);
    }
    
    private Vector2 GetGazePosition()
    {
        double lng, lat;
        OnlineMaps.instance.GetPosition(out lng, out lat);
        Vector2 target = new Vector2((float)lng, (float)lat);

        RaycastHit hitInfo;
        bool hit = Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitInfo, LayerMask.GetMask("Map"));
        if (hit)
        {
            target = OnlineMapsTileSetControl.instance.GetCoordsByWorldPosition(hitInfo.point);
        }
        return target;
    }

    public void ZoomToBuilding()
    {
        if (!mapLocked && BuildingManager.Instance.IsBuildingSelected)
        {
            var coords = BuildingManager.Instance.SelectedBuildingCoords;
            OnlineMaps.instance.SetPositionAndZoom(coords.x, coords.y, 18);
        }
    }

    public void MoveToPlane()
    {
        var coords = OnlineMapsTileSetControl.instance.GetCoordsByWorldPosition(PlaneManager.Instance.GetPlaneCenter());
        OnlineMaps.instance.SetPosition(coords.x, coords.y);
    }

    public void ResetApp()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public bool Contains(Vector2 coord)
    {
        return coord.x >= OnlineMaps.instance.topLeftPosition.x && coord.x <= OnlineMaps.instance.bottomRightPosition.x
            && coord.y >= OnlineMaps.instance.bottomRightPosition.y && coord.y <= OnlineMaps.instance.topLeftPosition.y;
    }

    public void LockMap()
    {
        mapLocked = true;
    }

    public void UnlockMap()
    {
        mapLocked = false;
    }
}
