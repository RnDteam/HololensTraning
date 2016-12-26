using UnityEngine;

public class InteractibleBuilding : MonoBehaviour {

    private Renderer buildingRenderer;
    private bool previousSelection = false;

    public bool IsSelected = false;
    public BuildingManager buildingManager;

    private void Start()
    {
        buildingRenderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        if (IsSelected != previousSelection)
        {
            UpdateSelection();
        }
        previousSelection = IsSelected;
    }

    private void UpdateSelection()
    {
        if (IsSelected)
        {
            buildingManager.SelectBuilding(gameObject);
            SetColor(Color.red);
        }
        else
        {
            SetColor(Color.white);
        }
    }

    private void SetColor(Color color)
    {
        foreach (var material in buildingRenderer.materials)
        {
            material.color = color;
        }
    }

    void OnSelect()
    {
        IsSelected = !IsSelected;
    }
}
