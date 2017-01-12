using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeviatanController : PlaneDisplayController
{
    private Color selectedColor;
    public Color defaultColor;
    public GameObject planeModel;
    private GameObject[] meshParts;

    public new void Start()
    {
        meshParts = new GameObject[5];

        // Assigning plane parts for coloring purposes
        meshParts[0] = planeModel.transform.Find("default").transform.Find("meshPart0").gameObject;
        meshParts[1] = planeModel.transform.Find("default").transform.Find("meshPart1").gameObject;
        meshParts[2] = planeModel.transform.Find("default").transform.Find("meshPart2").gameObject;
        meshParts[3] = planeModel.transform.Find("default").transform.Find("meshPart3").gameObject;
        meshParts[4] = planeModel.transform.Find("default").transform.Find("meshPart4").gameObject;

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
        foreach(GameObject meshPart in meshParts)
        {
            foreach(Material mat in meshPart.GetComponent<Renderer>().materials) {
                mat.color = color;
            }
        }
    }
    #endregion
}
