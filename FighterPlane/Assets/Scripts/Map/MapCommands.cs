using System;
using HoloToolkit.Unity;
using UnityEngine;
using UnityEngine.VR.WSA.Input;

public class MapCommands : MonoBehaviour {

    private OnlineMaps onlineMaps;
    private OnlineMapsBuildings buildings;
    private OnlineMapsLimits limits;

    public float MovementFactor = 0.00005f;

    public Material[] Materials;
    public GameObject TextPrefab;

    private void Start()
    {
        onlineMaps = OnlineMaps.instance;
        buildings = OnlineMapsBuildings.instance;
        limits = GetComponent<OnlineMapsLimits>();

        buildings.OnBuildingCreated += InitializeBuilding;
    }

    private void Update()
    {
        if (GestureManager.Instance.IsNavigating)
        {
            var motionvector = Camera.main.transform.TransformVector(GestureManager.Instance.NavigationPosition);
            MoveMap(motionvector);
        }
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

        double v = (double)MovementFactor * Time.deltaTime;

        double ox = mx * v * direction.x;
        double oy = my * v * direction.z;

        px += ox;
        pz += oy;

        onlineMaps.SetPosition(px, pz);
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
