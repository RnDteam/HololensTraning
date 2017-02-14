﻿using UnityEngine;
using System.Linq;

public class BuildingDisplay : MonoBehaviour {

    [Tooltip("Displays the building information.")]
    public GameObject TextHolder;
    public Color SelectedBuildingColor = Color.cyan;
    public GameObject ExplosionPrefab;
    public GameObject RuinBuildingPrefab;

    private Renderer buildingRenderer;
    private string text = string.Empty;

    private void Awake()
    {
        buildingRenderer = GetComponent<Renderer>();
        try
        {
            RuinBuildingIfBomed();
        } catch
        {
            Debug.Log("Couldn't ruin building " + gameObject.GetComponent<OnlineMapsBuildingBase>().id);
        }
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

    #region destroy building
    private void RuinBuildingIfBomed()
    {
        if (BuildingManager.Instance.desroidBuildingsList.Contains(gameObject.GetComponent<OnlineMapsBuildingBase>().id))
        {
            RuinBuilding();
        }
    }

    private void RuinBuilding()
    {
        var ruinBuilding = ReplaceInParent(RuinBuildingPrefab);
        buildingRenderer = ruinBuilding.GetComponent<Renderer>();
    }

    //public void BoomBuilding()
    //{
    //    if (!BuildingManager.Instance.desroidBuildingsList.Contains(gameObject.GetComponent<OnlineMapsBuildingBase>().id))
    //    {
    //        var explosion = ReplaceInParent(ExplosionPrefab);
    //        RuinBuilding();
    //        BuildingManager.Instance.desroidBuildingsList.Add(gameObject.GetComponent<OnlineMapsBuildingBase>().id);
    //    }
    //}

    private GameObject ReplaceInParent(GameObject prefab)
    {
        var gameObject = Instantiate(prefab, transform);
        gameObject.transform.localPosition = Vector3.zero;
        gameObject.transform.localRotation = transform.localRotation;
        gameObject.transform.localScale = transform.localScale;
        buildingRenderer.enabled = false;
        return gameObject;
    }
    #endregion

    #region info
    public void SetText()
    {
        var buildingInfo = GetComponent<OnlineMapsBuildingBase>().metaInfo;
        if (buildingInfo.Any(p => p.title == "name"))
        {
            text = ReverseHebrewName(buildingInfo.Single(p => p.title == "name").info);
        }
        else text = ReverseHebrewName("בניין כללי");
        /*BuildingWeapon weapon= GetComponent<BuildingWeapon>();
        if (weapon.Weapon.Equals(Weapon.Rocket))
        {
            text += "\n" + "ליט:קשנ";
        }
        else if (weapon.Equals(Weapon.Missile))
        {
            text += "\n" + "הצצפ:קשנ";
        }*/
    }

    public void ShowInfo()
    {
        TextHolder.GetComponent<TextMesh>().text = text + "\n" + GlobalManager.Reverse("נשק: " + GetComponent<BuildingWeapon>().Weapon.GetHebrewString());
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
            return GlobalManager.Reverse(s);
        }
        return s;
    }
    
    static bool IsHebrew(char c)
    {
        return "אבגדהוזחטיכלמנסעפצקרשתךםןףץ".Contains(c);
    }
    #endregion
}
