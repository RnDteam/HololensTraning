using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildingManager : MonoBehaviour {

    private List<GameObject> selectedBuildings;

    private void Start()
    {
        selectedBuildings = new List<GameObject>();
    }

    public void SelectBuilding(GameObject gameObject)
    {
        if (selectedBuildings.Contains(gameObject))
        {
            gameObject.GetComponent<InteractibleBuilding>().Unselect();
            selectedBuildings.ForEach(b => b.GetComponent<InteractibleBuilding>().HideInfo());
            selectedBuildings.Remove(gameObject);
        }
        else
        {
            if (selectedBuildings.Count > 0)
            {
                selectedBuildings.ForEach(b => b.GetComponent<InteractibleBuilding>().Unselect());
                selectedBuildings.ForEach(b => b.GetComponent<InteractibleBuilding>().HideInfo());
                selectedBuildings.RemoveRange(0, selectedBuildings.Count);
            }

            gameObject.GetComponent<InteractibleBuilding>().Select();
            gameObject.GetComponent<InteractibleBuilding>().ShowInfo();
            selectedBuildings.Add(gameObject);
        }
    }
}
