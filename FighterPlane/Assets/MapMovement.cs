using System;
using UnityEngine;

public delegate void MapMoved();
public delegate void MapZoomChanged();  

public class MapMovement : MonoBehaviour {

    public int DefaultZoom { get; private set; }
    public int PreviousZoom { get; private set; }
    public float AbsoluteZoomRatio { get; private set; }
    public float CurrentZoomRatio { get; private set; }

    private Vector2 previousCoords;
    public Vector3 MovementVector;

    #region events
    public event MapMoved Moved;
    public event MapZoomChanged ZoomChanged;

    protected virtual void OnMapMoved()
    {
        if (Moved != null)
        {
            Moved();
        }
    }

    protected virtual void OnZoomChanged()
    {
        if (ZoomChanged != null)
        {
            ZoomChanged();
        }
    }
    #endregion

    private void SetPosition(out Vector2 coords)
    {
        double lng, lat;
        OnlineMaps.instance.GetPosition(out lng, out lat);
        coords.x = (float)lng;
        coords.y = (float)lat;
    }

    void Start () {
        DefaultZoom = PreviousZoom = OnlineMaps.instance.zoom;
        AbsoluteZoomRatio = 1f;
        CurrentZoomRatio = 1f;
        MovementVector = Vector3.zero;

        SetPosition(out previousCoords);

        OnlineMaps.instance.OnChangeZoom += ChangeZoom;
        OnlineMaps.instance.OnChangePosition += ChangePosition;
    }

    private void ChangePosition()
    {
        var previousPosition = OnlineMapsTileSetControl.instance.GetWorldPosition(previousCoords);
        SetPosition(out previousCoords);
        var currentPosition = OnlineMapsTileSetControl.instance.GetWorldPosition(previousCoords);
        MovementVector = currentPosition - previousPosition;
    }

    private void ChangeZoom()
    {
        AbsoluteZoomRatio = (float)Math.Pow(2, OnlineMaps.instance.zoom - DefaultZoom);
        CurrentZoomRatio = (float)Math.Pow(2, OnlineMaps.instance.zoom - PreviousZoom);
        PreviousZoom = OnlineMaps.instance.zoom;
    }
}
