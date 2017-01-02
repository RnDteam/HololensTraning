using System;
using HoloToolkit.Unity;
using UnityEngine;
using UnityEngine.VR.WSA.Input;

public class MapCommands : MonoBehaviour {

    private OnlineMaps onlineMaps;
    private OnlineMapsBuildings buildings;
    private OnlineMapsLimits limits;

    public float MovementFactor = .25f;

    public Color SelectedBuildingColor;
    public GameObject TextPrefab;
    private int function;

    private void Start()
    {
        onlineMaps = OnlineMaps.instance;
        buildings = OnlineMapsBuildings.instance;
        limits = GetComponent<OnlineMapsLimits>();

        //onlineMaps.OnChangePosition += PositionChanged;

        buildings.OnBuildingCreated += InitializeBuilding;
    }

    private void Update()
    {
        if (GestureManager.Instance.IsNavigating)
        {
            var motionvector = Camera.main.transform.TransformVector(GestureManager.Instance.NavigationPosition);
            MoveMap(motionvector);
        }
        GetGazePosition();
    }

    private void MoveMap(Vector3 direction)
    {
        double px, pz, tlx, tly, brx, bry, dx, dy;

        onlineMaps.GetPosition(out px, out pz);
        onlineMaps.GetTopLeftPosition(out tlx, out tly);
        onlineMaps.GetBottomRightPosition(out brx, out bry);

        OnlineMapsUtils.DistanceBetweenPoints(tlx, tly, brx, bry, out dx, out dy);

        double mx = (brx - tlx) / dx;
        double my = (tly - bry) / dy;
        
        double ox = MovementFactor * mx * direction.x * Time.deltaTime;
        double oy = MovementFactor * my * direction.z * Time.deltaTime;

        px += ox;
        pz += oy;

        onlineMaps.SetPosition(px, pz);
    }
    

    private void InitializeBuilding(OnlineMapsBuildingBase building)
    {
        building.gameObject.layer = 31;
        var interactible = building.gameObject.AddComponent<InteractibleBuilding>();
        var buildingDisplay = building.gameObject.AddComponent<BuildingDisplay>();
        buildingDisplay.SelectedBuildingColor = SelectedBuildingColor;

        var textHolder = Instantiate(TextPrefab, Vector3.zero, Quaternion.identity);
        textHolder.transform.parent = building.gameObject.transform;
        textHolder.transform.localPosition = new Vector3(0, 100, 0);
        textHolder.transform.localRotation = Quaternion.identity;
        textHolder.transform.localScale = Vector3.one;
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
        if (onlineMaps.zoom >= limits.maxZoom)
            return;

        var target = GetGazePosition();
        onlineMaps.SetPositionAndZoom(target.x, target.y, onlineMaps.zoom + zoomDifference);
    }
    
    private Vector2 GetGazePosition()
    {
        double lng, lat;
        onlineMaps.GetPosition(out lng, out lat);
        Vector2 target = new Vector2((float)lng, (float)lat);

        RaycastHit hitInfo;
        bool hit = Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitInfo, LayerMask.GetMask("Map"));
        if (hit)
        {
            target = OnlineMapsTileSetControl.instance.GetCoordsByWorldPosition(hitInfo.point);
        }

        Debug.Log(string.Format("{0:0.000000} {1:0.000000}", target.x, target.y));
        return target;
    }

    public void ZoomToBuilding()
    {
        if (BuildingManager.Instance.IsBuildingSelected)
        {
            var coords = BuildingManager.Instance.SelectedBuildingCoords;
            onlineMaps.SetPositionAndZoom(coords.x, coords.y, 18);
        }
    }
    
    public bool Contains(Vector2 coord)
    {
        return coord.x >= onlineMaps.topLeftPosition.x && coord.x <= onlineMaps.bottomRightPosition.x
            && coord.y >= onlineMaps.bottomRightPosition.y && coord.y <= onlineMaps.topLeftPosition.y;
    }

    
}
