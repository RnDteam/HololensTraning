using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ScaleFixer : MonoBehaviour {

    private OnlineMapsBuildings onlineMapsBuildings;
    //private OnlineMapsTileSetControl onlineMapsTileSetControl;

    private float min = 0;

    void Start()
    {
        onlineMapsBuildings = gameObject.GetComponent<OnlineMapsBuildings>();
        //onlineMapsTileSetControl = gameObject.GetComponent<OnlineMapsTileSetControl>();
        onlineMapsBuildings.OnBuildingCreated += SetBuildingScale;
        onlineMapsBuildings.OnBuildingCreated += SetGameObjectScale;
        //onlineMapsTileSetControl.OnMeshUpdated += SetMapHeight;
    }
    private void SetMapHeight()
    {
        var minY = gameObject.GetComponent<MeshFilter>().sharedMesh.vertices.Select(v => v.y).Min();
        
        //if mesh is flat
        if (minY == 0 || minY == min)
            return;

        min = minY;
        var transformedDistance = transform.TransformPoint(new Vector3(0, minY, 0));
        Vector3 worldScale = GetWorldScale(transform);
        Debug.Log("distance = " + minY * GetWorldScale(transform).y);
        //Transform position = transform;
        //position.Translate(Vector3.up * minY * GetWorldScale(transform).y, Space.World);
        //transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        transform.Translate(-Vector3.up * minY * GetWorldScale(transform).y, Space.World);
        //onlineMapsTileSetControl.OnMeshUpdated -= SetMapHeight;
    }

    public static Vector3 GetWorldScale(Transform transform)
    {
        Vector3 worldScale = transform.localScale;
        Transform parent = transform.parent;

        while (parent != null)
        {
            worldScale = Vector3.Scale(worldScale, parent.localScale);
            parent = parent.parent;
        }

        return worldScale;
    }


    private void SetGameObjectScale(OnlineMapsBuildingBase b)
    {
        transform.FindChild("Buildings").localScale = Vector3.one;
        onlineMapsBuildings.OnBuildingCreated -= SetGameObjectScale;
    }

    private void SetBuildingScale(OnlineMapsBuildingBase b)
    {
        b.transform.localScale = Vector3.one;
    }
}
