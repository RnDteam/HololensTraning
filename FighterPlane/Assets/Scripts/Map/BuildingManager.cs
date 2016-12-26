using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour {

    private GameObject selectedBuilding;

    public void SelectBuilding(GameObject gameObject)
    {
        if (selectedBuilding)
        {
            selectedBuilding.GetComponent<InteractibleBuilding>().IsSelected = false;
        }
        selectedBuilding = gameObject;
    }
}
