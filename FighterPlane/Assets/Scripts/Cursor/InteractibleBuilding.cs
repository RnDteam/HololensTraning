using System.Linq;
using UnityEngine;

public class InteractibleBuilding : MonoBehaviour {

    private Renderer buildingRenderer;

    [Tooltip("Displays the building information.")]
    public GameObject TextHolder;

    public Color SelectedBuildingColor;

    #region for debug
    //private bool isSelected = false;

    public bool IsSelected = false;
    private bool sentToBuildingManager = false;
    #endregion


    private void Start()
    {
        buildingRenderer = GetComponent<Renderer>();
        SetText();
        TextHolder.SetActive(false);
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

    private void SetColor(Color color)
    {
        foreach (var material in buildingRenderer.materials)
        {
            material.color = color;
        }
    }

    public void Select()
    {
        //isSelected = true;
        SetColor(Color.Lerp(SelectedBuildingColor, Color.white, 0.3f));
        IsSelected = true;
    }

    public void Unselect()
    {
        //isSelected = false;
        SetColor(Color.white);
        IsSelected = false;
    }

    void OnSelect()
    {
        BuildingManager.Instance.SelectBuilding(gameObject);
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
