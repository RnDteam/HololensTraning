using UnityEngine;
using System.Linq;

public class BuildingDisplay : MonoBehaviour {

    [Tooltip("Displays the building information.")]
    public GameObject TextHolder;
    public Color SelectedBuildingColor = Color.cyan;

    private Renderer buildingRenderer;
    private string text = string.Empty;

    private void Awake()
    {
        buildingRenderer = GetComponent<Renderer>();
        SetText();
    }

    #region Selection
    public void Select()
    {
        SetColor(Color.Lerp(SelectedBuildingColor, Color.white, 0.3f));
    }

    public void Unselect()
    {
        SetColor(Color.white);
    }

    private void SetColor(Color color)
    {
        foreach (var material in buildingRenderer.materials)
        {
            material.color = color;
        }
    }

    #endregion

    #region info
    void SetText()
    {
        var buildingInfo = GetComponent<OnlineMapsBuildingBase>().metaInfo;
        if (buildingInfo.Any(p => p.title == "name"))
            text = ReverseHebrewName(buildingInfo.Single(p => p.title == "name").info);
        else text = ReverseHebrewName("בניין כללי");
    }

    public void ShowInfo()
    {
        TextHolder.GetComponent<TextMesh>().text = text;
    }

    public void HideInfo()
    {
        TextHolder.GetComponent<TextMesh>().text = string.Empty;
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
