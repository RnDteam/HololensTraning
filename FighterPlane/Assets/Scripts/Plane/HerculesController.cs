using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HerculesController : PlaneDisplayController {


    private Color selectedColor;
    public Color defaultColor;
    public GameObject planeModel;
    private GameObject wings;
    private GameObject mainbody;

    public new void Start()
    {
        // Assigning wings and plane body for color purposes
        wings = planeModel.transform.Find("Wings").gameObject;
        mainbody = planeModel.transform.Find("Main_Body").gameObject;

        selectedColor = Color.blue;
        ConvertColors(defaultColor);
        
        base.Start();
    }

    #region Selecting Plane
    public override void SelectPlane()
    {
        ConvertColors(selectedColor);
    }

    public override void DeselectPlane()
    {
        ConvertColors(defaultColor);
    }

    private void ConvertColors(Color color)
    {
        wings.GetComponent<Renderer>().material.color = color;
        mainbody.GetComponent<Renderer>().material.color = color;
    }
    #endregion
}
