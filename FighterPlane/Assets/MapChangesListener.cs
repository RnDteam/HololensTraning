using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapChangesListener : MonoBehaviour {

    private Vector3 defaultScale;

    private void Awake()
    {
        transform.localScale = new Vector3(1f, 1f, 1f);

        defaultScale = transform.localScale;

        MapMovement.Instance.Moved += MapMoved;
        MapMovement.Instance.ZoomChanged += MapZoomChanged;
    }

    private void MapZoomChanged()
    {
        transform.localScale = MapMovement.Instance.AbsoluteZoomRatio * defaultScale;
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y * MapMovement.Instance.CurrentZoomRatio, transform.localPosition.z);
    }

    private void MapMoved()
    {
        var newPosition = transform.position + MapMovement.Instance.MovementVector;
        transform.position = new Vector3(newPosition.x, transform.position.y, newPosition.z);
    }
}
