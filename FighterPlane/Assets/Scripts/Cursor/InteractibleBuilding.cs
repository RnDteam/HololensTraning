using System.Linq;
using UnityEngine;

public class InteractibleBuilding : MonoBehaviour {

    private Renderer buildingRenderer;
    private bool previousSelection = false;

    [Tooltip("Displays the building information.")]
    public GameObject TextHolder;
    public BuildingManager buildingManager;
    public bool IsSelected = false;

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

    #region select
    private void UpdateSelection()
    {
        buildingManager.SelectBuilding(gameObject);
    }

    private void SetColor(Color color)
    {
        foreach (var material in buildingRenderer.materials)
        {
            material.color = color;
        }
    }

    public void Select()
    {
        IsSelected = true;
        previousSelection = true;
        SetColor(Color.red);
    }

    public void Unselect()
    {
        IsSelected = false;
        previousSelection = false;
        SetColor(Color.white);
    }

    void OnSelect()
    {
        IsSelected = !IsSelected;
    }
    #endregion
    
    #region info
    void SetText()
    {
        var buildingInfo = GetComponent<OnlineMapsBuildingBase>().metaInfo;
        if (buildingInfo.Any(p => p.title == "name"))
            TextHolder.GetComponent<TextMesh>().text = ReverseHebrewName(buildingInfo.Single(p => p.title == "name").info);
        else TextHolder.GetComponent<TextMesh>().text = ReverseHebrewName("בניין כללי");
    }

    public void ShowInfo()
    {
        if (TextHolder.GetComponent<TextMesh>().text == string.Empty)
        {
            SetText();
        }
        TextHolder.SetActive(true);
    }

    public void HideInfo()
    {
        TextHolder.SetActive(false);
    }
    #endregion

    #region string utils
    static string ReverseHebrewName(string s)
    {
        if (s.Any(c => IsHebrew(c)))
        {
            return Reverse(s);
        }
        return s;
    }

    static string Reverse(string s)
    {
        char[] charArray = s.ToCharArray();
        return new string(charArray.Reverse().ToArray());
    }

    static bool IsHebrew(char c)
    {
        return "אבגדהוזחטיכלמנסעפצקרשתךםןףץ".Contains(c);
    }
    #endregion
}
