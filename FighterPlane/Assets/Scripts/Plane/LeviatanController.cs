using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeviatanController : PlaneDisplayController
{
    public GameObject planeModel;
    private GameObject[] meshParts;

    public new void Start()
    {
        meshParts = new GameObject[5];

        // Assigning plane parts for coloring purposes
        meshParts[0] = planeModel.transform.Find("default").transform.Find("default_MeshPart0").gameObject;
        meshParts[1] = planeModel.transform.Find("default").transform.Find("default_MeshPart1").gameObject;
        meshParts[2] = planeModel.transform.Find("default").transform.Find("default_MeshPart2").gameObject;
        meshParts[3] = planeModel.transform.Find("default").transform.Find("default_MeshPart3").gameObject;
        meshParts[4] = planeModel.transform.Find("default").transform.Find("default_MeshPart4").gameObject;

        base.Start();
    }

    #region Selecting Plane
    protected override void ConvertColors(Color color)
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
