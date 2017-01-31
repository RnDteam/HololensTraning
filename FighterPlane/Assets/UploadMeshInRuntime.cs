using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UploadMeshInRuntime : MonoBehaviour
{
    
    void Start()
    {
        GameObject building;
        Material roofMaterial = (Material)Resources.Load("Assets/Resources/Materials/RoofMaterial", typeof(Material));

        foreach (Transform child in transform)
        {
            building = child.gameObject;
            if (!building.active || !building.name.StartsWith("96"))
            {
                Destroy(building);
            }
            building.GetComponent<MeshFilter>().mesh = (Mesh)Resources.Load("Meshes/mesh" + building.name, typeof(Mesh)); ;

            for (var i=0; i < building.GetComponent<MeshRenderer>().materials.Length; i++)
            {
                building.GetComponent<MeshRenderer>().materials[i] = roofMaterial;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}