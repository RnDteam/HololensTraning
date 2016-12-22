using UnityEngine;
using System.Collections;
using System;

public class PlaneDisplayController : MonoBehaviour {
    
    public bool IsInfoShown {
        get;
        private set;
    }

    public int planeNumber;
    private Color selectedColor;
    public  Color defaultColor;
    public GameObject planeInfo;

    private GameObject wings;
    private GameObject mainbody;
   
    private PhysicsParameters pParams;
    
    void Start () {
        // Assigning wings and plane body for color purposes
        wings = transform.Find("Wings").gameObject;
        mainbody = transform.Find("Main_Body").gameObject;

        pParams = new PhysicsParameters(transform);
        selectedColor = Color.blue;
        ConvertColors(defaultColor);
    }

    void Update()
    {
        // Calculate inforamtion only if text is shown
        if (IsInfoShown)
		{
            // Calculate physics information
            pParams.UpdatePhysics(transform);
            
			DisplayUpdatedInfo();
		}
    }

    private void DisplayUpdatedInfo()
    {
        planeInfo.GetComponent<TextMesh>().text = pParams.ToString();
    }

    #region Selecting Plane
    public void SelectPlane()
    {
        ConvertColors(selectedColor);
    }

    public void DeselectPlane()
    {
		ConvertColors(defaultColor);
    }

    private void ConvertColors(Color color)
    {
        wings.GetComponent<Renderer>().material.color = color;
        mainbody.GetComponent<Renderer>().material.color = color;
    }
    #endregion

    #region Visibility of Plane Details
    public void HidePlaneInfo()
    {
        planeInfo.SetActive(false);
        IsInfoShown = false;
    }

    public void ShowPlaneInfo()
    {
        planeInfo.SetActive(true);
        IsInfoShown = true;
    }
    #endregion
}
