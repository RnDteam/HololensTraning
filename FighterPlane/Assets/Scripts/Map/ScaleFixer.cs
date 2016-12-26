using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ScaleFixer : MonoBehaviour {

    private OnlineMapsBuildings onlineMapsBuildings;

    void Start()
    {
        onlineMapsBuildings = gameObject.GetComponent<OnlineMapsBuildings>();
        onlineMapsBuildings.OnBuildingCreated += SetBuildingScale;
        onlineMapsBuildings.OnBuildingCreated += SetGameObjectScale;
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
