using HoloToolkit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildingManager : Singleton<BuildingManager> 
{
    protected BuildingManager() { }

    private GameObject selectedBuilding = null;
    public bool infoVisiblity = false;
    
    public GameObject SelectedBuilding
    {
        get
        {
            return selectedBuilding;
        }
    }

    public bool IsBuildingSelected
    {
        get
        {
            return SelectedBuilding != null;
        }
    }

    public void SelectBuilding(GameObject gameObject)
    {
        if (selectedBuilding == gameObject)
        {
            selectedBuilding.GetComponent<InteractibleBuilding>().Unselect();
            selectedBuilding.GetComponent<InteractibleBuilding>().HideInfo();
            selectedBuilding = null;
        }
        else
        {
            if (selectedBuilding != null)
            {
                selectedBuilding.GetComponent<InteractibleBuilding>().Unselect();
                selectedBuilding.GetComponent<InteractibleBuilding>().HideInfo();
            }

            selectedBuilding = gameObject;
            selectedBuilding.GetComponent<InteractibleBuilding>().Select();

            if (infoVisiblity)
            {
                selectedBuilding.GetComponent<InteractibleBuilding>().ShowInfo();
            }
        }
    }

    public void ShowInfo()
    {
        infoVisiblity = true;
        if (IsBuildingSelected)
        {
            selectedBuilding.GetComponent<InteractibleBuilding>().ShowInfo();
        }
    }

    public void HideInfo()
    {
        infoVisiblity = false;
        if (IsBuildingSelected)
        {
            selectedBuilding.GetComponent<InteractibleBuilding>().HideInfo();
        }
    }
}
