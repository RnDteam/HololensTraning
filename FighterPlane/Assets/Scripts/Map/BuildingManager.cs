using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour {

    
    private GameObject selectedBuilding;

    public GameObject Map;
    public Material SelectedBuildingMaterial;
    public Material UnselectedBuildingMaterial;

    void Start () {
        Map.GetComponent<OnlineMapsBuildings>().OnBuildingCreated += LoadBuildings;
	}

    private void LoadBuildings(OnlineMapsBuildingBase building)
    {
        
    }
    
}
