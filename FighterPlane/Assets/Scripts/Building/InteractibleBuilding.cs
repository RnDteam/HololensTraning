using UnityEngine;

public class InteractibleBuilding : MonoBehaviour {
    
    private BuildingDisplay display;

    #region for debug
    //private bool isSelected = false;
    
    public bool IsSelected = false;
    private bool sentToBuildingManager = false;
    #endregion


    private void Awake()
    {
        display = GetComponent<BuildingDisplay>();
    }

    private void Update()
    {
        if (IsSelected && !sentToBuildingManager)
        {
            OnSelect();
            sentToBuildingManager = true;
        }
        if (!IsSelected && sentToBuildingManager)
        {
            OnSelect();
            sentToBuildingManager = false;
        }
    }

    #region select

    //TODO: For Debug only
    public void Select()
    {
        //display.Select();
        IsSelected = true;
    }

    //TODO: For Debug only
    public void Unselect()
    {
        //display.Unselect();
        IsSelected = false;
    }

    void OnSelect()
    {
        BuildingManager.Instance.SelectBuilding(gameObject);
    }
    #endregion
    
}
