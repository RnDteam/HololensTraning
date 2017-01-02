using HoloToolkit;
using UnityEngine;

public class LocationManager : Singleton<LocationManager> {
    protected LocationManager() { }

    public void GetCoordsByWorldPosition(Transform transform, out double longitude, out double latitude, out double altitude) {
        var coords = OnlineMapsTileSetControl.instance.GetCoordsByWorldPosition(transform.TransformPoint(transform.position));
        longitude = coords.x;
        latitude = coords.y;
        altitude = CalculateHeight(transform);
    }

    private double CalculateHeight(Transform transform)
    {
        var selfY = transform.localPosition.y;

        var point1 = OnlineMapsTileSetControl.instance.GetCoordsByWorldPosition(transform.TransformPoint(transform.position));
        var point2 = OnlineMapsTileSetControl.instance.GetCoordsByWorldPosition(transform.TransformPoint(transform.position + Vector3.forward));
        var planeUnit = OnlineMapsUtils.DistanceBetweenPoints(point1, point2).magnitude;

        return (selfY / planeUnit + OnlineMapsTileSetControl.instance.elevationMinValue) / 100;
    }
}
