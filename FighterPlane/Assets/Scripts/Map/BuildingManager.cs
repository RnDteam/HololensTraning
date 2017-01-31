using HoloToolkit;
using System.Collections;
using System.Linq;
using UnityEngine;

public partial class BuildingManager : Singleton<BuildingManager> 
{
    private GameObject selectedBuilding = null;
    private string selectedBuildingId = null;
    private Vector2 selectedBuildingCoords;
    public ArrayList desroidBuildingsList = new ArrayList();

    public string BuildingKeyword;

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
            building.GetComponent<InteractibleBuilding>().Select(); //TODO: delete after debug
            building.GetComponent<BuildingDisplay>().Select();
            if (infoVisiblity)
            {
                building.GetComponent<BuildingDisplay>().ShowInfo();
            }
        }
        else
        {
            building.GetComponent<InteractibleBuilding>().Unselect(); //TODO: delete after debug
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
        //if we're selecting the selected building == unselcting a building
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

    public GameObject getBuildingById(string buildingId)
    {
        return transform.Find("Buildings/" + buildingId).gameObject;
        //OnlineMapsBuildings.instance.buildings.ContainsKey(buildingId) ? 
        //    OnlineMapsBuildings.instance.buildings[buildingId].gameObject : null;
    }

    public void SelectBuildingById(string id)
    {
        if (id == null)
        {
            return;
        }
        if (OnlineMapsBuildings.instance.buildings.ContainsKey(id))
        {
            SelectBuilding(getBuildingById(id));
        }
        else
        {
            selectedBuildingId = id;
            selectedBuilding = null;
        }
    }
        
    private string GetValue(OnlineMapsBuildingBase building, string attribute)
    {
        if (building.metaInfo.Count(p => p.title == attribute) == 1)
        {
            return building.metaInfo.Single(p => p.title == attribute).info;
        }
        return null;
    }

    private string NameToId(string name)
    {
        if (OnlineMapsBuildings.instance.buildings.Count(b => GetValue(b.Value, "name:en") == name) == 1) {
            return (OnlineMapsBuildings.instance.buildings.Single(b => GetValue(b.Value, "name:en") == name).Value).id;
        }
        return null;
    }

    private bool BuildingIsReachable(string name)
    {
        return NameToId(name) != null;
    }

    public void SelectBuildingVoiceCommand()
    {
        Debug.Log("BuildingKeyword = " + BuildingKeyword);
        if (BuildingKeyword != null)
        {
            var id = NameToId(BuildingKeyword);
            if (id == null)
            {
                Debug.Log("Building not found!");
            }
            else
            {
                Debug.Log("BuildingID = " + id);
                SelectBuildingById(id);
            }
        }
    }

    public void SelectGrandCanyon()
    {
        SelectBuildingById("39383661");
    }

    public void SelectHouse()
    {
        SelectBuildingById("183294007");
    }

    
}
