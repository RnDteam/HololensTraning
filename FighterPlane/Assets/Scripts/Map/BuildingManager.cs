using HoloToolkit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class BuildingManager : Singleton<BuildingManager> 
{
    private GameObject selectedBuilding = null;
    private string selectedBuildingId = null;
    private Vector2 selectedBuildingCoords;

    #region properties
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
            return selectedBuildingId != null;
        }
    }

    public string SelectedBuildingId
    {
        get
        {
            return selectedBuildingId;
        }
    }

    public Vector2 SelectedBuildingCoords
    {
        get
        {
            return selectedBuildingCoords;
        }
    }
    #endregion

    private void SetBuilding(GameObject building, bool select)
    {
        if (!building)
        {
            return;
        }

        if (select)
        {
            building.GetComponent<InteractibleBuilding>().Select();
            building.GetComponent<BuildingDisplay>().Select();
            if (infoVisiblity)
            {
                building.GetComponent<BuildingDisplay>().ShowInfo();
            }
        }
        else
        {
            building.GetComponent<InteractibleBuilding>().Unselect();
            building.GetComponent<BuildingDisplay>().Unselect();
            building.GetComponent<BuildingDisplay>().HideInfo();
        }
    }

    public void SelectBuilding(GameObject gameObject)
    {
        if (gameObject == null)
        {
            return;
        }
        //if we're selecting the selected building
        if (IsBuildingSelected && selectedBuildingId == gameObject.GetComponent<OnlineMapsBuildingBase>().id)
        {
            SetBuilding(selectedBuilding, false);
            selectedBuilding = null;
            selectedBuildingId = null;
        }
        else
        {   
            //if we're selecting a new building when another is selected
            if (IsBuildingSelected)
            {
                SetBuilding(selectedBuilding, false);
            }
            
            selectedBuilding = gameObject;
            selectedBuilding.GetComponent<OnlineMapsBuildingBase>().OnDispose += buildingDisposed;
            selectedBuildingId = selectedBuilding.GetComponent<OnlineMapsBuildingBase>().id;
            selectedBuildingCoords = selectedBuilding.GetComponent<OnlineMapsBuildingBase>().centerCoordinates;
            SetBuilding(selectedBuilding, true);
        }
    }

    private void buildingDisposed(OnlineMapsBuildingBase building)
    {
        selectedBuilding = null;
    }

    public void ReselectBuilding(GameObject gameObject)
    {
        selectedBuilding = gameObject;
        SetBuilding(selectedBuilding, true);
    }

    public void ShowInfo()
    {
        infoVisiblity = true;
        if (IsBuildingSelected)
        {
            selectedBuilding.GetComponent<BuildingDisplay>().ShowInfo();
        }
    }

    public void HideInfo()
    {
        infoVisiblity = false;
        if (IsBuildingSelected)
        {
            selectedBuilding.GetComponent<BuildingDisplay>().HideInfo();
        }
    }
}
