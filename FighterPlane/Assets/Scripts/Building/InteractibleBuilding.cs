﻿using UnityEngine;

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
    private void UpdateSelection()
    {
        BuildingManager.Instance.SelectBuilding(gameObject);
    }

    public void Select()
    {
        display.Select();
        IsSelected = true;
    }

    public void Unselect()
    {
        display.Unselect();
        IsSelected = false;
    }

    void OnSelect()
    {
        BuildingManager.Instance.SelectBuilding(gameObject);
    }
    #endregion
    
}