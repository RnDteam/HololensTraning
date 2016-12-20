using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleFixer : MonoBehaviour {

    private OnlineMapsBuildings onlineMapsBuildings;

    void Start()
    {
        onlineMapsBuildings = gameObject.GetComponent<OnlineMapsBuildings>();
        onlineMapsBuildings.OnBuildingCreated += SetBuildingScale;
        onlineMapsBuildings.OnBuildingCreated += SetGameObjectScale;
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
