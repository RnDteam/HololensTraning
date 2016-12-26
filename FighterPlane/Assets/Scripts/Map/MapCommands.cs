using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCommands : MonoBehaviour {

    private OnlineMaps onlineMaps;
    private OnlineMapsLimits limits;

    void Start()
    {
        onlineMaps = GetComponent<OnlineMaps>();
        limits = GetComponent<OnlineMapsLimits>();
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
