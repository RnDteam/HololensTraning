using UnityEngine;

public class LocationSync : MonoBehaviour {

    public double Latitude, Longitude;

    [Tooltip("Plane altitude in meters")]
    public double Altitude;

    private Vector2 coords;

    private void Update () {
        coords = OnlineMapsTileSetControl.instance.GetCoordsByWorldPosition(transform.TransformPoint(transform.position));
        Longitude = coords.x;
        Latitude = coords.y;
        Altitude = CalculateHeight();
    }

    private double CalculateHeight()
    {
        var selfY = transform.localPosition.y;

        var point1 = OnlineMapsTileSetControl.instance.GetCoordsByWorldPosition(transform.TransformPoint(transform.position));
        var point2 = OnlineMapsTileSetControl.instance.GetCoordsByWorldPosition(transform.TransformPoint(transform.position + Vector3.forward));
        var planeUnit = OnlineMapsUtils.DistanceBetweenPoints(point1, point2).magnitude;

        return (selfY / planeUnit + OnlineMapsTileSetControl.instance.elevationMinValue) / 100;
    }
}
