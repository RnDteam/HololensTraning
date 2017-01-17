using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HerculesController : PlaneDisplayController {

    public GameObject planeModel;
    private GameObject wings;
    private GameObject mainbody;

    public new void Start()
    {
        // Assigning wings and plane body for color purposes
        wings = planeModel.transform.Find("Wings").gameObject;
        mainbody = planeModel.transform.Find("Main_Body").gameObject;
        
        base.Start();
    }

    #region Selecting Plane
    protected override void ConvertColors(Color color)
    {
        wings.GetComponent<Renderer>().material.color = color;
        mainbody.GetComponent<Renderer>().material.color = color;
    }
    #endregion
}
